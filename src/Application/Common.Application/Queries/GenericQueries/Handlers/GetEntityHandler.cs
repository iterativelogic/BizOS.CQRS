using System.Threading;
using System.Threading.Tasks;
using Common.Application.Contracts;
using Common.Application.Contracts.Commands;
using Common.Domain.Entities;

namespace Common.Application.Queries.GenericQueries.Handlers
{
  public class GetEntityHandler<T> : AbstractQueryHandler<GetEntity<T>, T> where T : Entity
  {
    private readonly IDBContextProvider dbContextProvider;

    public GetEntityHandler(IDBContextProvider dbContextProvider)
    {
      this.dbContextProvider = dbContextProvider;
    }

    public override async Task<T> PerformQueryAsync(GetEntity<T> request, CancellationToken cancellationToken)
    {
      var dbSet = dbContextProvider.GetDBSet<T>();
      return await dbSet.QueryByIdAsync(request.Id, cancellationToken);
    }
  }
}
