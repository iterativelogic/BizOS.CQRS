using Common.Domain.Entities;

namespace Accounts.Domain.Entities
{
  public class ProductCategory : TenantEntity
  {
    public string Name { get; set; }

    public bool IsInventory { get; set; }

  }
}