using System;
using Common.Domain.Entities;

namespace Accounts.Domain.Entities
{
 
  public class LedgerGroup : TenantEntity
  {
    public string Name { get; set; }

    public Guid AccountTypeId { get; set; }

    public Guid ParentId { get; set; }
  }  
}