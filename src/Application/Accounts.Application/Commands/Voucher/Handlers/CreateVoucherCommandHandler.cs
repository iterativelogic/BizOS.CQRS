using System;
using System.Threading;
using System.Threading.Tasks;
using Accounts.Application.Commands.Voucher.Validations;
using Accounts.Application.Contracts.Persistance;
using Accounts.Application.Exceptions;
using Accounts.Domain.Entities;
using Common.Application.Contracts.Commands;

namespace Accounts.Application.Commands.Voucher.Handlers
{
  public class CreateVoucherCommandHandler : AbstractCommandHandler<CreateVoucherCommand, Guid>
  {
    private readonly IAccountDBContext accountDBContext;
    private readonly IVoucherValidations voucherValidations;

    public CreateVoucherCommandHandler(
      IAccountDBContext accountDBContext, 
      IVoucherValidations voucherValidations)
    {
      this.accountDBContext = accountDBContext;
      this.voucherValidations = voucherValidations;
    }

    public override async Task<Guid> HandleAsync(CreateVoucherCommand request, CancellationToken cancellationToken)
    {
      Guid voucherId;
      VoucherEntry voucherEntry = MapVoucherEntry(request, out voucherId);

      if (!voucherValidations.IsTotalMatch(voucherEntry))
      {
        throw new VoucherEntryTotalException();
      }

      await accountDBContext.VoucherEntries.InsertAsync(request.TenantId, request.AccountId, cancellationToken, voucherEntry);
      return voucherId;
    }

    private static VoucherEntry MapVoucherEntry(CreateVoucherCommand request, out Guid voucherId)
    {
      var voucherGuid = Guid.NewGuid();

      request.VoucherDetails.ForEach(voucherDetail =>
      {
        voucherDetail.VoucherId = voucherGuid;
        voucherDetail.Id = Guid.NewGuid();
      });
      voucherId = voucherGuid;

      return new VoucherEntry
      {
        CreatedBy = request.AccountId,
        CreatedDate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
        Date = request.Date,
        Description = request.Description,
        Type = request.Type,
        VoucherNo = request.VoucherNo,
        VoucherDetails = request.VoucherDetails,
        Id = voucherId
      };
    }
  }
}
