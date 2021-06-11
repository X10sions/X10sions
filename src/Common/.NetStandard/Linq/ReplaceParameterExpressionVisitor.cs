using System.Linq.Expressions;

namespace Common.Linq {
  public class ReplaceParameterExpressionVisitor : ExpressionVisitor {

    public ReplaceParameterExpressionVisitor(ParameterExpression from, ParameterExpression to) {
      this.from = from;
      this.to = to;
    }
    readonly ParameterExpression from;
    readonly ParameterExpression to;

    protected override Expression VisitParameter(ParameterExpression node) => node == from ? to : base.VisitParameter(node);

  }
}
