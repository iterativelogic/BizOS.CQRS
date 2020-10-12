using System;
using Common.Application.Contracts.Commands;
using Common.Domain.Entities;

namespace Common.Application.Commands.GenericCommand
{
  public class CreateEntityCommand<T> : Command<T> where T : Entity
  {
    public T Model { get; }

    public CreateEntityCommand(Guid accountId, T model) : base(accountId)
    {
      Model = model;
    }
  }
}
