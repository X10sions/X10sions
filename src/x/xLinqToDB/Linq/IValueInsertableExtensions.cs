using System.Linq.Expressions;
using System.Reflection;

namespace LinqToDB.Linq;
public static class IValueInsertableExtensions {

  public static IValueInsertable<T> ValueLambdaByType<T>(this IValueInsertable<T> insertable, LambdaExpression expression, object value) where T : notnull {
    var fieldType = expression.Body.Type;
    if (fieldType.IsNullable()) {
      insertable = fieldType == typeof(bool?) ? insertable.Value(expression.AsTypedExpression<T, bool?>(), (bool?)value)
                 : fieldType == typeof(double?) ? insertable.Value(expression.AsTypedExpression<T, double?>(), (double?)value)
                 : fieldType.IsEnum ? insertable.Value(expression.AsTypedExpression<T, int?>(), (int?)value)
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

  //public static IValueInsertable<T> ValueExpression<T, TV>(this IValueInsertable<T> source, Expression field, TV value) where T : notnull {
  //  source = source.Value(Expression.Lambda<Func<T, TV>>(field), value);
  //  return source;
  //}

  internal class xValueInsertable<T> : IValueInsertable<T> {
    public xValueInsertable(IQueryable<T> query) {
      Query = query;
    }
    public IQueryable<T> Query;
    public override string ToString() => Query.ToString()!;
  }

  public static IValueInsertable<T> ValueLambda<T, TV>(this IValueInsertable<T> source, LambdaExpression le, TV value) where T : notnull {
    //var query = ((ValueInsertable<T>)source).Query;
    //IQueryable<T> query = ((LinqToDB.LinqExtensions.ValueInsertable<T>)source).Query;
    IQueryable<T> query = (IQueryable<T>)source;

    var resultBody = Expression.Convert(le.Body, value.GetType());
    var lambda = Expression.Lambda<Func<T, TV>>(resultBody, le.Parameters);

    var q = query.Provider.CreateQuery<T>(
      Expression.Call(null,
        MethodHelper.GetMethodInfo(LinqExtensions.Value, source, lambda, value),
        query.Expression, Expression.Quote(le), Expression.Constant(value, typeof(TV))));



    Assembly businessAssembly = typeof(LinqToDB.Data.DataConnection).Assembly;
    Type t = businessAssembly.GetType("LinqToDB.LinqExtensions.ValueInsertable");

    BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
    object instantiatedType = Activator.CreateInstance(t, flags, null, query, null);
    //object instantiatedType = Activator.CreateInstance(t, true)
    //var myClass = Activator.CreateInstance(typeof(MyClass), true);//say nonpublic
    return (IValueInsertable<T>)instantiatedType;


    //return new LinqToDB.LinqExtensions.ValueInsertable<T>(q);


    //source = source.Value(field.AsTypedExpression<T,TV>(value), value);

    //var resultBody = Expression.Convert(le.Body, value.GetType());
    //source = source.Value(Expression.Lambda<Func<T, TV>>(resultBody, le.Parameters), value);
    //return source;
  }

  //public static IValueInsertable<T> ValueExpressionNullable<T, TV>(this IValueInsertable<T> source, Expression field, TV? value) where T : notnull {
  //  source = source.Value(Expression.Lambda<Func<T, TV>>(field), value);
  //  return source;
  //}

  //public static IValueInsertable<T> ValueLambdaNullable<T, TV>(this IValueInsertable<T> source, LambdaExpression le, TV? value) where T : notnull {
  //  //source = source.Value(field.AsTypedExpressionNullable<T,TV>(value), value);
  //  var resultBody = Expression.Convert(le.Body, value.GetType());
  //  source = source.Value(Expression.Lambda<Func<T, TV>>(resultBody, le.Parameters), value);
  //  return source;
  //}

}