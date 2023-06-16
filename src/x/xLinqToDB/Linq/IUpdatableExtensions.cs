using System.Linq.Expressions;

namespace LinqToDB.Linq;
public static class IUpdatableExtensions {

  public static IUpdatable<T> SetLambdaByType<T>(this IUpdatable<T> updatable, LambdaExpression expression, object value) where T : notnull {
    var fieldType = expression.Body.Type;
    if (fieldType.IsNullable()) {
      updatable = fieldType == typeof(bool?) ? updatable.Set(expression.AsTypedExpression<T, bool?>(), (bool?)value)
                : fieldType == typeof(double?) ? updatable.Set(expression.AsTypedExpression<T, double?>(), (double?)value)
                : fieldType.IsEnum ? updatable.Set(expression.AsTypedExpression<T, int?>(), (int?)value)
                : fieldType == typeof(int?) ? updatable.Set(expression.AsTypedExpression<T, int?>(), (int?)value)
                : fieldType == typeof(long?) ? updatable.Set(expression.AsTypedExpression<T, long?>(), (long?)value)
                : fieldType == typeof(short?) ? updatable.Set(expression.AsTypedExpression<T, short?>(), (short?)value)
                : fieldType == typeof(string) ? updatable.Set(expression.AsTypedExpression<T, string?>(), (string?)value)
                : throw new NotImplementedException(fieldType.ToString());
    } else {
      updatable = fieldType == typeof(bool) ? updatable.Set(expression.AsTypedExpression<T, bool>(), (bool)value)
                : fieldType == typeof(double) ? updatable.Set(expression.AsTypedExpression<T, double>(), (double)value)
                : fieldType.IsEnum ? updatable.Set(expression.AsTypedExpression<T, int>(), (int)value)
                : fieldType == typeof(int) ? updatable.Set(expression.AsTypedExpression<T, int>(), (int)value)
                : fieldType == typeof(long) ? updatable.Set(expression.AsTypedExpression<T, long>(), (long)value)
                : fieldType == typeof(short) ? updatable.Set(expression.AsTypedExpression<T, short>(), (short)value)
                : fieldType == typeof(string) ? updatable.Set(expression.AsTypedExpression<T, string>(), (string)value)
                : throw new NotImplementedException(fieldType.ToString());
    }
    return updatable;
  }

  public static IUpdatable<T> SetLambda<T, TV>(this IUpdatable<T> source, LambdaExpression le, TV value) where T : notnull {
    source = source.Set(Expression.Lambda<Func<T, TV>>(le.Body, le.Parameters), value);
    //source = source.Set(le.AsTypedExpressionNullable<T, TV>(value), value);
    return source;
  }

}