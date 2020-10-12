using System;
using System.Threading;
using System.Threading.Tasks;
using Security.Application.Contracts;
using Security.Application.Contracts.Persistance;
using Security.Domain.Entities;

namespace Security.Application.Command.Handler
{
  public class UserVerifyService : IUserVerifyService
  {
    private readonly ISecurityDBContext securityDBContext;

    public UserVerifyService(ISecurityDBContext securityDBContext)
    {
      this.securityDBContext = securityDBContext;
    }

    public async Task<bool> VerifyUserAsync(Guid tenantId, string userName, string password, CancellationToken cancellationToken)
    {
      var userCredential = await GetUserCredentialByUserName(tenantId, userName, cancellationToken);
      // return false if user not found
      if (userCredential == null)
      {
        return false;
      }

      return SecurityHelper.VerifyPasswordHash(password, userCredential.Hash, userCredential.Salt);
    }

    private async Task<UserCredential> GetUserCredentialByUserName(Guid tenantId, string userName, CancellationToken cancellationToken)
    {
      var userCredentialQuery = securityDBContext.UserCredentials.Query;
      userCredentialQuery.Where(securityDBContext.UserCredentials.GetColumnName(nameof(UserCredential.UserName)), userName);

      var userCredential = await securityDBContext.UserCredentials.QueryFirstOrDefaultAsync(tenantId, userCredentialQuery, cancellationToken);
      return userCredential;
    }
  }
}
