using System.Linq.Expressions;

namespace IQToolkit {
  /// <summary>
  /// Replaces references to one specific instance of an expression node with another node
  /// </summary>
  public class ExpressionReplacer : ExpressionVisitor {
    private readonly Expression searchFor;
    private readonly Expression replaceWith;

    private ExpressionReplacer(Expression searchFor, Expression replaceWith) {
      this.searchFor = searchFor;
      this.replaceWith = replaceWith;
    }

    public static Expression Replace(Expression expression, Expression searchFor, Expression replaceWith) => new ExpressionReplacer(searchFor, replaceWith).Visit(expression);

    public static Expression ReplaceAll(Expression expression, Expression[] searchFor, Expression[] replaceWith) {
      for (int i = 0, n = searchFor.Length; i < n; i++) {
        expression = Replace(expression, searchFor[i], replaceWith[i]);
      }
      return expression;
    }

    public override Expression Visit(Expression exp) {
      if (exp == searchFor) {
        return replaceWith;
      }
      return base.Visit(exp);
    }
  }
}
