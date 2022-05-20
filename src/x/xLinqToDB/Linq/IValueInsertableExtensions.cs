using System.Linq.Expressions;

namespace LinqToDB.Linq;
public static class IValueInsertableExtensions {

  public static IValueInsertable<T> ValueLambda<T>(this IValueInsertable<T> source, LambdaExpression field, object value) where T : notnull {
    return value switch {
      null=> source.Value(field.AsTypedExpression<T, object>(), value),
      double dbl => source.Value(field.AsTypedExpression<T, double>(), dbl),
      int i => source.Value(field.AsTypedExpression<T, int>(), i),
      string s => source.Value(field.AsTypedExpression<T, string>(), s),
      _ => throw new NotImplementedException(value.GetType().ToString())
    };
  }

}