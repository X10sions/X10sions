using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Data.Assocations;
public static class Extensions {
  public static PropertyInfo GetPropertyInfo(this MemberInfo memberInfo) => memberInfo.ReflectedType.GetProperty(memberInfo.Name);

  //static bool IsIEnumerable(this Expression expression) => typeof(IEnumerable).IsAssignableFrom((expression as LambdaExpression)?.Body.Type);
  public static bool IsIEnumerable(this LambdaExpression expression) => typeof(IEnumerable).IsAssignableFrom(expression.Body.Type);
}
