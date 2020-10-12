using System;

namespace Common.Domain.Entities
{
  public class City : Entity
  {
    public string Name { get; set; }

    public Guid StateId { get; set; }

    public Guid CountryId { get; set; }

  }
}