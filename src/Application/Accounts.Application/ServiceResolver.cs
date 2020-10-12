using System;
using System.Collections.Generic;
using System.Reflection;
using Accounts.Domain.Entities;
using Autofac;
using Common.Application.Contracts.Persistance;

namespace Accounts.Application
{
  public static class ServiceResolver
  {
    public static void RegisterAccountServices(ContainerBuilder builder, IModelBuilder modelBuilder)
    {
      modelBuilder.OnApplicationStart();
      
      var assembly = Assembly.GetAssembly(typeof(ApplicationStartup));

      builder
          .RegisterAssemblyTypes(assembly)
          .AsImplementedInterfaces()
          .SingleInstance();
    }

    public static List<Type> GetCommonGenericTypes()
    { 
      return new List<Type>() {
        typeof(AccountType),
        typeof(MeasurementUnit)
      };
    }

    public static List<Type> GetTenantGenericTypes()
    {
      return new List<Type>()
      {
        typeof(Ledger),
        typeof(LedgerGroup),
        typeof(Product),
        typeof(ProductCategory),
        typeof(ProductSubCategory),
        typeof(ProductTax),
        typeof(TaxCategory)
      };
    }
  }
}
