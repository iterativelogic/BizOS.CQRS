using System.Threading;
using System.Threading.Tasks;
using Common.Application.Contracts;
using Common.Application.Contracts.Commands;
using Common.Domain.Entities;

namespace Common.Application.Commands.GenericTenantCommand.Handlers
{
  public class UpdateTenantEntityCommandHandler<T> : AbstractCommandHandler<UpdateTenantEntityCommand<T>, T> where T : TenantEntity
  {
    private readonly IDBContextProvider dbContextProvider;

    public UpdateTenantEntityCommandHandler(IDBContextProvider dbContextProvider)
    {
      this.dbContextProvider = dbContextProvider;
    }

    public override async Task<T> HandleAsync(UpdateTenantEntityCommand<T> request, CancellationToken cancellationToken)
    {
      var tenantDBSet = dbContextProvider.GetTenantDBSet<T>();
      request.Model.Id = request.Id;
      await tenantDBSet.UpdateAsync(request.TenantId, request.AccountId, cancellationToken, request.Model);
      return await tenantDBSet.QueryByIdAsync(request.TenantId, request.Id, cancellationToken);
    }
  }
}
