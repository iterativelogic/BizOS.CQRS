using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Accounts.Application.Queries.GenericQueries.Handlers;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BizOS.Accounts.Controllers;
using Common.Application.Commands.GenericCommand.Handlers;
using Common.Application.Commands.GenericTenantCommand.Handlers;
using Common.Application.Contracts.Persistance;
using Common.Application.Queries.GenericQueries.Handlers;
using FluentValidation.AspNetCore;
using Infrastructure.ApplicationFeatures;
using Infrastructure.Middleware;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AccountServiceResolver = Accounts.Application.ServiceResolver;
using CommonServiceResolver = Common.Application.ServiceResolver;
using InfrastructureServiceResolver = Infrastructure.ServiceResolver;
using SecurityServiceResolver = Security.Application.ServiceResolver;

namespace BizOS.Accounts
{
  public class Startup
  {
    public Startup(IHostEnvironment env)
    {
      var builder = new ConfigurationBuilder()
                      .SetBasePath(env.ContentRootPath)
                      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                      .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    public ILifetimeScope AutofacContainer { get; private set; }

    // ConfigureServices is where you register dependencies. This gets
    // called by the runtime before the ConfigureContainer method, below.
    public void ConfigureServices(IServiceCollection services)
    {
      // Add services to the collection. Don't build or return
      // any IServiceProvider or the ConfigureContainer method
      // won't get called. Don't create a ContainerBuilder
      // for Autofac here, and don't call builder.Populate() - that
      // happens in the AutofacServiceProviderFactory for you.

      List<Type> tenantControllerTypes = GetTenantControllerTypes().ToList();
      List<Type> commonControllerTypes = GetCommonControllerTypes().ToList();

      services
        .AddOptions()
        .AddAuthorization()
        .AddControllers()
        .ConfigureApplicationPartManager(apm =>
        {
          apm.FeatureProviders.Add(new GenericControllerFeatureProvider(commonControllerTypes, tenantControllerTypes));
        })
        .SetCompatibilityVersion(CompatibilityVersion.Latest)
        .AddJsonOptions(options =>
        {
          options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
          options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        });

      services
        .AddMvc()
        .AddFluentValidation();

      services.AddSwaggerDocument();

      AddApiVersioning(services, tenantControllerTypes, commonControllerTypes);
      InfrastructureServiceResolver.RegisterServices(services, Configuration);
      RegisterMediatRRequestHandlers(services);
    }

    private static void AddApiVersioning(IServiceCollection services, List<Type> tenantControllerTypes, List<Type> commonControllerTypes)
    {
      var commonControllerApiVersion = typeof(GenericController<>).GetCustomAttribute<ApiVersionAttribute>();
      var tenantControllerApiVersion = typeof(GenericTenantController<>).GetCustomAttribute<ApiVersionAttribute>();
      services
        .AddApiVersioning(options =>
        {
          tenantControllerTypes.ForEach(closedControllerType => options
                                          .Conventions
                                          .Controller(closedControllerType)
                                          .HasApiVersion(tenantControllerApiVersion.Versions.FirstOrDefault()));

          commonControllerTypes.ForEach(closedControllerType => options
                                          .Conventions
                                          .Controller(closedControllerType)
                                          .HasApiVersion(commonControllerApiVersion.Versions.FirstOrDefault()));
        });
    }


    private void RegisterMediatRRequestHandlers(IServiceCollection services)
    {
      var commonTypes = GetCommonGenericTypes();
      var tenantTypes = GetTenantGenericTypes();

      var genericCommonEntityTypeHandlers = new List<Type> {
        typeof(GetEntitiesHandler<>),
        typeof(GetEntityHandler<>),
        typeof(CreateEntityCommandHandler<>),
        typeof(UpdateEntityCommandHandler<>),
        typeof(DeleteEntityCommandHandler<>)
      };

      var genericTenantEntityTypeHandlers = new List<Type>
      {
        typeof(GetTenantEntitiesHandler<>),
        typeof(GetTenantEntityHandler<>),
        typeof(CreateTenantEntityCommandHandler<>),
        typeof(UpdateTenantEntityCommandHandler<>),
        typeof(DeleteTenantEntityCommandHandler<>)
      };

      RegisterGenericHandlers(services, commonTypes, genericCommonEntityTypeHandlers);
      RegisterGenericHandlers(services, tenantTypes, genericTenantEntityTypeHandlers);

      services.AddMediatR(new Assembly[] {
        typeof(AccountServiceResolver).Assembly,
        typeof(CommonServiceResolver).Assembly,
        typeof(SecurityServiceResolver).Assembly
      });
    }

    private void RegisterGenericHandlers(IServiceCollection services, List<Type> commonTypes, List<Type> genericEntityTypeHandlers)
    {
      commonTypes.ForEach(commonType =>
      {

        var typeInfo = commonType.GetTypeInfo();
        genericEntityTypeHandlers
        .ForEach(commonEntityTypeHandlers =>
        {
          var closedTypeHandler = MakeCloseTypeFromGeneric(typeInfo, commonEntityTypeHandlers);
          services.AddSingleton(closedTypeHandler.GetInterfaces().First(), closedTypeHandler);
        });
      });
    }

    private IEnumerable<Type> GetTenantControllerTypes()
    {
      List<Type> tenantTypes = GetTenantGenericTypes();
      var tenantControllerType = typeof(GenericTenantController<>);
      var tenantControllerTypes = tenantTypes.Select(tenantType => MakeCloseTypeFromGeneric(tenantType.GetTypeInfo(), tenantControllerType));
      return tenantControllerTypes;
    }

    private static List<Type> GetTenantGenericTypes()
    {
      return AccountServiceResolver.GetTenantGenericTypes();
    }

    private IEnumerable<Type> GetCommonControllerTypes()
    {
      List<Type> genericTypes = GetCommonGenericTypes();
      var commonControllerType = typeof(GenericController<>);
      var commonControllerTypes = genericTypes.Select(tenantType => MakeCloseTypeFromGeneric(tenantType.GetTypeInfo(), commonControllerType));
      return commonControllerTypes;
    }

    private static List<Type> GetCommonGenericTypes()
    {
      var genericTypes = CommonServiceResolver.GetControllerGenericTypes();
      genericTypes.AddRange(AccountServiceResolver.GetCommonGenericTypes());
      return genericTypes;
    }

    private Type MakeCloseTypeFromGeneric(TypeInfo entityType, Type classType)
    {
      return classType
              .MakeGenericType(entityType.AsType());
    }

    // ConfigureContainer is where you can register things directly
    // with Autofac. This runs after ConfigureServices so the things
    // here will override registrations made in ConfigureServices.
    // Don't build the container; that gets done for you by the factory.
    public void ConfigureContainer(ContainerBuilder builder)
    {
      // Register your own things directly with Autofac here. Don't
      // call builder.Populate(), that happens in AutofacServiceProviderFactory
      // for you.

      IModelBuilder modelBuilder = new ModelBuilder();
      builder.RegisterInstance(modelBuilder);
      AccountServiceResolver.RegisterAccountServices(builder, modelBuilder);
      SecurityServiceResolver.RegisterSecurityServices(builder, modelBuilder);
      CommonServiceResolver.RegisterCommonServices(builder, modelBuilder);
      InfrastructureServiceResolver.RegisterInfrastructureServices(builder);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseCors(builder => builder
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowAnyOrigin());

      app.UseMiddleware(typeof(ErrorHandlingMiddleware));
      app.UseMiddleware(typeof(TransactionScopeMiddleware));

      app.UseHttpsRedirection();
      app.UseRouting();
      app.UseAuthorization();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });

      app.UseOpenApi();
      app.UseSwaggerUi3();

      AutofacContainer = app.ApplicationServices.GetAutofacRoot();
    }
  }
}
