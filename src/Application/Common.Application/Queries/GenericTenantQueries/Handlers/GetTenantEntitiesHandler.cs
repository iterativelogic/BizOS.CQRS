using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Application.Contracts;
using Common.Application.Contracts.Commands;
using Common.Application.Queries.GenericTenantQueries;
using Common.Domain.Entities;

namespace Accounts.Application.Queries.GenericQueries.Handlers
{
  public class GetTenantEntitiesHandler<T> : AbstractQueryHandler<GetTenantEntities<T>, IEnumerable<T>> where T: TenantEntity
  {
    private readonly IDBContextProvider dbContextProvider;

    public GetTenantEntitiesHandler(
      IDBContextProvider dbContextProvider
      )
    {
      this.dbContextProvider = dbContextProvider;
    }

    public override async Task<IEnumerable<T>> PerformQueryAsync(GetTenantEntities<T> request, CancellationToken cancellationToken)
    {
      var dbSet = dbContextProvider.GetTenantDBSet<T>();
      var query = dbSet.GetSearchQuery(request.SearchTerm);
      return await dbSet.QueryAsync(request.TenantId, query, cancellationToken);
    }
  }
}
