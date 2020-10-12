using System;

namespace Common.Application.Contracts.Commands
{
  public abstract class TenantCommand<TModel> : Command<TModel>
  {
    public Guid TenantId { get; private set; }

    protected TenantCommand(Guid tenantId, Guid accountId) : base(accountId)
    {
      TenantId = tenantId;
    }
  }
}