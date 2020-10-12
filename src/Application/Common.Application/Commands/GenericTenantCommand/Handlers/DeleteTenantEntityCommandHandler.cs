using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Application.Contracts;
using Common.Application.Contracts.Commands;
using Common.Domain.Entities;

namespace Common.Application.Commands.GenericTenantCommand.Handlers
{
  public class DeleteTenantEntityCommandHandler<T> : AbstractCommandHandler<DeleteTenantEntityCommand<T>, Guid> where T : TenantEntity
  {
    private readonly IDBContextProvider dbContextProvider;

    public DeleteTenantEntityCommandHandler(IDBContextProvider dbContextProvider)
    {
      this.dbContextProvider = dbContextProvider;
    }

    public override async Task<Guid> HandleAsync(DeleteTenantEntityCommand<T> request, CancellationToken cancellationToken)
    {
      var dbSet = dbContextProvider.GetDBSet<T>();
      var entity = await dbSet.QueryByIdAsync(request.Id, cancellationToken);
      await dbSet.DeleteAsync(entity, cancellationToken);
      return request.Id;
    }
  }
}
