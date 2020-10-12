using Common.Application.Contracts;
using Common.Application.Contracts.Persistance;
using Security.Application.Contracts.Persistance;
using Security.Domain.Entities;

namespace Infrastructure.Persistence
{
  public class SecurityDBContext : ISecurityDBContext
  {
    private readonly IDBContextProvider dbContextProvider;

    private ITenantDBSet<User> users;

    private ITenantDBSet<UserCredential> userCredentials;

    public SecurityDBContext(
      IDBContextProvider dbContextProvider)
    {
      this.dbContextProvider = dbContextProvider;
    }

    public ITenantDBSet<User> Users => users ??= dbContextProvider.GetTenantDBSet<User>();

    public ITenantDBSet<UserCredential> UserCredentials => userCredentials ??= dbContextProvider.GetTenantDBSet<UserCredential>();
  }
}
