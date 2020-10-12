using System;
using System.Threading;
using System.Threading.Tasks;
using BizOS.Accounts.Request;
using Infrastructure.Extentions;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security.Application.Command;
using Security.Domain.Entities;

namespace BizOS.Accounts.Controllers
{
  [Route("api/v{version:apiVersion}/[controller]")]
  [ApiController]
  [ApiVersion("1.0")]
  [Authorize(JwtBearerDefaults.AuthenticationScheme)]
  public class UserController : ControllerBase
  {
    private readonly IMediator mediator;

    public UserController(IMediator mediator)
    {
      this.mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("{tenantId}/authenticate")]
    public async Task<IActionResult> Authenticate([FromRoute] Guid tenantId, [FromBody] AuthenticateModel model, CancellationToken cancellationToken)
    {
      var token = await mediator
        .Send(
              new AuthenticateUserCommand(
                tenantId, 
                model.UserName, 
                model.Password), 
        cancellationToken);

      if (token == null)
        return BadRequest(new { message = "Username or password is incorrect" });

      return this.Ok(token);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody] User user, CancellationToken cancellationToken)
    {
      return this.Ok(await mediator.Send(
        new RegisterUserCommand(
          User.GetTenantId(), 
          User.GetAccountId(), 
          user.FirstName,
          user.LastName,
          user.UserName,
          user.Email),
        cancellationToken));
    }


    [HttpPut]
    public async Task<IActionResult> UpdateUserInfo([FromBody] User user, CancellationToken cancellationToken)
    {
      return this.Ok(await mediator.Send(
                       new UpdateUserInfoCommand(
                         User.GetTenantId(),
                         User.GetAccountId(),
                         user.Id,
                         user.UserName,
                         user.FirstName,
                         user.LastName,
                         user.Email),
                       cancellationToken));
    }

    [HttpPut("changepassword")]
    public async Task<IActionResult> UpdateCredentials([FromBody] PasswordChange passwordChange, CancellationToken cancellationToken)
    {
      return this.Ok(await mediator.Send(
                       new PasswordChangeCommand(
                         User.GetTenantId(),
                         User.GetAccountId(),
                         passwordChange.OldPassword,
                         passwordChange.NewPassword,
                         passwordChange.ConfirmPassword),
                       cancellationToken));
    }
  }
}
