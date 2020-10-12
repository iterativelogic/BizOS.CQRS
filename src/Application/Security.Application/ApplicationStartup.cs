using Common.Application.Contracts.Persistance;
using Security.Domain.Entities;

namespace Security.Application
{
  public static class ApplicationStartup
  {
    public static void OnApplicationStart(this IModelBuilder modelBuilder)
    {
      modelBuilder
        .Entity<User>()
        .HasTableName("Users")
          .Property(x => x.Id)
            .IsId();

      modelBuilder
        .Entity<UserCredential>()
          .HasTableName("Users")
          .Property(x => x.Id)
            .IsId();
    }
  }
}
