using System;
using Common.Application.Contracts.Commands;
using MediatR;
using Security.Domain.Entities;

namespace Security.Application.Command
{
  public class RegisterUserCommand : TenantCommand<User>
  {
    public RegisterUserCommand(Guid tenantId, Guid accountId, string firstName, string lastName, string userName,
      string email) : base(tenantId, accountId)
    {
      FirstName = firstName;
      LastName = lastName;
      UserName = userName;
      Email = email;
    }

    public readonly string FirstName;

    public readonly string LastName;

    public readonly string UserName;

    public readonly string Email;
  }
}
