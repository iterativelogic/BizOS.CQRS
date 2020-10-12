using System;

namespace Common.Application.Contracts.Commands
{
  public class TenantQuery<TResponse> : CommonQuery<TResponse>
  {
    public Guid TenantId { get; }

    public TenantQuery(Guid tenantId)
    {
      TenantId = tenantId;
    }
  }
}
