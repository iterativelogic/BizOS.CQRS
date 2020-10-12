using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Application.Contracts.Commands;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Security.Application.Contracts;
using Security.Application.Contracts.Persistance;
using Security.Domain.Entities;

namespace Security.Application.Command.Handler
{
  public class AuthenticateUserCommandHandler : AbstractCommandHandler<AuthenticateUserCommand, string>
  {
    private readonly ISecurityDBContext securityDBContext;
    private readonly IUserVerifyService userVerifyService;
    private readonly IOptions<SecurityOptions> securityOptions;

    public AuthenticateUserCommandHandler(ISecurityDBContext securityDBContext,
      IUserVerifyService userVerifyService,
      IOptions<SecurityOptions> securityOptions)
    {
      this.securityDBContext = securityDBContext;
      this.userVerifyService = userVerifyService;
      this.securityOptions = securityOptions;
    }

    public override async Task<string> HandleAsync(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
      if (await userVerifyService.VerifyUserAsync(request.TenantId, request.UserName, request.Password, cancellationToken))
      {
        User user = await GetUserByUserName(request, cancellationToken);
        return GenerateJwtToken(request.TenantId, user);
      }

      return null;
    }

    private async Task<User> GetUserByUserName(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
      var userQuery = securityDBContext.Users.Query
                              .Where(securityDBContext.Users.GetColumnName(nameof(User.UserName)), request.UserName);

      var user = await securityDBContext.Users.QueryFirstOrDefaultAsync(request.TenantId, userQuery, cancellationToken);
      return user;
    }

    private string GenerateJwtToken(Guid tenantId, User user)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(securityOptions.Value.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
        {
          new Claim(ClaimTypes.Name, user.UserName),
          new Claim("tenantId", tenantId.ToString()),
          new Claim("accountId", user.Id.ToString())
        }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }
  }
}
