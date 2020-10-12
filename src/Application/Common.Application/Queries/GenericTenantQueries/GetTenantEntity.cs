using System;
using Common.Application.Contracts.Commands;
using Common.Domain.Entities;

namespace Common.Application.Queries.GenericTenantQueries
{
  public class GetTenantEntity<T> : TenantQuery<T> where T : TenantEntity
  {
    public GetTenantEntity(Guid tenantId, Guid id) : base(tenantId)
    {
      Id = id;
    }

    public Guid Id { get; set; }
  }
}

