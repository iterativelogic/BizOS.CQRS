
using System.Collections.Generic;
using Common.Application.Contracts.Commands;
using Common.Domain.Entities;

namespace Common.Application.Queries.GenericQueries
{
  public class GetEntities<T> : CommonQuery<IEnumerable<T>> where T : Entity
  {
    public string SearchTerm { get; set; }
  }
}
