using System;
using System.Collections.Generic;
using Accounts.Domain.Enums;
using Common.Domain.Entities;

namespace Accounts.Domain.Entities
{
  public class VoucherEntry : TenantEntity
  {
    public VoucherEntry()
    {
      VoucherDetails = new List<VoucherDetail>();
    }

    public DateTimeOffset Date { get; set; }

    public int VoucherNo { get; set; }

    public VoucherType Type { get; set; }

    public string Description { get; set; }

    public List<VoucherDetail> VoucherDetails { get; set; }
  }
}