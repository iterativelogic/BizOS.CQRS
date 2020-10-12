using System;
using Common.Application.Contracts.Commands;
using Common.Domain.Entities;

namespace Common.Application.Queries.GenericQueries
{
  public class GetEntity<T> : CommonQuery<T> where T : Entity
  {
    public GetEntity(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; set; }
  }
}

