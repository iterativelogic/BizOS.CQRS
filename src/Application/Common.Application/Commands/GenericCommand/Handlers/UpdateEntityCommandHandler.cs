using System.Threading;
using System.Threading.Tasks;
using Common.Application.Contracts;
using Common.Application.Contracts.Commands;
using Common.Domain.Entities;

namespace Common.Application.Commands.GenericCommand.Handlers
{
  public class UpdateEntityCommandHandler<T> : AbstractCommandHandler<UpdateEntityCommand<T>, T> where T : Entity
  {
    private readonly IDBContextProvider dbContextProvider;

    public UpdateEntityCommandHandler(IDBContextProvider dbContextProvider)
    {
      this.dbContextProvider = dbContextProvider;
    }

    public override async Task<T> HandleAsync(UpdateEntityCommand<T> request, CancellationToken cancellationToken)
    {
      var dbSet = dbContextProvider.GetDBSet<T>();
      request.Model.Id = request.Id;
      await dbSet.UpdateAsync(request.Model, cancellationToken);
      return await dbSet.QueryByIdAsync(request.Id, cancellationToken);
    }
  }
}
