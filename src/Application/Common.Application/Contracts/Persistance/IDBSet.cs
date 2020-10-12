using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Domain.Entities;
using SqlKata;

namespace Common.Application.Contracts.Persistance
{
  public interface IDBSet<T> where T: Entity
  {
    Query Query { get; }

    Task<IEnumerable<T>> QueryAsync(Query query, CancellationToken cancellationToken);

    Task<T> QueryFirstOrDefaultAsync(Query query, CancellationToken cancellationToken);

    Task<IEnumerable<T>> QueryAllAsync(CancellationToken cancellationToken);

    Task<T> QueryByIdAsync(Guid id, CancellationToken cancellationToken);

    Task InsertAsync(T model, CancellationToken cancellationToken);

    Task UpdateAsync(T model, CancellationToken cancellationToken);

    Task DeleteAsync(T model, CancellationToken cancellationToken);

    string GetColumnName(string propertyName);

    Query GetSearchQuery(string term);
  }
}
