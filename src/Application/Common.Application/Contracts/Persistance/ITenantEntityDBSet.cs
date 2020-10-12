using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Domain.Entities;
using SqlKata;

namespace Common.Application.Contracts.Persistance
{
  public interface ITenantDBSet<T> where T: TenantEntity
  {
    Query Query { get; }

    Task<IEnumerable<T>> QueryAsync(Guid tenantId, Query query, CancellationToken cancellationToken);

    Task<IEnumerable<T>> QueryAllAsync(Guid tenantId, CancellationToken cancellationToken);

    Task<T> QueryFirstOrDefaultAsync(Guid tenantId, Query query, CancellationToken cancellationToken);

    Task<T> QueryByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);

    Task InsertAsync(Guid tenantId, Guid accountId, CancellationToken cancellationToken, params T[] entities);

    Task UpdateAsync(Guid tenantId, Guid accountId, CancellationToken cancellationToken, params T[] entities);

    Task DeleteAsync(Guid tenantId, Guid accountId, CancellationToken cancellationToken, params T[] entities);

    List<Query> GetInsertQuery(Guid tenantId, params T[] entities);

    List<Query> GetUpdateQuery(Guid tenantId, params T[] entities);

    List<Query> GetDeleteQuery(Guid tenantId, params T[] entities);

    string GetColumnName(string propertyName);

    Query GetSearchQuery(string term);
  }
}
