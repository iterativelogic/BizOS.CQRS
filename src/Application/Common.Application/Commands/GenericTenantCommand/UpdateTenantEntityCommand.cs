using System;
using Common.Application.Contracts.Commands;
using Common.Domain.Entities;

namespace Common.Application.Commands.GenericTenantCommand
{
  public class UpdateTenantEntityCommand<T> : TenantCommand<T> where T : TenantEntity
  {
    public Guid Id { get; }

    public T Model { get; }

    public UpdateTenantEntityCommand(Guid tenantId, Guid accountId, Guid id, T model) : base(tenantId, accountId)
    {
      Id = id;
      Model = model;
    }
  }
}
