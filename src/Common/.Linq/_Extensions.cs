using System.Linq.Expressions;
using System.Reflection;

namespace Common;
public static class LinqExtensions {

  internal static object GetExpressionValue(this Expression expression) {
    if (expression == null)      throw new ArgumentNullException(nameof(expression));
    switch (expression.NodeType) {
      case ExpressionType.Call: return ResolveMethodCall(expression as MethodCallExpression);
      case ExpressionType.Constant: return (expression as ConstantExpression).Value;
      case ExpressionType.Convert:
        if (expression is UnaryExpression convertExpression)
          return convertExpression.Operand.GetExpressionValue();
        var expressionType = expression.GetType();
        throw new
            ArgumentException($"Expected some expression as {nameof(UnaryExpression)} but received {expressionType.FullName}");
      case ExpressionType.MemberAccess:
        if (expression is MemberExpression memberExpr) {
          if (memberExpr.Expression == null) {
            var value = ResolveValue(memberExpr.Member, null);
            return value;
          }
          var obj = memberExpr.Expression.GetExpressionValue();
          return ResolveValue(memberExpr.Member, obj);
        }
        throw new ArgumentException("Invalid expression");
      default:
        throw new ArgumentException("Expected constant expression");
    }
  }

  internal static object ResolveMethodCall(this MethodCallExpression callExpression) {
    var arguments = callExpression.Arguments.Select(GetExpressionValue).ToArray();
    var obj = callExpression.Object != null ? GetExpressionValue(callExpression.Object) : arguments.First();
    return callExpression.Method.Invoke(obj, arguments);
  }

  internal static object ResolveValue(this PropertyInfo property, object obj) => property.GetValue(obj, null);

  internal static object ResolveValue(this FieldInfo field, object obj) => field.GetValue(obj);
  internal static object ResolveValue(this MethodInfo method, object obj) => method.Invoke(obj, null);

  internal static object ResolveValue(this MemberInfo member, object obj) => member switch {
    FieldInfo f => f.ResolveValue(obj),
    PropertyInfo p => p.ResolveValue(obj),
    MethodInfo m => m.ResolveValue(obj),
    _ => throw new ArgumentException("MemberInfo must be of type FieldInfo, PropertyInfo or MethodInfo", nameof(member))
  };

  public static T GetValue<T>(this MemberInfo member, object srcObject) => (T)member.ResolveValue(srcObject);

  public static void SetValue(this MemberInfo member, object destObject, object value) {
    switch (member) {
      case FieldInfo mfi:
        mfi.SetValue(destObject, value);
        break;
      case PropertyInfo mpi:
        mpi.SetValue(destObject, value);
        break;
      case MethodInfo mi:
        mi.Invoke(destObject, [value]);
        break;
      default:
        throw new ArgumentException("MemberInfo must be of type FieldInfo, PropertyInfo or MethodInfo", nameof(member));
    }
  }
  public static void SetValue<T>(this MemberInfo member, object destObject, T value) => member.SetValue(destObject, (object)value);
}
