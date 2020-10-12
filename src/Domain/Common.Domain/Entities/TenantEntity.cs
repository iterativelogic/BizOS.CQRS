
namespace Common.Domain.Entities
{
  public class TenantEntity : AuditableEntity
  { 
    public long RowVersion { get => LastModifiedDate ?? CreatedDate; set => LastModifiedDate = value; }
  }
}