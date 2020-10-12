using System.Collections.Generic;
using Common.Application.Contracts.Persistance;
using Common.Domain.Entities;

namespace Infrastructure.Persistence
{
  public class ModelBuilder : IModelBuilder
  {
    public Dictionary<string, object> EntitySchemas { get; }

    public ModelBuilder()
    {
      EntitySchemas = new Dictionary<string, object>();
    }

    public IEntitySchema<T> Entity<T>()
    {
      if (!EntitySchemas.TryGetValue(typeof(T).FullName, out var entitySchema))
      {
        entitySchema = new EntitySchema<T>();
        if(typeof(TenantEntity).IsAssignableFrom(typeof(T)))
        {
          (entitySchema as EntitySchema<T>).IsTenantProperty();
        }
        EntitySchemas.Add(typeof(T).FullName, entitySchema);
      }
      return entitySchema as EntitySchema<T>;
    }
  }
}
