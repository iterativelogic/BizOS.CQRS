using System.Collections.Generic;
using Accounts.Domain.Entities;
using Common.Application.Contracts.Persistance;
using Common.Application.Metadata;
using Common.Domain.Entities;

namespace Accounts.Application
{
  public static class ApplicationStartup
  {
    public static void OnApplicationStart(this IModelBuilder modelBuilder)
    {
      modelBuilder
        .Entity<AccountType>()
          .Property(x => x.Name)
            .IsSearchColumn();

      modelBuilder
        .Entity<AccountType>()
          .Property(x => x.Id)
            .IsId();

      modelBuilder
        .Entity<Ledger>()
          .Property(x => x.AccountName)
            .IsSearchColumn();

      modelBuilder
        .Entity<LedgerGroup>()
          .Property(x => x.Name)
            .IsSearchColumn();

      modelBuilder
        .Entity<MeasurementUnit>()
          .Property(x => x.Name)
            .IsSearchColumn();

      modelBuilder
        .Entity<Product>()
          .Property(x => x.Name)
            .IsSearchColumn();

      modelBuilder
        .Entity<ProductSubCategory>()
          .Property(x => x.Name)
            .IsSearchColumn();

      modelBuilder
        .Entity<ProductCategory>()
          .Property(x => x.Name)
            .IsSearchColumn();

      modelBuilder
        .Entity<TaxCategory>()
          .Property(x => x.Name)
            .IsSearchColumn();

      modelBuilder.
        Entity<VoucherEntry>()
          .Property(x => x.VoucherDetails)
            .Ignore();
    }

    public static List<EntitySchema> RegisterEntitySchema()
    {
      return new List<EntitySchema> {
        EntitySchema.Create<AccountType>()
        .IsGenericEntity()
        .HasCatalogInfo()
        .HasSearch(nameof(AccountType.Name))
        .HasReference<AccountType>(nameof(AccountType.ParentId))
        .Build(),

        EntitySchema.Create<Ledger>()
        .HasCatalogInfo(nameof(Ledger.Id), nameof(Ledger.AccountName))
        .HasSearch(nameof(Ledger.AccountName))
        .HasReference<LedgerGroup>(nameof(Ledger.LedgerGroupId))
        .HasReference<City>(nameof(Ledger.CityId))
        .HasReference<State>(nameof(Ledger.StateId))
        .HasReference<Country>(nameof(Ledger.CountryId))
        .Build(),

        EntitySchema.Create<LedgerGroup>()
        .IsGenericEntity()
        .HasCatalogInfo()
        .HasSearch(nameof(LedgerGroup.Name))
        .HasReference<AccountType>(nameof(LedgerGroup.AccountTypeId))
        .HasReference<LedgerGroup>(nameof(LedgerGroup.ParentId))
        .Build(),

        EntitySchema.Create<MeasurementUnit>()
        .IsGenericEntity()
        .HasCatalogInfo()
        .HasSearch(nameof(MeasurementUnit.Name))
        .Build(),

        EntitySchema.Create<ProductCategory>()
        .IsGenericEntity()
        .HasCatalogInfo()
        .HasSearch(nameof(ProductCategory.Name))
        .Build(),

        EntitySchema.Create<ProductSubCategory>()
        .IsGenericEntity()
        .HasCatalogInfo()
        .HasSearch(nameof(ProductSubCategory.Name))
        .HasReference<ProductCategory>(nameof(ProductSubCategory.CategoryId))
        .Build(),

        EntitySchema.Create<Product>()
        .IsGenericEntity()
        .HasCatalogInfo()
        .HasSearch(nameof(Product.Name))
        .HasReference<ProductCategory>(nameof(Product.CategoryId))
        .HasReference<ProductSubCategory>(nameof(Product.SubCategoryId))
        .HasReference<Ledger>(nameof(Product.SaleAccountId))
        .HasReference<Ledger>(nameof(Product.PurchaseAccountId))
        .HasReference<MeasurementUnit>(nameof(Product.MeasurementUnitId))
        .Build(),

        EntitySchema.Create<TaxCategory>()
        .IsGenericEntity()
        .HasCatalogInfo()
        .HasSearch(nameof(TaxCategory.Name))
        .HasReference<TaxCategory>(nameof(TaxCategory.CessId))
        .Build(),

      };
    }
  }
}