using System;
using Common.Application.Contracts.Commands;
using Common.Domain.Entities;

namespace Common.Application.Commands.GenericCommand
{
  public class DeleteEntityCommand<T> : Command<Guid> where T : Entity
  {
    public Guid Id { get; }

    public DeleteEntityCommand(Guid accountId, Guid id) : base(accountId)
    {
      Id = id;
    }
  }
}
