using System.Linq;
using Common.Application.Contracts.Persistance;

namespace Infrastructure.Persistence
{
  public class MetadataProvider : IMetadataProvider
  {
    private const string schemaName = "public";

    public string GetColumnName(string name, string tableName, string schemaName = MetadataProvider.schemaName)
    {
      if (string.IsNullOrEmpty(tableName))
      {
        return ToDatabaseFormat(name);
      }
      else
      {
        return $"{tableName}.{ToDatabaseFormat(name)}";
      }
    }
    
    public string GetTableName(string name, string schemaName = MetadataProvider.schemaName)
    {
      return $"{schemaName}.{ToDatabaseFormat(name)}";
    }

    public string ToDatabaseFormat(string name)
    {
      return string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLowerInvariant();
    }
  }
}
