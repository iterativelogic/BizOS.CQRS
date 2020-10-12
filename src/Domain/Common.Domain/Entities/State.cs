using System;

namespace Common.Domain.Entities
{
  public class State : Entity
  {
    public string Name { get; set; }
    public Guid CountryId { get; set; }
  }
}