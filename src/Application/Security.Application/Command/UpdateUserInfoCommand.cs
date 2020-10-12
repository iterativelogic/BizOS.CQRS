using System;
using Common.Application.Contracts.Commands;
using MediatR;
using Security.Domain.Entities;

namespace Security.Application.Command
{
  public class UpdateUserInfoCommand : TenantCommand<User>
  {
    public UpdateUserInfoCommand(
      Guid tenantId, 
      Guid accountId, 
      Guid id, 
      string userName,
      string firstName,
      string lastName,
      string email) : base(tenantId, accountId)
    {
      Id = id;
      UserName = userName;
      FirstName = firstName;
      LastName = lastName;
      Email = email;
    }

    public Guid Id { get; }
    public string UserName { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
  }
}
