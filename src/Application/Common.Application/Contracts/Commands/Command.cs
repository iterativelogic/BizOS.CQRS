using System;

namespace Common.Application.Contracts.Commands
{

  public abstract class Command<TModel> : AbstractCommand<TModel>
  {

    public readonly Guid AccountId;

    protected Command(Guid accountId)
    {
      AccountId = accountId;
    }
  }
}