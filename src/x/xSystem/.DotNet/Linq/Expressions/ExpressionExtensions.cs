using System.Reflection;

namespace System.Linq.Expressions;
public static class ExpressionExtensions {
  public static bool IsPropertyNullable(this Expression expression) => expression.GetMemberInfo().IsPropertyNullable();

}
