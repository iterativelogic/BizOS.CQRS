using System;
using System.Collections.Generic;
using Accounts.Domain.Entities;
using Accounts.Domain.Enums;
using Common.Application.Contracts.Commands;
using Common.Domain.Entities;

namespace Accounts.Application.Queries.Voucher
{
  public class GetVouchersQuery : TenantQuery<IEnumerable<VoucherEntry>>
  {
    public GetVouchersQuery(Guid tenantId): base(tenantId)
    {
    }

    public DateRange Range { get; set; }

    public Guid? VoucherId { get; set; }

    public VoucherType? VoucherType { get; set; }
  }
}