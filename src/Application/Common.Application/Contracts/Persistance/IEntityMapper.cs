using System;
using System.Collections.Generic;
using SqlKata;

namespace Common.Application.Contracts.Persistance
{
  public interface IEntityMapper<T>
  {
    public Query Query { get; }

    public Type[] ChildTypes { get; }

    public Func<object[], T> GetMapper();
    
    public List<string> IgnoredColumns { get; }

    public List<string> SplitOn { get; }

    public string GetColumnName(string propName);
  }
}
