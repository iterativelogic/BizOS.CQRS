using System;
using Common.Application.Contracts.Commands;
using Common.Domain.Entities;

namespace Common.Application.Commands.GenericCommand
{
  public class UpdateEntityCommand<T> : Command<T> where T : Entity
  {
    public Guid Id { get; }

    public T Model { get; }

    public UpdateEntityCommand(Guid accountId, Guid id, T model) : base(accountId)
    {
      Id = id;
      Model = model;
    }
  }
}
