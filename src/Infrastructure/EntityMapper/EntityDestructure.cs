
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Common.Application.Contracts.Persistance;

namespace Infrastructure.EntityMapper
{
  public class EntityDestructure<T> : Destructure<T>
  {
    private readonly ConcurrentDictionary<string, Func<T, object>> getters;
    private readonly IMetadataProvider metadataProvider;

    public EntityDestructure(List<PropertyInfo> propertyInfoes, IMetadataProvider metadataProvider) {
      getters = new ConcurrentDictionary<string, Func<T, object>>();
      this.metadataProvider = metadataProvider;
      GenerateGetters(propertyInfoes);
    }
    private void GenerateGetters(List<PropertyInfo> propertyInfoes)
    {
      var param = Expression.Parameter(typeof(T));
      foreach (var propertyInfo in propertyInfoes)
      {
        var insertColumn = GetColumnName(propertyInfo.Name);
        var getter = Expression.Lambda<Func<T, object>>(
          Expression.Convert(
            Expression.Property(param, propertyInfo.Name),
            typeof(object)
          ),
          param
        ).Compile();
        getters.TryAdd(insertColumn, getter);
      }
    }

    private string GetColumnName(string column) => metadataProvider.GetColumnName(column,string.Empty);

    public object[] GetObjectValues(ReadOnlyCollection<string> insertColumns, object obj)
    {
      if(obj is T entity)
      {
        return insertColumns.Select(
          column => getters.GetValueOrDefault(column).Invoke(entity)
        ).ToArray();
      }
      return new object[] { };
    }

    public override Func<T, List<object>> GetDestructureFunction(ReadOnlyCollection<string> columns)
    {
      var columnGetters = columns.Select(column => getters.GetValueOrDefault(column)).ToList();
      return (obj) =>
      {
        if (obj is T entity)
        {
          return columnGetters.Select(getter => getter.Invoke(entity)).ToList();
        }
        return new List<object>();
      };
    }

    public Func<T, Dictionary<string, object>> GetParameterFunction(ReadOnlyCollection<string> columns)
    {
      return (obj) =>
      {
        
        if (obj is T entity)
        {
          return columns
          .Select(column => new { column, value=getters.GetValueOrDefault(column).Invoke(entity) })
          .ToDictionary(pair => pair.column, pair => pair.value);
        }
        return null;
      };
    }
  }
}