namespace Common.Application.Contracts.Persistance
{
  public interface IMetadataProvider
  {
    string ToDatabaseFormat(string name);

    string GetColumnName(string name, string tableName, string schemaName = "public");

    string GetTableName(string name, string schemaName = "public");
  }
}