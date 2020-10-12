using Common.Application.Contracts;
using Common.Application.Contracts.Persistance;
using Common.Application.Metadata;
using Common.Domain.Entities;

namespace Common.Application
{
  public static class ApplicationStartup
  {
    public static void OnApplicationStart(this IModelBuilder modelBuilder)
    {
      modelBuilder
        .Entity<City>()
          .Property(x => x.Id)
            .IsId();

      modelBuilder
        .Entity<State>()
          .Property(x => x.Id)
            .IsId();

      modelBuilder
        .Entity<Country>()
          .Property(x => x.Id)
            .IsId();
    }
    
    public static void RegisterEntitySchema(IEntitySchemaContainer container)
    {
      container.Register<City>(EntitySchema.Create<City>()
        .IsGenericEntity()
        .HasCatalogInfo()
        .HasReference<State>(nameof(City.StateId))
        .HasReference<Country>(nameof(City.CountryId))
        .Build());

      container.Register<State>(EntitySchema.Create<State>()
        .IsGenericEntity()
        .HasCatalogInfo()
        .HasReference<Country>(nameof(State.CountryId))
        .Build());

      container.Register<Country>(EntitySchema.Create<Country>()
        .IsGenericEntity()
        .HasCatalogInfo()
        .Build());
    }
  }
}