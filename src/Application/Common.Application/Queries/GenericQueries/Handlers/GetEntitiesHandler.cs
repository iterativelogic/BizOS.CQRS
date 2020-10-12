using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Application.Contracts;
using Common.Application.Contracts.Commands;
using Common.Domain.Entities;

namespace Common.Application.Queries.GenericQueries.Handlers
{
  public class GetEntitiesHandler<T> : AbstractQueryHandler<GetEntities<T>, IEnumerable<T>> where T : Entity
  {
    private readonly IDBContextProvider dbContextProvider;

    public GetEntitiesHandler(
      IDBContextProvider dbContextProvider
      )
    {
      this.dbContextProvider = dbContextProvider;
    }

    public override async Task<IEnumerable<T>> PerformQueryAsync(GetEntities<T> request, CancellationToken cancellationToken)
    {
      var dbSet = dbContextProvider.GetDBSet<T>();
      var query = dbSet.GetSearchQuery(request.SearchTerm);
      return await dbSet.QueryAsync(query, cancellationToken);
    }
  }
}
