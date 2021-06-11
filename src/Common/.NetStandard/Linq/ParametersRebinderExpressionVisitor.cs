using System.Collections.Generic;
using System.Linq.Expressions;

namespace Common.Linq {
  public class ParametersRebinderExpressionVisitor : ExpressionVisitor {
    // 2011-02-10: UniversalPredicateBuilder
    // https://petemontgomery.wordpress.com/2011/02/10/a-universal-predicatebuilder/

    ParametersRebinderExpressionVisitor(Dictionary<ParameterExpression, ParameterExpression> map) {
      this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
    }

    readonly Dictionary<ParameterExpression, ParameterExpression> map;

    public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp) => new ParametersRebinderExpressionVisitor(map).Visit(exp);

    protected override Expression VisitParameter(ParameterExpression p) {
      ParameterExpression replacement;
      if (map.TryGetValue(p, out replacement)) {
        p = replacement;
      }
      return base.VisitParameter(p);
    }

  } 
}
