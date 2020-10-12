using System.Threading;
using System.Threading.Tasks;
using Common.Application.Contracts;
using Common.Application.Contracts.Commands;
using Common.Application.Queries.GenericTenantQueries;
using Common.Domain.Entities;

namespace Accounts.Application.Queries.GenericQueries.Handlers
{
  public class GetTenantEntityHandler<T> : AbstractQueryHandler<GetTenantEntity<T>, T> where T: TenantEntity
  {
    private readonly IDBContextProvider dbContextProvider;

    public GetTenantEntityHandler(IDBContextProvider dbContextProvider)
    {
      this.dbContextProvider = dbContextProvider;
    }

    public override async Task<T> PerformQueryAsync(GetTenantEntity<T> request, CancellationToken cancellationToken)
    {
      var dbSet = dbContextProvider.GetTenantDBSet<T>();
      return await dbSet.QueryByIdAsync(request.TenantId, request.Id, cancellationToken);
    }
  }
}
