using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Application.Contracts;
using Common.Application.Contracts.Commands;
using Common.Domain.Entities;

namespace Common.Application.Commands.GenericCommand.Handlers
{
  public class DeleteEntityCommandHandler<T> : AbstractCommandHandler<DeleteEntityCommand<T>, Guid> where T : Entity
  {
    private readonly IDBContextProvider dbContextProvider;

    public DeleteEntityCommandHandler(IDBContextProvider dbContextProvider)
    {
      this.dbContextProvider = dbContextProvider;
    }

    public async override Task<Guid> HandleAsync(DeleteEntityCommand<T> request, CancellationToken cancellationToken)
    {
      var dbSet = dbContextProvider.GetDBSet<T>();
      var entity = await dbSet.QueryByIdAsync(request.Id, cancellationToken);
      await dbSet.DeleteAsync(entity, cancellationToken);
      return request.Id;
    }
  }
}
