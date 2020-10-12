using Common.Domain.Entities;

namespace Security.Domain.Entities
{
  public class UserCredential : TenantEntity
  {
    public string Hash { get; set; }

    public string Salt { get; set; }

    public string UserName { get; set; }
  }
}
