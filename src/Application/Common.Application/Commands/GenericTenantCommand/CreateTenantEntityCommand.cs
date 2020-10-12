using System;
using Common.Application.Contracts.Commands;
using Common.Domain.Entities;

namespace Common.Application.Commands.GenericTenantCommand
{
  public class CreateTenantEntityCommand<T> : TenantCommand<T> where T : TenantEntity
  {

    public CreateTenantEntityCommand(Guid tenantId, Guid accountId, T model) : base(tenantId, accountId)
    {
      Model = model;
    }

    public T Model { get; }
  }
}
