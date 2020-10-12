using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Infrastructure.ApplicationFeatures
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
  public class GenericControllerNameConventionAttribute : Attribute, IControllerModelConvention
  {
    private readonly Type genericControllerType;

    public GenericControllerNameConventionAttribute(Type genericControllerType)
    {
      this.genericControllerType = genericControllerType;
    }
    public void Apply(ControllerModel controller)
    {
      if (controller.ControllerType.IsGenericType &&
            controller.ControllerType.GetGenericTypeDefinition() == genericControllerType
               )
      {
        var entityType = controller.ControllerType.GenericTypeArguments[0];
        controller.ControllerName = entityType.Name;
      }
    }
  }
}
