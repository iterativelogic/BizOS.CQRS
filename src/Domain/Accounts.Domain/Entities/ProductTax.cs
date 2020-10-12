using System;
using Common.Domain.Entities;

namespace Accounts.Domain.Entities
{
  public class ProductTax : TenantEntity
  {
    public Guid ProductId { get; set; }
    
    public Guid TaxId { get; set; }

  }
}