using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Application.Contracts.Commands;
using Security.Application.Contracts.Persistance;
using Security.Domain.Entities;

namespace Security.Application.Command.Handler
{
  public class RegisterUserCommandHandler : AbstractCommandHandler<RegisterUserCommand, User>
  {
    private readonly ISecurityDBContext securityDBContext;

    public RegisterUserCommandHandler(ISecurityDBContext securityDBContext)
    {
      this.securityDBContext = securityDBContext;
    }

    public override async Task<User> HandleAsync(RegisterUserCommand request, CancellationToken cancellationToken)
    {
      var createdUser = await RegisterUserAsync(request, cancellationToken);
      await UpdateDefaultCredentialsAsync(request, createdUser, cancellationToken);
      return createdUser;
    }

    private async Task UpdateDefaultCredentialsAsync(RegisterUserCommand request, User createdUser, CancellationToken cancellationToken)
    {
      var userCredential = await securityDBContext.UserCredentials.QueryByIdAsync(request.TenantId, createdUser.Id, cancellationToken);

      SecurityHelper.CreatePasswordHash($"{request.UserName}@321", out string passwordHash, out string passwordSalt);
      userCredential.Hash = passwordHash;
      userCredential.Salt = passwordSalt;

      await securityDBContext.UserCredentials.UpdateAsync(
        request.TenantId,
        request.AccountId,
        cancellationToken,
        userCredential);
    }

    private async Task<User> RegisterUserAsync(RegisterUserCommand request, CancellationToken cancellationToken)
    {
      var user = new User
      {
        Id = Guid.NewGuid(),
        Deleted = false,
        Email = request.Email,
        FirstName = request.FirstName,
        LastName = request.LastName,
        UserName = request.UserName
      };

      await securityDBContext.Users.InsertAsync(request.TenantId, request.AccountId, cancellationToken, user);

      return await securityDBContext.Users.QueryByIdAsync(request.TenantId, user.Id, cancellationToken);
    }
  }
}
