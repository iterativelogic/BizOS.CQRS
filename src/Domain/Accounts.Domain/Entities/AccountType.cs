using System;
using Common.Domain.Entities;

namespace Accounts.Domain.Entities
{
  public class AccountType : Entity
  {
    public string Name { get; set; }
    
    public Guid? ParentId { get; set; }

    public bool IsSelectable { get; set; }

    public bool IsSystem { get; set; }

  }
}