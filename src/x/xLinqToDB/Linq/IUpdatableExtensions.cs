using System.Linq.Expressions;

namespace LinqToDB.Linq;
public static class IUpdatableExtensions {

  public static IUpdatable<T> SetLambda<T>(this IUpdatable<T> source, LambdaExpression extract, object value) where T : notnull {
    return value switch {
      null => source.Set(extract.AsTypedExpression<T, object>(), value),
      double dbl => source.Set(extract.AsTypedExpression<T, double>(), dbl),
      int i => source.Set(extract.AsTypedExpression<T, int>(), i),
      string s => source.Set(extract.AsTypedExpression<T, string>(), s),
      _ => throw new NotImplementedException(value.GetType().ToString())
    };
  }

}