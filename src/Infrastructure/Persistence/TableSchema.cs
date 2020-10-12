
using System;
using System.Collections.Generic;

namespace Infrastructure.Persistence
{
  public class TableSchema<T>
  {
    public readonly Func<T, List<object>> InsertPart;
    public readonly Func<T, List<object>> UpdatePart;
    public readonly Func<T, Dictionary<string,object>> WherePart;

    public TableSchema(
      List<string> columns,
      List<string> insertColumns, 
      List<string> updateColumns, 
      List<string> primaryKeyColumns,
      List<string> searchColumns,
      Func<T, List<object>> insertPart,
      Func<T, List<object>> updatePart,
      Func<T, Dictionary<string, object>> wherePart,
      string tableName
      )
    {
      Columns = columns;
      SearchColumns = searchColumns;
      InsertColumns = insertColumns;
      UpdateColumns = updateColumns;
      PrimaryKeyColumns = primaryKeyColumns;
      InsertPart = insertPart;
      UpdatePart = updatePart;
      WherePart = wherePart;
      TableName = tableName;
      EntityName = typeof(T).Name;
    }

    public string TableName { get; }

    public string EntityName { get; }

    public List<string> Columns { get; }
    
    public List<string> InsertColumns { get; }
    
    public List<string> UpdateColumns { get; }

    public List<string> PrimaryKeyColumns { get; }

    public List<string> SearchColumns { get; }

  }
}
