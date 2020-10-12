using System;
using System.Linq.Expressions;

namespace Common.Application.Contracts.Persistance
{
  public interface IEntitySchema<T>
  {
    IPropertySchema Property(Expression<Func<T, object>> keyExpression);

    IEntitySchema<T> IsTenantProperty();

    IEntitySchema<T> HasTableName(string tableName);

  }
}