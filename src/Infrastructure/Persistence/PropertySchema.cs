using System.Reflection;
using Common.Application.Contracts.Persistance;

namespace Infrastructure.Persistence
{
  public class PropertySchema : IPropertySchema
  {
    public readonly PropertyInfo PropertyInfo;

    public bool IsIgnore { get; private set; }

    public bool IsIgnoreOnInsert { get; private set; }

    public bool IsIgnoreOnUpdate { get; private set; }

    public bool IsSearch { get; private set; }

    public bool IsPrimaryKey { get; private set; }

    public PropertySchema(PropertyInfo propertyInfo)
    {
      PropertyInfo = propertyInfo;
    }

    public IPropertySchema Ignore()
    {
      IsIgnore = true;
      IsIgnoreOnInsert = true;
      IsIgnoreOnUpdate = true;
      return this;
    }

    public IPropertySchema IgnoreOnInsert()
    {
      IsIgnoreOnInsert = true;
      return this;
    }

    public IPropertySchema IgnoreOnUpdate()
    {
      IsIgnoreOnUpdate = true;
      return this;
    }

    public IPropertySchema IsSearchColumn()
    {
      IsSearch = true;
      return this;
    }

    public IPropertySchema IsId()
    {
      IsPrimaryKey = true;
      return this;
    }
  }
}
