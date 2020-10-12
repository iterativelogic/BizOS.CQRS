using Accounts.Application.Contracts.Persistance;
using Accounts.Domain.Entities;
using Common.Application.Contracts;
using Common.Application.Contracts.Persistance;

namespace Infrastructure.Persistence
{
  public class AccountDBContext : IAccountDBContext
  {
    private readonly IDBContextProvider dbContextProvider;
    private IDBSet<AccountType> accountTypes;
    private IDBSet<MeasurementUnit> measurementUnits;
    private ITenantDBSet<Ledger> ledgers;
    private ITenantDBSet<LedgerGroup> ledgerGroups;
    private ITenantDBSet<Product> products;
    private ITenantDBSet<ProductCategory> productCategories;
    private ITenantDBSet<ProductSubCategory> productSubCategories;
    private ITenantDBSet<ProductTax> productTaxes;
    private ITenantDBSet<VoucherEntry> voucherEntries;

    public AccountDBContext(IDBContextProvider dbContextProvider)
    {
      this.dbContextProvider = dbContextProvider;
    }

    public IDBSet<AccountType> AccountTypes => accountTypes ??= dbContextProvider.GetDBSet<AccountType>();
    
    public IDBSet<MeasurementUnit> MeasurementUnits => measurementUnits ??= dbContextProvider.GetDBSet<MeasurementUnit>();

    public ITenantDBSet<Ledger> Ledgers => ledgers ??= dbContextProvider.GetTenantDBSet<Ledger>();

    public ITenantDBSet<LedgerGroup> LedgerGroups => ledgerGroups ??= dbContextProvider.GetTenantDBSet<LedgerGroup>();

    public ITenantDBSet<Product> Products => products ??= dbContextProvider.GetTenantDBSet<Product>();

    public ITenantDBSet<ProductCategory> ProductCategories => productCategories ??= dbContextProvider.GetTenantDBSet<ProductCategory>();

    public ITenantDBSet<ProductSubCategory> ProductSubCategories => productSubCategories ??= dbContextProvider.GetTenantDBSet<ProductSubCategory>();

    public ITenantDBSet<ProductTax> ProductTaxes => productTaxes ??= dbContextProvider.GetTenantDBSet<ProductTax>();

    public ITenantDBSet<VoucherEntry> VoucherEntries => voucherEntries ??= dbContextProvider.GetTenantDBSet<VoucherEntry>();

  }
}
