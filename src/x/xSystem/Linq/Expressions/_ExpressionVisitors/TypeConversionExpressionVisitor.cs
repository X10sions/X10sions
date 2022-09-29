namespace System.Linq.Expressions;

public class TypeConversionExpressionVisitor : ExpressionVisitor {
  public TypeConversionExpressionVisitor(Dictionary<Expression, Expression> parameterMap) {
    this.parameterMap = parameterMap;
  }

  readonly Dictionary<Expression, Expression> parameterMap;

  protected override Expression VisitParameter(ParameterExpression node) {
    // re-map the parameter
    if (!parameterMap.TryGetValue(node, out var found)) found = base.VisitParameter(node);
    return found;
  }

  protected override Expression VisitMember(MemberExpression node) {
    // re-perform any member-binding
    var expr = Visit(node.Expression);
    if (expr.Type != node.Type) {
      var newMember = expr.Type.GetMember(node.Member.Name).Single();
      return Expression.MakeMemberAccess(expr, newMember);
    }
    return base.VisitMember(node);
  }

}