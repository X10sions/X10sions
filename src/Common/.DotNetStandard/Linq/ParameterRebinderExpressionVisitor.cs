using System.Linq.Expressions;

namespace Common.Linq;
public class ParameterRebinderExpressionVisitor : ExpressionVisitor {

    readonly ParameterExpression _parameter;

    public ParameterRebinderExpressionVisitor(ParameterExpression parameter) { _parameter = parameter; }

    protected override Expression VisitParameter(ParameterExpression p) => base.VisitParameter(_parameter);

  }