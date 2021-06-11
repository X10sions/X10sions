using System.Collections.Generic;
using System.Linq.Expressions;

namespace Common.Linq {
  public class SubstituteParameterExpressionVisitor : ExpressionVisitor {
    // 2013-10-31: https://github.com/EdCharbeneau/PredicateExtensions/blob/master/PredicateExtensions/PredicateExtensions.cs
    // https://www.nuget.org/packages/PredicateExtensions/

    public Dictionary<Expression, Expression> Substitues = new Dictionary<Expression, Expression>();

    protected override Expression VisitParameter(ParameterExpression node) => Substitues.TryGetValue(node, out var newValue) ? newValue : node;

  }
}