namespace System.Linq.Expressions;

public static class MemberExpressionExtensions {

  public static bool DoMemberTranslate(this MemberExpression memberExpression, string memberName, params Type[] types)
    => memberExpression.Expression != null
    && memberName == memberExpression.Member.Name
    && types.Contains(memberExpression.Expression.Type);

}