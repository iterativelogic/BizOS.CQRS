using System;
using Common.Application.Contracts.Commands;

namespace Security.Application.Command
{
  public class PasswordChangeCommand : TenantCommand<bool>
  {
    public PasswordChangeCommand(Guid tenantId, Guid accountId, string oldPassword, string newPassword, string confirmPassword) : base(tenantId, accountId)
    {
      OldPassword = oldPassword;
      NewPassword = newPassword;
      ConfirmPassword = confirmPassword;
    }

    public readonly string OldPassword;

    public readonly string NewPassword;

    public readonly string ConfirmPassword;
  }
}
