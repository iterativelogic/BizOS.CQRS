using System;

namespace Common.Domain.Entities
{
  public class Entity
  {
    public Guid Id { get; set; }

    public bool Deleted { get; set; }
    
    public dynamic Descriptions { get; set; }

  }
}