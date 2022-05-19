namespace System.Linq.Expressions;

public class ReplaceExpressionVisitor : ExpressionVisitor {
  //public ReplaceExpressionVisitor() { }
  public ReplaceExpressionVisitor(Expression source, Expression target) {
    this.source = source;
    this.target = target;
  }

  private readonly Expression source;
  private readonly Expression target;

  public override Expression Visit(Expression node) => node == source ? target : base.Visit(node);
}

[Obsolete("Try Remove")]
public class ReplaceParameterExpressionVisitor : ExpressionVisitor {
  public ReplaceParameterExpressionVisitor(ParameterExpression left, ParameterExpression right) {
    Expressions[right] = left;
    codeSource = CodeSource.PredicateExtensions;
  }

  public ReplaceParameterExpressionVisitor(Dictionary<Expression, Expression> substitues) {
    Expressions = substitues;
    codeSource = CodeSource.PredicateExtensions;
  }
  public ReplaceParameterExpressionVisitor(Dictionary<ParameterExpression, ParameterExpression> substitues) {
    parameterExpressions = substitues;
    codeSource = CodeSource.UniversalPredicateBuilder;
  }

  CodeSource codeSource;
  private readonly Dictionary<ParameterExpression, ParameterExpression> parameterExpressions = new Dictionary<ParameterExpression, ParameterExpression>();
  public Dictionary<Expression, Expression> Expressions { get; set; } = new Dictionary<Expression, Expression>();

  enum CodeSource { PredicateExtensions, UniversalPredicateBuilder }

  protected override Expression VisitParameter(ParameterExpression node) => codeSource switch {
    CodeSource.PredicateExtensions => VisitParameter_PE(node),
    CodeSource.UniversalPredicateBuilder => VisitParameter_UPB(node),
    _ => throw new NotImplementedException()
  };

  Expression VisitParameter_PE(ParameterExpression node) => Expressions.TryGetValue(node, out Expression newValue) ? newValue : node;

  Expression VisitParameter_UPB(ParameterExpression node) {
    if (parameterExpressions.TryGetValue(node, out ParameterExpression replacement)) {
      node = replacement;
    }
    return base.VisitParameter(node);
  }

}