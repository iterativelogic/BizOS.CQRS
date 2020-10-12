using System.Reflection;
using System.Text;
using Autofac;
using Infrastructure.Contracts;
using Infrastructure.Persistence;
using Infrastructure.Validations;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Security.Application.Contracts;

namespace Infrastructure
{
  public static class ServiceResolver
  {
    public static void RegisterInfrastructureServices(ContainerBuilder builder)
    {
      var assembly = Assembly.GetAssembly(typeof(ApplicationStartup));

      builder
          .RegisterAssemblyTypes(assembly)
          .Where(type => type != typeof(ModelBuilder))
          .AsImplementedInterfaces()
          .SingleInstance();
    }

    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
      Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
      services.Configure<DatabaseOptions>(configuration.GetSection(AppSettings.DatabaseOptions));
      services.Configure<SecurityOptions>(configuration.GetSection(AppSettings.SecurityOptions));
      var appSettings = configuration.GetSection(AppSettings.SecurityOptions).Get<SecurityOptions>();
      var key = Encoding.ASCII.GetBytes(appSettings.Secret);

      services.AddAuthorization(auth => auth
        .AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
        .RequireAuthenticatedUser().Build())
      );

      services.AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(x =>
      {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false,
          ValidateLifetime = true
        };
      });

      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
    }
  }
}
