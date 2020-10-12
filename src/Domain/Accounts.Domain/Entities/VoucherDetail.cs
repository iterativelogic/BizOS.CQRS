using System;
using Common.Domain.Entities;

namespace Accounts.Domain.Entities
{
  public class VoucherDetail : TenantEntity
  {
    public Guid VoucherId { get; set; }

    public Guid LedgerId { get; set; }

    public double Debit { get; set; }

    public double Credit { get; set; }

  }
}