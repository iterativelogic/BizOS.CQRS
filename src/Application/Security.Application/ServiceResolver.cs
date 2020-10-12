using System.Reflection;
using Autofac;
using Common.Application.Contracts.Persistance;

namespace Security.Application
{
  public static class ServiceResolver
  {
    public static void RegisterSecurityServices(ContainerBuilder builder, IModelBuilder modelBuilder)
    {
      modelBuilder.OnApplicationStart();
      var assembly = Assembly.GetAssembly(typeof(ServiceResolver));
      builder
          .RegisterAssemblyTypes(assembly)
          .AsImplementedInterfaces()
          .SingleInstance();
    }
  }
}
