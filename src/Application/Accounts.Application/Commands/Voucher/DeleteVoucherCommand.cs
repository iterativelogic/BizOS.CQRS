using System;
using Common.Application.Contracts.Commands;

namespace Accounts.Application.Commands.Voucher
{
  public class DeleteVoucherCommand : TenantCommand<Guid>
  {
    public Guid VoucherId { get; private set; }

    public DeleteVoucherCommand(Guid TenantId, Guid AccountId, Guid VoucherId) : base(TenantId, AccountId)
    {
      this.VoucherId = VoucherId;
    }
  }
}