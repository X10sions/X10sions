using DynamicQueryNet.Constants;
using System;
using System.Linq.Expressions;

namespace DynamicQueryNet.Utility {
  public class NormalCompare : ICompare {

    public Expression Equal<T>(CompareInput input) => Expression.Equal(input.PropertyExpr, Expression.Convert(input.Value, input.PropertyExpr.Type));
    public Expression NotEqual<T>(CompareInput input) => Expression.NotEqual(input.PropertyExpr, Expression.Convert(input.Value, input.PropertyExpr.Type));
    public Expression GreaterThan<T>(CompareInput input) => Expression.GreaterThan(input.PropertyExpr, Expression.Convert(input.Value, input.PropertyExpr.Type));
    public Expression GreaterThanOrEqual<T>(CompareInput input) => Expression.GreaterThanOrEqual(input.PropertyExpr, Expression.Convert(input.Value, input.PropertyExpr.Type));
    public Expression LessThan<T>(CompareInput input) => Expression.LessThan(input.PropertyExpr, Expression.Convert(input.Value, input.PropertyExpr.Type));
    public Expression LessThanOrEqual<T>(CompareInput input) => Expression.LessThanOrEqual(input.PropertyExpr, Expression.Convert(input.Value, input.PropertyExpr.Type));
    public Expression Contains<T>(CompareInput input) => throw new Exception(ErrorConstants.OPERATION_NOT_ALLOWED);

  }
}
