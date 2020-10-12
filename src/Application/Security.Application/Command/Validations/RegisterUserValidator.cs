using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Application.Contracts;
using Common.Application.Extentions;
using FluentValidation;
using Security.Application.Contracts.Persistance;
using Security.Domain.Entities;

namespace Security.Application.Command.Validations
{
  public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
  {
    private readonly ErrorCode uniqueUserName = new ErrorCode("USER_NAME_EXISTS", "User name already in use.");
    private readonly ISecurityDBContext securityDBContext;

    public RegisterUserValidator(ISecurityDBContext securityDBContext)
    {
      this.securityDBContext = securityDBContext;
      //Other user name validations like length pattern etc.
      RuleFor(x => x.UserName).MustAsync((request, userName, cancellationToken) => BeUnique(request.TenantId, userName, cancellationToken)).WithError(uniqueUserName);
    }

    private async Task<bool> BeUnique(Guid tenantId, string userName, CancellationToken cancellationToken)
    {
      var query = securityDBContext.Users.Query;
      query.Where(securityDBContext.Users.GetColumnName(nameof(User.UserName)), userName);
      var user = await securityDBContext.Users.QueryFirstOrDefaultAsync(tenantId, query, cancellationToken);
      return user == null;
    }
  }
}
