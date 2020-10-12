using System;
using Common.Domain.Entities;

namespace Accounts.Domain.Entities
{   
  public class ProductSubCategory : TenantEntity
  {
    public string Name { get; set; }

    public Guid CategoryId { get; set; }

  }
}