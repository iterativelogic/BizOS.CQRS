using Common.Application.Contracts;
using Common.Application.Extentions;
using FluentValidation;
using Security.Application.Command;

namespace BizOS.Accounts.Request.Validations
{
  public class AuthenticateModelValidator : AbstractValidator<AuthenticateUserCommand>
  {
    public static ErrorCode PasswordRequeried { get; } = ErrorCode.GetRequiredFieldErrorCode(nameof(AuthenticateUserCommand.Password));
    public static ErrorCode UserNameRequeried { get; } = ErrorCode.GetRequiredFieldErrorCode(nameof(AuthenticateUserCommand.UserName));

    public static ErrorCode PasswordMinimumLength { get; } = ErrorCode.GetMinimumFieldErrorCode(nameof(AuthenticateUserCommand.Password), 8);
    public static ErrorCode UserNameMinimumLength { get; } = ErrorCode.GetMinimumFieldErrorCode(nameof(AuthenticateUserCommand.UserName), 5);


    public AuthenticateModelValidator()
    {
      RuleFor(x => x.UserName).Required().WithError(UserNameRequeried);
      RuleFor(x => x.Password).Required().WithError(PasswordRequeried);
      RuleFor(x => x.Password).MinimumLength(8).WithError(PasswordMinimumLength);
      RuleFor(x => x.UserName).MinimumLength(5).WithError(UserNameMinimumLength);
    }
  }
}
