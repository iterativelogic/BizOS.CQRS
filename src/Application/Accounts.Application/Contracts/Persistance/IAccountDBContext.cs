using Accounts.Domain.Entities;
using Common.Application.Contracts.Persistance;

namespace Accounts.Application.Contracts.Persistance
{
  public interface IAccountDBContext
  {
    IDBSet<AccountType> AccountTypes { get; }

    IDBSet<MeasurementUnit> MeasurementUnits { get; }

    ITenantDBSet<Ledger> Ledgers { get; }

    ITenantDBSet<LedgerGroup> LedgerGroups { get; }

    ITenantDBSet<Product> Products { get; }

    ITenantDBSet<ProductCategory> ProductCategories { get; }

    ITenantDBSet<ProductSubCategory> ProductSubCategories { get; }

    ITenantDBSet<ProductTax> ProductTaxes { get; }

    ITenantDBSet<VoucherEntry> VoucherEntries { get; }

  }
}
