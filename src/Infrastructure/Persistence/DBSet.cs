using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Application.Contracts.Persistance;
using Common.Domain.Entities;
using Dapper;
using Infrastructure.Contracts;
using SqlKata;


namespace Infrastructure.Persistence
{
  public class DBSet<T> : IDBSet<T> where T : Entity
  {
    protected readonly IConnectionProvider connectionProvider;
    protected readonly IMetadataProvider metadataProvider;
    protected readonly TableSchema<T> tableSchema;

#pragma warning disable CA1819 // Properties should not return arrays
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
    protected virtual Type[] ChildTypes => null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
#pragma warning restore CA1819 // Properties should not return arrays

    protected virtual string splitOn => null;

    public virtual Query Query => new Query(tableSchema.TableName);

    public DBSet(IConnectionProvider connectionProvider,
                   IMetadataProvider metadataProvider,
                   TableSchema<T> tableSchema)
    {
      this.connectionProvider = connectionProvider;
      this.metadataProvider = metadataProvider;
      this.tableSchema = tableSchema;
    }

    public virtual async Task<IEnumerable<T>> QueryAsync(Query query, CancellationToken cancellationToken)
    {
      var sqlCompiled = connectionProvider.QueryCompiler.Compile(query);
      using (var connection = await connectionProvider.GetConnectionAsync(cancellationToken))
      {
        var mapper = GetMapper();
        if (mapper != null)
        {
          return await connection.QueryAsync<T>(sqlCompiled.Sql, ChildTypes, GetMapper(), sqlCompiled.NamedBindings, splitOn: splitOn);
        }
        return await connection.QueryAsync<T>(sqlCompiled.Sql, sqlCompiled.NamedBindings);
      }
    }

    public virtual async Task<IEnumerable<T>> QueryAllAsync(CancellationToken cancellationToken)
    {
      return await QueryAsync(Query, cancellationToken);
    }

    public virtual async Task<T> QueryFirstOrDefaultAsync(Query query, CancellationToken cancellationToken)
    {
      var entities = await QueryAsync(query, cancellationToken);
      return entities.FirstOrDefault();
    }

    public virtual async Task<T> QueryByIdAsync(Guid id, CancellationToken cancellationToken)
    {
      var query = Query.Where(metadataProvider.GetColumnName(nameof(Entity.Id), tableSchema.TableName), id);
      return await QueryFirstOrDefaultAsync(query, cancellationToken);
    }

    public virtual async Task InsertAsync(T model, CancellationToken cancellationToken)
    {
      var insertQuery = GetInsertQuery(model);
      var sqlCompiled = connectionProvider.QueryCompiler.Compile(insertQuery);
      using (var connection = await connectionProvider.GetConnectionAsync(cancellationToken))
      {
        await connection.ExecuteAsync(sqlCompiled.Sql, sqlCompiled.NamedBindings);
      }
    }

    public virtual async Task UpdateAsync(T model, CancellationToken cancellationToken)
    {
      var updateQuery = GetUpdateQuery(model);
      var sqlCompiled = connectionProvider.QueryCompiler.Compile(updateQuery);
      using (var connection = await connectionProvider.GetConnectionAsync(cancellationToken))
      {
        await connection.ExecuteAsync(sqlCompiled.Sql, sqlCompiled.NamedBindings);
      }
    }

    public virtual async Task DeleteAsync(T model, CancellationToken cancellationToken)
    {
      var deleteQuery = GetDeleteQuery(model);
      var sqlCompiled = connectionProvider.QueryCompiler.Compile(deleteQuery);
      using (var connection = await connectionProvider.GetConnectionAsync(cancellationToken))
      {
        await connection.ExecuteAsync(sqlCompiled.Sql, sqlCompiled.NamedBindings);
      }
    }

    public virtual List<Query> GetInsertQuery(params T[] models)
    {
      var insertQuery = new Query(tableSchema.TableName);
      insertQuery.AsInsert(
        tableSchema.InsertColumns, 
        models.Select(model => tableSchema.InsertPart(model)));

      return new List<Query> { insertQuery };
    }

    public virtual List<Query> GetUpdateQuery(params T[] models)
    { 
      return models
              .Select(model => new Query(tableSchema.TableName)
                                  .Where(tableSchema.WherePart(model))
                                  .AsUpdate(tableSchema.UpdateColumns, tableSchema.UpdatePart(model)))
              .ToList();

    }

    public virtual List<Query> GetDeleteQuery(params T[] models)
    {
      return models
            .Select(model => new Query(tableSchema.TableName)
                        .Where(tableSchema.WherePart(model))
                        .AsDelete())
            .ToList();
    }
  
    protected virtual Func<object[], T> GetMapper()
    {
      return null;
    }

    public string GetColumnName(string propertyName)
    {
      return metadataProvider.GetColumnName(propertyName, tableSchema.TableName);
    }

    public Query GetSearchQuery(string term)
    {
      var query = Query;
      if (!string.IsNullOrWhiteSpace(term))
      {
        tableSchema.SearchColumns.ForEach(searchColumn => query.WhereContains(searchColumn, term));
      }
      return query;
    }
  }
}
