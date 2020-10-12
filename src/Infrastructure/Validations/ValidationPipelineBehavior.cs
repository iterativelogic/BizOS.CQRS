using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Application.Contracts;
using FluentValidation;
using MediatR;

namespace Infrastructure.Validations
{
  public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
  {
    private readonly IEnumerable<IValidator<TRequest>> validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
      this.validators = validators;
    }
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
      var awaitableValidations = validators.Select(validator => validator.ValidateAsync(request));

      var validationResults = await Task.WhenAll(awaitableValidations);

      if(validationResults.Any(validationResult => !validationResult.IsValid))
      {
        List<ErrorCode> errorCodes = validationResults
          .Where(validationResult => !validationResult.IsValid)
          .SelectMany(validationResult => validationResult.Errors)
          .Select(error => new ErrorCode(error.ErrorCode, error.ErrorMessage))
          .ToList();

        throw new CommandValidationException(errorCodes);
      }
      else
      {
        return await next();
      }
    }
  }
}
