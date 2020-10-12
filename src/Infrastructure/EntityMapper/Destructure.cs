using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Infrastructure.EntityMapper
{
  public abstract class Destructure<T>
  {
    public abstract Func<T, List<object>> GetDestructureFunction(ReadOnlyCollection<string> columns);
  }
}