namespace System.Linq.Expressions;

public class ReplaceExpressionVisitor : ExpressionVisitor {
  public ReplaceExpressionVisitor(Expression source, Expression target) {
    this.source = source;
    this.target = target;
  }

  private readonly Expression source;
  private readonly Expression target;

  public override Expression Visit(Expression node) => node == source ? target : base.Visit(node);
}