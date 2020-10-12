using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Application.Contracts;
using Common.Application.Contracts.Commands;
using Common.Domain.Entities;

namespace Common.Application.Commands.GenericCommand.Handlers
{
  public class CreateEntityCommandHandler<T> : AbstractCommandHandler<CreateEntityCommand<T>, T> where T : Entity
  {
    private readonly IDBContextProvider dbContextProvider;

    public CreateEntityCommandHandler(IDBContextProvider dbContextProvider)
    {
      this.dbContextProvider = dbContextProvider;
    }

    public async override Task<T> HandleAsync(CreateEntityCommand<T> request, CancellationToken cancellationToken)
    {
      var dbSet = dbContextProvider.GetDBSet<T>();
      request.Model.Id = Guid.NewGuid();
      await dbSet.InsertAsync(request.Model, cancellationToken);
      return await dbSet.QueryByIdAsync(request.Model.Id, cancellationToken);
    }
  }
}
