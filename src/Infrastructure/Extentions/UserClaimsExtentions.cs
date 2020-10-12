using System;
using System.Security.Claims;

namespace Infrastructure.Extentions
{
  public static class UserClaimsExtentions
  {
    public static Guid GetTenantId(this ClaimsPrincipal userClaim)
    {
      return new Guid(userClaim.FindFirst("tenantId").Value);
    }

    public static Guid GetAccountId(this ClaimsPrincipal userClaim)
    {
      return new Guid(userClaim.FindFirst("accountId").Value);
    }
  }
}
