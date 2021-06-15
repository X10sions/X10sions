using System.Linq.Expressions;

namespace DynamicQueryNet.Utility {
  public class StringCompare : ICompare {
    private static int _equalNumber = 0;
    private const bool TrueValue = true;

    public Expression Equal<T>(CompareInput input) {
      var compare = Expression.Call(typeof(string), nameof(string.Compare), null, input.PropertyExpr, input.Value);
      return Expression.Equal(compare, Expression.Constant(_equalNumber));
    }

    public Expression NotEqual<T>(CompareInput input) {
      var compare = Expression.Call(typeof(string), nameof(string.Compare), null, input.PropertyExpr, input.Value);
      return Expression.NotEqual(compare, Expression.Constant(_equalNumber));
    }

    public Expression GreaterThan<T>(CompareInput input) {
      var compare = Expression.Call(typeof(string), nameof(string.Compare), null, input.PropertyExpr, input.Value);
      return Expression.GreaterThan(compare, Expression.Constant(_equalNumber));
    }

    public Expression GreaterThanOrEqual<T>(CompareInput input) {
      var compare = Expression.Call(typeof(string), nameof(string.Compare), null, input.PropertyExpr, input.Value);
      return Expression.GreaterThanOrEqual(compare, Expression.Constant(_equalNumber));
    }

    public Expression LessThan<T>(CompareInput input) {
      var compare = Expression.Call(typeof(string), nameof(string.Compare), null, input.PropertyExpr, input.Value);
      return Expression.LessThan(compare, Expression.Constant(_equalNumber));
    }

    public Expression LessThanOrEqual<T>(CompareInput input) {
      var compare = Expression.Call(typeof(string), nameof(string.Compare), null, input.PropertyExpr, input.Value);
      return Expression.LessThanOrEqual(compare, Expression.Constant(_equalNumber));
    }

    public Expression Contains<T>(CompareInput input) {
      var method = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });
      var contains__1 = Expression.Call(input.PropertyExpr, method, input.Value);
      return Expression.Equal(contains__1, Expression.Constant(TrueValue));
    }

  }
}
