using Common.Application.Contracts.Persistance;
using Security.Domain.Entities;

namespace Security.Application.Contracts.Persistance
{
  public interface ISecurityDBContext
  {
    ITenantDBSet<User> Users { get; }

    ITenantDBSet<UserCredential> UserCredentials { get; }
  }
}
