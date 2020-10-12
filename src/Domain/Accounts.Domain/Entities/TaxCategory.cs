using System;
using Common.Domain.Entities;

namespace Accounts.Domain.Entities
{
  public class TaxCategory : TenantEntity
  {
    public string Name { get; set; }

    public float Percentage { get; set; }

    public Guid CessId { get; set; }
  }
}