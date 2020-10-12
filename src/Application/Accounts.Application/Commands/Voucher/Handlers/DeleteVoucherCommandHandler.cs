using System;
using System.Threading;
using System.Threading.Tasks;
using Accounts.Application.Contracts.Persistance;
using Common.Application.Contracts.Commands;

namespace Accounts.Application.Commands.Voucher.Handlers
{
  public class DeleteVoucherCommandHandler : AbstractCommandHandler<DeleteVoucherCommand, Guid>
  {
    private readonly IAccountDBContext accountDBContext;

    public DeleteVoucherCommandHandler(IAccountDBContext accountDBContext)
    {
      this.accountDBContext = accountDBContext;
    }

    public override async Task<Guid> HandleAsync(DeleteVoucherCommand request, CancellationToken cancellationToken)
    {
      var voucherEntry = await accountDBContext.VoucherEntries.QueryByIdAsync(request.TenantId, request.VoucherId, cancellationToken);
      await accountDBContext.VoucherEntries.DeleteAsync(request.TenantId, request.AccountId, cancellationToken, voucherEntry);
      return voucherEntry.Id;
    }
  }
}
