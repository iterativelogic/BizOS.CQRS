namespace Common.Application.Metadata
{
  public class CatalogInfo
  {
    public CatalogInfo(string codeColumn, string descriptionColumn, string tableName = "")
    {
      CodeColumn = codeColumn;
      DescriptionColumn = descriptionColumn;
      TableName = tableName;
    }

    public string CodeColumn { get; }
    public string DescriptionColumn { get; }
    public string TableName { get; }
  }
}