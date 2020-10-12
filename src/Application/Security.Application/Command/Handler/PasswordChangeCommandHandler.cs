using System.Threading;
using System.Threading.Tasks;
using Common.Application.Contracts.Commands;
using Security.Application.Contracts.Persistance;

namespace Security.Application.Command.Handler
{
  public class PasswordChangeCommandHandler : AbstractCommandHandler<PasswordChangeCommand, bool>
  {
    private readonly ISecurityDBContext securityDBContext;

    public PasswordChangeCommandHandler(ISecurityDBContext securityDBContext)
    {
      this.securityDBContext = securityDBContext;
    }

    public override async Task<bool> HandleAsync(PasswordChangeCommand request, CancellationToken cancellationToken)
    {
      var userCredential = await securityDBContext.UserCredentials.QueryByIdAsync(request.TenantId, request.AccountId, cancellationToken);
      // return false if user not found
      if (userCredential == null)
      {
        return false;
      }

      if (SecurityHelper.VerifyPasswordHash(request.OldPassword, userCredential.Hash, userCredential.Salt))
      {
        SecurityHelper.CreatePasswordHash(request.NewPassword, out string passwordHash, out string passwordSalt);
        userCredential.Hash = passwordHash;
        userCredential.Salt = passwordSalt;
        await securityDBContext.UserCredentials.UpdateAsync(request.TenantId, request.AccountId, cancellationToken, userCredential);
        return true;
      }
      return false;
    }
  }
}
