using System.Linq.Expressions;
using System.Reflection;

namespace LinqToDB.Linq;
public static class IValueInsertableExtensions {

  public static IValueInsertable<T> ValueLambdaByType<T>(this IValueInsertable<T> insertable, LambdaExpression expression, object? value) where T : notnull {
    var fieldType = expression.Body.Type;

    if (fieldType.IsNullable()) {
      insertable = fieldType == typeof(bool?) ? insertable.Value(expression.AsTypedExpression<T, bool?>(), (bool?)value)
                 : fieldType == typeof(double?) ? insertable.Value(expression.AsTypedExpression<T, double?>(), (double?)value)
                 : fieldType.IsEnum ? insertable.Value(expression.AsTypedExpression<T, object>(), value is string ? Enum.Parse(fieldType, value as string): Enum.ToObject(fieldType, value))
                 : fieldType == typeof(int?) ? insertable.Value(expression.AsTypedExpression<T, int?>(), (int?)value)
                 : fieldType == typeof(long?) ? insertable.Value(expression.AsTypedExpression<T, long?>(), (long?)value)
                 : fieldType == typeof(short?) ? insertable.Value(expression.AsTypedExpression<T, short?>(), (short?)value)
                 : fieldType == typeof(string) ? insertable.Value(expression.AsTypedExpression<T, string?>(), (string?)value)
                 : throw new NotImplementedException(fieldType.ToString());
    } else {
      insertable = fieldType == typeof(bool) ? insertable.Value(expression.AsTypedExpression<T, bool>(), (bool)value)
                 : fieldType == typeof(double) ? insertable.Value(expression.AsTypedExpression<T, double>(), (double)value)
                 : fieldType.IsEnum ? insertable.Value(expression.AsTypedExpression<T, int>(), (int)value)
                 : fieldType == typeof(int) ? insertable.Value(expression.AsTypedExpression<T, int>(), (int)value)
                 : fieldType == typeof(long) ? insertable.Value(expression.AsTypedExpression<T, long>(), (long)value)
                 : fieldType == typeof(short) ? insertable.Value(expression.AsTypedExpression<T, short>(), (short)value)
                 : fieldType == typeof(string) ? insertable.Value(expression.AsTypedExpression<T, string>(), (string)value)
                 : throw new NotImplementedException(fieldType.ToString());
    }
    return insertable;
  }

  public static IValueInsertable<T> ValueLambda<T,TV>(this IValueInsertable<T> source, LambdaExpression le, TV value, IQueryable<T> query) where T : notnull {
    //var converted  = Expression.Lambda<Func<T, int?>>(Expression.Convert(le.Body,  le.Body.Type), le.Parameters);

    //var valueType = le.Body.Type;
    //var body = le.Body;
    //var body = Expression.Convert(le.Body, valueType);

    //Expression<Func<T, TV>> expr = Expression.Lambda<Func<T, TV>>(le.Body, le.Parameters);
    //var methodInfo = MethodHelper.GetMethodInfo(LinqExtensions.Value, source, expr, value);
    //var call = Expression.Call(null,methodInfo, query.Expression, Expression.Quote(le), Expression.Constant(value, le.Body.Type));

    //var q = query.Provider.CreateQuery<T>(call);
    //return new ValueInsertable<T>(q);

    source = source.Value(Expression.Lambda<Func<T, TV>>(le.Body, le.Parameters), value);
    //source = source.Value(le.AsTypedExpression<T,TV>(value), value);
    return source;
  }
}