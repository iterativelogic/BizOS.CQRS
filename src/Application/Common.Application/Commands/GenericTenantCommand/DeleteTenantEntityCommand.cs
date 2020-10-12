using System;
using Common.Application.Contracts.Commands;
using Common.Domain.Entities;

namespace Common.Application.Commands.GenericTenantCommand
{
  public class DeleteTenantEntityCommand<T> : TenantCommand<Guid> where T : TenantEntity
  {
    public Guid Id { get; }

    public DeleteTenantEntityCommand(Guid tenantId, Guid accountId, Guid id) : base(tenantId, accountId)
    {
      Id = id;
    }
  }
}
