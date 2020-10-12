using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Application.Contracts.Commands;
using Security.Application.Contracts.Persistance;
using Security.Domain.Entities;

namespace Security.Application.Command.Handler
{
  public class UpdateUserInfoCommandHandler : AbstractCommandHandler<UpdateUserInfoCommand, User>
  {
    private readonly ISecurityDBContext securityDBContext;

    public UpdateUserInfoCommandHandler(ISecurityDBContext securityDBContext)
    {
      this.securityDBContext = securityDBContext;
    }

    public override async Task<User> HandleAsync(UpdateUserInfoCommand request, CancellationToken cancellationToken)
    {
      var user = new User()
      {
        Id = request.Id,
        Email = request.Email,
        FirstName = request.FirstName,
        LastName = request.LastName,
        UserName = request.UserName,
        LastModifiedBy = request.AccountId,
        LastModifiedDate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
      };

      await securityDBContext.Users.UpdateAsync(request.TenantId, request.AccountId, cancellationToken, user);
      return await securityDBContext.Users.QueryByIdAsync(request.TenantId, request.Id, cancellationToken);
    }
  }
}
