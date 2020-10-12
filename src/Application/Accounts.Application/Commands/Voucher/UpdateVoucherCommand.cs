using System;
using System.Collections.Generic;
using Accounts.Domain.Entities;
using Accounts.Domain.Enums;
using Common.Application.Contracts.Commands;

namespace Accounts.Application.Commands.Voucher
{
  public class UpdateVoucherCommand : TenantCommand<VoucherEntry>
  {
    public Guid Id { get; set; }

    public DateTimeOffset Date { get; set; }

    public int VoucherNo { get; set; }

    public VoucherType Type { get; set; }

    public string Description { get; set; }

    public List<VoucherDetail> VoucherDetails { get; set; }

    public UpdateVoucherCommand(Guid tenantId, Guid accountId) : base(tenantId, accountId)
    { }
  }
}