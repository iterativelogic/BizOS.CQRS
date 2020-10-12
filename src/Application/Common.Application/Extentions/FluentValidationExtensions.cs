using Common.Application.Contracts;
using FluentValidation;

namespace Common.Application.Extentions
{
  public static class FluentValidationExtensions
  {
    public static IRuleBuilderOptions<T, TElement> WithError<T, TElement>(this IRuleBuilderOptions<T, TElement> ruleBuilder, ErrorCode errorCode)
    {
      return ruleBuilder.WithErrorCode(errorCode.Code).WithMessage(errorCode.Description);
    }

    public static IRuleBuilderOptions<T, TElement> Required<T, TElement>(this IRuleBuilderInitial<T, TElement> ruleBuilder)
    {
      return ruleBuilder.NotNull().NotEmpty();
    }
  }
}
