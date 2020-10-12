using System;

namespace Common.Domain.Entities
{
  public class AuditableEntity : Entity
  {
    public long? LastModifiedDate { get; set; }

    public long CreatedDate { get; set; }

    public Guid? LastModifiedBy { get; set; }

    public Guid CreatedBy { get; set; }
  }
}