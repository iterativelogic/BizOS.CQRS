using System;
using Common.Domain.Entities;

namespace Accounts.Domain.Entities
{
  public class Product : TenantEntity
  {
    public string Name { get; set; }

    public string Description { get; set; }

    public Guid CategoryId { get; set; }

    public Guid SubCategoryId { get; set; }

    public Guid SaleAccountId {get; set;}

    public Guid PurchaseAccountId {get; set;}

    public string BarCode { get; set; }

    public string ReferenceCode { get; set; }

    public string Size { get; set; }

    public Guid? MeasurementUnitId { get; set; }

    public bool IsAvailable { get; set; }

    public int SafetyStock { get; set; }

    public int ReorderPoint { get; set; }

    public double Price { get; set; }

  }
}