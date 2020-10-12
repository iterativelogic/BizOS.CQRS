using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Common.Application.Contracts.Persistance;
using Common.Domain.Entities;

namespace Common.Application
{
  public static class ServiceResolver
  {
    public static void RegisterCommonServices(ContainerBuilder builder, IModelBuilder modelBuilder)
    {
      modelBuilder.OnApplicationStart();
      var assembly = Assembly.GetAssembly(typeof(ApplicationStartup));

      builder
          .RegisterAssemblyTypes(assembly)
          .AsImplementedInterfaces()
          .SingleInstance();
    }

    public static List<Type> GetControllerGenericTypes()
    {
      return new List<Type>() {
        typeof(City),
        typeof(State),
        typeof(Country),
      };
    }
  }
}
