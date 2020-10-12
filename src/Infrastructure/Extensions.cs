using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure
{
  public static class Extensions
  {
    public static PropertyInfo GetPropertyInfo<TSource, TProperty>(this Expression<Func<TSource, TProperty>> propertyLambda)
    {
      PropertyInfo propInfo = null;
      if (propertyLambda.Body is MemberExpression memberExpression)
      {
        propInfo = GetPropertyInfo(memberExpression);
      }
      else if(propertyLambda.Body is UnaryExpression unaryExpression)
      {
        propInfo = GetPropertyInfo(unaryExpression);
      }

      if (propInfo == null)
      {
        throw new ArgumentException(string.Format(
            "Expression '{0}' refers to a method, not a property.",
            propertyLambda.ToString()));
      }
      return propInfo;
    }

    private static PropertyInfo GetPropertyInfo(UnaryExpression unaryExpression)
    {
      if (unaryExpression.Operand is MemberExpression memberExpression)
      {
        return GetPropertyInfo(memberExpression);
      }
      return null;
    }

    private static PropertyInfo GetPropertyInfo(MemberExpression memberExpression)
    {
        var propInfo = memberExpression.Member as PropertyInfo;
        if (propInfo == null)
          throw new ArgumentException(string.Format(
              "Expression '{0}' refers to a field, not a property.",
              memberExpression.ToString()));

       return propInfo;
    }
  }
}
