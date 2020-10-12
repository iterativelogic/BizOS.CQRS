using System;
using System.Threading;
using System.Threading.Tasks;
using Accounts.Application.Contracts.Persistance;
using Accounts.Domain.Entities;
using Common.Application.Contracts.Commands;

namespace Accounts.Application.Commands.Voucher.Handlers
{
  public class UpdateVoucherCommandHandler : AbstractCommandHandler<UpdateVoucherCommand, VoucherEntry>
  {
    private readonly IAccountDBContext accountDBContext;

    public UpdateVoucherCommandHandler(IAccountDBContext accountDBContext)
    {
      this.accountDBContext = accountDBContext;
    }

    public override async Task<VoucherEntry> HandleAsync(UpdateVoucherCommand request, CancellationToken cancellationToken)
    {
      var voucherEntry = new VoucherEntry
      {
        LastModifiedBy = request.AccountId,
        LastModifiedDate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
        Date = request.Date,
        Description = request.Description,
        Id= request.Id,
        Type = request.Type,
        VoucherDetails = request.VoucherDetails,
        VoucherNo = request.VoucherNo
      };

      await accountDBContext.VoucherEntries.UpdateAsync(request.TenantId, request.AccountId, cancellationToken, voucherEntry);
      return await accountDBContext.VoucherEntries.QueryByIdAsync(request.TenantId, request.Id, cancellationToken);
    }
  }
}
