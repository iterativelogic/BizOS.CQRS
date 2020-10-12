using System;
using Common.Application.Contracts.Commands;

namespace Security.Application.Command
{
  public class AuthenticateUserCommand : TenantQuery<string>
  {
    public AuthenticateUserCommand(Guid tenantId, string userName, string password): base(tenantId)
    { 
      UserName = userName;
      Password = password;
    }

    public readonly string UserName;
    public readonly string Password;
  }
}
