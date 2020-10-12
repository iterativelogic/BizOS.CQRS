using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Common.Application.Contracts.Persistance;
using Common.Domain.Entities;
using Infrastructure.EntityMapper;

namespace Infrastructure.Persistence
{
  public class EntitySchema<T> : IEntitySchema<T>
  {
    private Dictionary<string, PropertySchema> propertySchemas { get; }

    private bool IsTenantEntity { get; set; }
    private string tableName { get; set; }

    public EntitySchema()
    {
      propertySchemas = new Dictionary<string, PropertySchema>();
    }

    public IPropertySchema Property(Expression<Func<T, object>> keyExpression)
    {
      var propertyInfo = keyExpression.GetPropertyInfo();
      if (!propertySchemas.TryGetValue(propertyInfo.Name, out var propertySchema))
      {
        propertySchema = new PropertySchema(propertyInfo);
        propertySchemas.Add(propertyInfo.Name, propertySchema);
      }

      return propertySchema;
    }

    public IEntitySchema<T> HasTableName(string tableName)
    {
      this.tableName = tableName;
      return this;
    }

    public IEntitySchema<T> IsTenantProperty()
    {
      IsTenantEntity = true;
      return this;
    }

    public TableSchema<T> BuildSchema(IMetadataProvider metadataProvider)
    {
      List<string> ignoreColumnsList = new List<string> {
        nameof(Entity.Descriptions),
        nameof(TenantEntity.RowVersion)
      };

      List<string> ignoreInInsert = new List<string>
      {
        nameof(TenantEntity.LastModifiedBy),
        nameof(TenantEntity.LastModifiedDate)
      };

      List<string> ignoreInUpdate = new List<string>
      {
        nameof(TenantEntity.CreatedBy),
        nameof(TenantEntity.CreatedDate)
      };

      List<PropertyInfo> propertyInfos = typeof(T)
                                            .GetProperties()
                                            .Where(prop => !ignoreColumnsList.Contains(prop.Name) && !(propertySchemas.TryGetValue(prop.Name, out var propSchema)
                                                                  && propSchema.IsIgnore))
                                            .ToList();

      List<PropertyInfo> insertPropertyInfos = propertyInfos
                                                .Where(prop => !ignoreInInsert.Contains(prop.Name) && !(propertySchemas.TryGetValue(prop.Name, out var propSchema)
                                                                        && propSchema.IsIgnoreOnInsert))
                                                .ToList();

      List<PropertyInfo> primaryKeys = propertyInfos
                                        .Where(prop => propertySchemas.TryGetValue(prop.Name, out var propSchema) && propSchema.IsPrimaryKey)
                                        .ToList();

      List<PropertyInfo> updatePropertyInfos = propertyInfos
                                                .Where(prop => !primaryKeys.Contains(prop) && !ignoreInUpdate.Contains(prop.Name) && !(propertySchemas.TryGetValue(prop.Name, out var propSchema)
                                                                    && propSchema.IsIgnoreOnUpdate
                                                                    && !propSchema.IsPrimaryKey))
                                                .ToList();

      

      return GetTableSchema(metadataProvider,
                              propertyInfos,
                              insertPropertyInfos,
                              updatePropertyInfos,
                              primaryKeys);
    }

    private TableSchema<T> GetTableSchema(IMetadataProvider metadataProvider,
      List<PropertyInfo> propertyInfos, 
      List<PropertyInfo> insertPropertyInfos,
      List<PropertyInfo> updatePropertyInfos,
      List<PropertyInfo> primaryKeys)
    {
      List<PropertyInfo> searchColumnProps = propertyInfos
                                                      .Where(prop => propertySchemas.TryGetValue(prop.Name, out var propSchema) && propSchema.IsSearch)
                                                      .ToList();


      tableName = metadataProvider.GetTableName(tableName ?? typeof(T).Name);

      var primaryKeyColumns = GetDBFormatted(primaryKeys, metadataProvider);
      var columns = GetColumnNames(propertyInfos, metadataProvider, tableName);
      var insertColumns = GetDBFormatted(insertPropertyInfos, metadataProvider);
      var updateColumns = GetDBFormatted(updatePropertyInfos, metadataProvider);
      var searchColumns = GetColumnNames(searchColumnProps, metadataProvider, tableName);


      var destructure = new EntityDestructure<T>(propertyInfos, metadataProvider);

      var insertPart = destructure.GetDestructureFunction(new ReadOnlyCollection<string>(insertColumns));
      var updatePart = destructure.GetDestructureFunction(new ReadOnlyCollection<string>(updateColumns));
      var wherePart = destructure.GetParameterFunction(new ReadOnlyCollection<string>(primaryKeyColumns));

      HandleTenantEntity(metadataProvider, insertColumns);

      return new TableSchema<T>(
        columns, insertColumns, updateColumns, primaryKeyColumns, searchColumns, 
        insertPart, updatePart, wherePart, tableName);
    }

    private void HandleTenantEntity(IMetadataProvider metadataProvider, List<string> insertColumns)
    {
      if (IsTenantEntity)
      {
        var tenantIdColumn = metadataProvider.ToDatabaseFormat(Constants.TenantIdFieldName);
        insertColumns.Insert(0, tenantIdColumn);
      }
    }

    private static List<string> GetDBFormatted(List<PropertyInfo> propertyInfos, IMetadataProvider metadataProvider)
    {
      return propertyInfos.Select(prop => metadataProvider.ToDatabaseFormat(prop.Name)).ToList();
    }

    private static List<string> GetColumnNames(List<PropertyInfo> propertyInfos, IMetadataProvider metadataProvider, string tableName)
    {
      return propertyInfos.Select(prop => metadataProvider.GetColumnName(prop.Name, tableName)).ToList();
    }
  }
}
