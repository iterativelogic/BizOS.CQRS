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
  public class TenantDBSet<T> : DBSet<T>, ITenantDBSet<T> where T: TenantEntity
  {
    private readonly string tenantIdColumn;
    public TenantDBSet(IConnectionProvider connectionProvider,
                   IMetadataProvider metadataProvider,
                   TableSchema<T> tableSchema) : base(connectionProvider, metadataProvider, tableSchema)
    {
      tenantIdColumn = metadataProvider.GetColumnName(Constants.TenantIdFieldName, tableSchema.TableName);
    }

    public virtual async Task<IEnumerable<T>> QueryAsync(Guid tenantId, Query query, CancellationToken cancellationToken)
    {
      query = query.Where(metadataProvider.GetColumnName(Constants.TenantIdFieldName, tableSchema.TableName), tenantId);
      return await base.QueryAsync(query, cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> QueryAllAsync(Guid tenantId, CancellationToken cancellationToken)
    {
      return await QueryAsync(tenantId, Query, cancellationToken);
    }

    public virtual async Task<T> QueryFirstOrDefaultAsync(Guid tenantId, Query query, CancellationToken cancellationToken)
    {
      var entities = await QueryAsync(tenantId, query, cancellationToken);
      return entities.FirstOrDefault();
    }

    public virtual async Task<T> QueryByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
    {
      var query = Query.Where(metadataProvider.GetColumnName(nameof(Entity.Id), tableSchema.TableName), id);
      return await QueryFirstOrDefaultAsync(tenantId, query, cancellationToken);
    }

    public virtual async Task InsertAsync(Guid tenantId, Guid accountId, CancellationToken cancellationToken, params T[] entities)
    {
      foreach(var entity in entities)
      {
        entity.CreatedBy = accountId;
        entity.CreatedDate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
      }
      
      var insertQuery = GetInsertQuery(tenantId, entities);
      var sqlCompiled = connectionProvider.QueryCompiler.Compile(insertQuery);
      using (var connection = await connectionProvider.GetConnectionAsync(cancellationToken))
      {
        await connection.ExecuteAsync(sqlCompiled.Sql, sqlCompiled.NamedBindings);
      }
    }

    public virtual async Task UpdateAsync(Guid tenantId, Guid accountId, CancellationToken cancellationToken, params T[] entities)
    {
      foreach (var entity in entities)
      {
        entity.LastModifiedBy = accountId;
        entity.LastModifiedDate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
      }

      var updateQuery = GetUpdateQuery(tenantId, entities);
      var sqlCompiled = connectionProvider.QueryCompiler.Compile(updateQuery);
      using (var connection = await connectionProvider.GetConnectionAsync(cancellationToken))
      {
        await connection.ExecuteAsync(sqlCompiled.Sql, sqlCompiled.NamedBindings);
      }
    }

    public virtual async Task DeleteAsync(Guid tenantId, Guid accountId, CancellationToken cancellationToken, params T[] entities)
    {
      var deleteQuery = GetDeleteQuery(tenantId, entities);
      var sqlCompiled = connectionProvider.QueryCompiler.Compile(deleteQuery);
      using (var connection = await connectionProvider.GetConnectionAsync(cancellationToken))
      {
        await connection.ExecuteAsync(sqlCompiled.Sql, sqlCompiled.NamedBindings);
      }
    }

    public virtual List<Query> GetInsertQuery(Guid tenantId,  params T[] entities)
    {
      var destructuredObjects = entities.Select(model => tableSchema.InsertPart(model)).ToList();
      destructuredObjects.ForEach(objectArr => objectArr.Insert(0, tenantId));

      var insertQuery = new Query(tableSchema.TableName)
                            .AsInsert(
                              tableSchema.InsertColumns,
                              destructuredObjects);     
      return new List<Query> { insertQuery };
    }

    public virtual List<Query> GetUpdateQuery(Guid tenantId,  params T[] entities)
    {
      var updateQueries = base.GetUpdateQuery(entities);
      AppendTenantIdCondition(tenantId, updateQueries);
      return updateQueries;
    }

    public virtual List<Query> GetDeleteQuery(Guid tenantId, params T[] entities)
    {
      var deleteQueries = base.GetDeleteQuery(entities);
      AppendTenantIdCondition(tenantId, deleteQueries);
      return deleteQueries;
    }

    protected void AppendTenantIdCondition(Guid tenantId, List<Query> queries)
    {
      queries.ForEach(query => query.Where(tenantIdColumn, tenantId));
    }
  }
}