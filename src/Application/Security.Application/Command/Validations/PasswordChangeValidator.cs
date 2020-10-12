using Common.Application.Contracts;
using Common.Application.Extentions;
using FluentValidation;
using Security.Application.Command;

namespace BizOS.Accounts.Request.Validations
{
  public class PasswordChangeValidator : AbstractValidator<PasswordChangeCommand>
  {
    public static ErrorCode NewPasswordRequired { get; } = ErrorCode.GetRequiredFieldErrorCode(nameof(PasswordChangeCommand.NewPassword));
    
    public static ErrorCode NewPasswordMinLenth { get; } = ErrorCode.GetMinimumFieldErrorCode(nameof(PasswordChangeCommand.NewPassword), 8);
    
    public static ErrorCode ConfirmPasswordMustMatchNewPassword { get; } = new ErrorCode("CONFIRM_PASSWORD_MUST_MATCH", "Confirm password and new password must be same.");

    public PasswordChangeValidator()
    {
      RuleFor(x => x.NewPassword)
        .Required()
        .WithError(NewPasswordRequired);

      RuleFor(x => x.NewPassword)
        .MinimumLength(8)
        .WithError(NewPasswordMinLenth);

      RuleFor(x => x.ConfirmPassword)
        .Must((passwordChange, confirmPassword) => passwordChange.NewPassword == confirmPassword)
        .WithError(ConfirmPasswordMustMatchNewPassword);
    }
  }
}
