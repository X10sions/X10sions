using System.Linq.Expressions;

namespace Common.Linq {
  public class ReplaceExpressionVisitor : ExpressionVisitor {

    public ReplaceExpressionVisitor(Expression source, Expression target) {
      Source = source;
      Target = target;
    }

    readonly Expression Source;
    readonly Expression Target;

    public override Expression Visit(Expression node) => node == Source ? Target : base.Visit(node);

  }
}
