namespace Common.Linq {

  public class xCriterion {
    public xCriterion(xParameter Parameter, xComparator Comparator, double Value) {
      this.Parameter = Parameter;
      this.Comparator = Comparator;
      this.Value = Value;
    }

    public readonly xParameter Parameter;
    public readonly xComparator Comparator;
    public readonly double Value;

    //public Expression<Func<xField, bool>> CreateExpression(IEnumerable<xCriterion> Criteria) {
    //  var FullExpression = ExpressionExtensions.True<xField>();

    //  foreach (var criterion in Criteria) {
    //    var Value = criterion.Value;

    //    Dictionary<xComparator, Expression<Func<xField, bool>>> TotalCostExpressions = new Dictionary<xComparator, Expression<Func<xField, bool>>>()        {
    //      { xComparator.LessThan, Field => Field.TotalCost < Value            },
    //      { xComparator.GreaterThan, Field => Field.TotalCost > Value            },
    //      { xComparator.Equals,  Field => Field.TotalCost == Value            }
    //    };

    //    Dictionary<xComparator, Expression<Func<xField, bool>>> RequiredExpressions = new Dictionary<xComparator, Expression<Func<xField, bool>>>()        {
    //      { xComparator.LessThan, Field => Field.Required < Value            },
    //      { xComparator.GreaterThan, Field => Field.Required > Value            },
    //      { xComparator.Equals, Field => Field.Required == Value            }
    //    };

    //    Dictionary<xParameter, IDictionary<xComparator, Expression<Func<xField, bool>>>> Expressions = new Dictionary<xParameter, IDictionary<xComparator, Expression<Func<xField, bool>>>>() {
    //      { xParameter.TotalCost, TotalCostExpressions },
    //      { xParameter.Required, RequiredExpressions }
    //    };


    //    var Expression = Expressions[criterion.Parameter](criterion.Comparator);
    //    FullExpression = Expression.And(Expression);
    //  }

    //  return FullExpression;
    //}

  }
}
