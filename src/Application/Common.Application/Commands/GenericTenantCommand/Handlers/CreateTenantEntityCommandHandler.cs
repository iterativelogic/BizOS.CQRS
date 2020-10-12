using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Application.Contracts;
using Common.Application.Contracts.Commands;
using Common.Domain.Entities;

namespace Common.Application.Commands.GenericTenantCommand.Handlers
{
  public class CreateTenantEntityCommandHandler<T> : AbstractCommandHandler<CreateTenantEntityCommand<T>, T> where T : TenantEntity
  {
    private readonly IDBContextProvider dbContextProvider;

    public CreateTenantEntityCommandHandler(IDBContextProvider dbContextProvider)
    {
      this.dbContextProvider = dbContextProvider;
    }

    public async override Task<T> HandleAsync(CreateTenantEntityCommand<T> request, CancellationToken cancellationToken)
    {
      var tenantDBContext = dbContextProvider.GetTenantDBSet<T>();
      request.Model.Id = Guid.NewGuid();
      await tenantDBContext.InsertAsync(request.TenantId, request.AccountId, cancellationToken, request.Model);
      return await tenantDBContext.QueryByIdAsync(request.TenantId, request.Model.Id, cancellationToken);
    }
  }
}
