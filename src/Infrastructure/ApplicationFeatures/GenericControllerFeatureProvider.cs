using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Infrastructure.ApplicationFeatures
{
  public class GenericControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
  {
    private readonly IEnumerable<Type> commonControllerTypes;

    private readonly IEnumerable<Type> tenantControllerTypes;

    public GenericControllerFeatureProvider(IEnumerable<Type> commonControllers, IEnumerable<Type> tenantControllers)
    {
      commonControllerTypes = commonControllers;
      tenantControllerTypes = tenantControllers;
    }

    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
      AddGenericController(feature, commonControllerTypes);
      AddGenericController(feature, tenantControllerTypes);
    }

    private void AddGenericController(ControllerFeature feature, IEnumerable<Type> entityTypes)
    {
      foreach (var controllerType in entityTypes)
      {
        feature.Controllers.Add(controllerType.GetTypeInfo());
      }
    }
  }
}
