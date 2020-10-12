
using System;
using System.Collections.Generic;
using Common.Application.Contracts.Commands;
using Common.Domain.Entities;

namespace Common.Application.Queries.GenericTenantQueries
{
  public class GetTenantEntities<T> : TenantQuery<IEnumerable<T>> where T : TenantEntity
  {
    public GetTenantEntities(Guid tenantId) : base(tenantId)
    {
    }

    public string SearchTerm { get; set; }
  }
}
