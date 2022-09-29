namespace System.Linq.Expressions;

public static class MemberExpressionExtensions {

  public static bool DoMemberTranslate(this MemberExpression memberExpression, string memberName, params Type[] types)
    => memberExpression.Expression != null
    && memberName == memberExpression.Member.Name
    && types.Contains(memberExpression.Expression.Type);

  public static object GetValue(this MemberExpression exp) =>
    // expression is ConstantExpression or FieldExpression
    exp.Expression switch {
      ConstantExpression => ((ConstantExpression)exp.Expression).Value.GetType().GetField(exp.Member.Name).GetValue(((ConstantExpression)exp.Expression).Value),
      MemberExpression => ((MemberExpression)exp.Expression).GetValue(),
      _ => throw new NotImplementedException()
    };

}