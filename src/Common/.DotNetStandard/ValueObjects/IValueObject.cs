using System.Linq.Expressions;

namespace Common.ValueObjects;

public interface IValueObject<out T> {
  T Value { get; }
}

public static class IValueObjectExpressions<T> {

  //public static IQueryable<T> Contains<T, TValue>(this IQueryable<T> qry, params TValue[] values)
  //  where T : IValueObject<T>
  //  => qry.Where(x => values.Select(x => x.Value).Contains(x.CONO15));

  public static Expression<Func<T, bool>> Contains<TValue, TValueObject>(Func<T, TValue> getValue, params TValueObject[] values)
    where TValueObject : IValueObject<TValue>
    => x => values.Select(x => x.Value).Contains(getValue(x));


  //public static Expression<Func<T, bool>> Contains<T,T2, TValue>(Expression<Func<T, IValueObject<T>>> getValue, params IValueObject<T>[] values)
  //  where T: class
  //     => x => values.Select(x => x.Value).Contains(getValue.Compile()(x).Value);


  //  public static Expression<Func<T, bool>> Contains<T>(Expression<Func<T, IValueObject<T>>> getValue, params IValueObject<T>[] values)
  //    => x => values.Select(x => x.Value).Contains(getValue.Compile()(x).Value);

  //  public static Expression<Func<T, bool>> Contains<T, TValue>(Expression<Func<T, TValue>> getValue, params IValueObject<TValue>[] values)
  //    => x => values.Select(x => x.Value).Contains(getValue.Compile()(x));
}

public static class IValueObjectExtensions {
  public static Expression<Func<IEnumerable<IValueObject<T>>, bool>> Contains<T>(T value) => values => values.Select(x => x.Value).Contains(value);

  public static bool Contains<TV, T>(this IEnumerable<TV> valueObjects, T value) where TV : IValueObject<T> => valueObjects.Select(x => x.Value).Contains(value);
  public static Expression<Func<T, bool>> ContainsExpression<T, TVO, TV>(this IEnumerable<TVO> valueObjects, Func<T, TV> selector) where TVO : IValueObject<TV> => x => valueObjects.Select(vo => vo.Value).Contains(selector(x));
  public static Expression<Func<T, bool>> ContainsExpression<T, TVO, TV>(this IEnumerable<TVO> valueObjects, TV value) where TVO : IValueObject<TV> => x => valueObjects.Select(vo => vo.Value).Contains(value);

  public static IQueryable<T> ContainsValueObject<T, TValue, TValueObject>(this IQueryable<T> qry, Func<T, TValue> getValue, params TValueObject[] values)
    where TValueObject : IValueObject<TValue>
    => qry.Where(IValueObjectExpressions<T>.Contains(getValue, values));

  public static IEnumerable<T> SelectValues<TV, T>(this TV[] valueObjects) where TV : IValueObject<T> => valueObjects.Select(x => x.Value);
  public static IEnumerable<T> SelectValues<TV, T>(this IEnumerable<TV> valueObjects) where TV : IValueObject<T> => valueObjects.Select(x => x.Value);
  public static Expression<Func<TVO, TV>> ValueExpression<TVO, TV>() where TVO : IValueObject<TV> => vo => vo.Value;
  public static string ToString<T>(this IValueObject<T> valueObject) => valueObject.ToString() ?? string.Empty;

  public static IQueryable<TEntity> WhereValueObjectContains1<TEntity, T>(this IQueryable<TEntity> source, Expression<Func<TEntity, T>> propertySelector, IEnumerable<IValueObject<T>> values) {
    var valueList = values.Select(x => x.Value).ToList();
    var containsMethod = typeof(Enumerable).GetMethods().First(m => m.Name == nameof(string.Contains) && m.GetParameters().Length == 2).MakeGenericMethod(typeof(T));
    var containsExpression = Expression.Call(containsMethod, Expression.Constant(valueList), propertySelector.Body);
    var lambda = Expression.Lambda<Func<TEntity, bool>>(containsExpression, propertySelector.Parameters);
    return source.Where(lambda);
  }

  public static IQueryable<TEntity> WhereValueObjectContains2<TEntity, T>(this IQueryable<TEntity> source, Expression<Func<TEntity, T>> propertySelector, IEnumerable<IValueObject<T>> valueObjects) {
    var values = valueObjects.Select(x => x.Value).ToList();
    var parameter = propertySelector.Parameters[0];
    var property = propertySelector.Body;
    var valuesConstant = Expression.Constant(values);
    var containsMethod = typeof(List<T>).GetMethod("Contains");
    var containsCall = Expression.Call(valuesConstant, containsMethod, property);
    var lambda = Expression.Lambda<Func<TEntity, bool>>(containsCall, parameter);
    return source.Where(lambda);
  }

  public static IQueryable<T> WhereKeyIn<T>(this IQueryable<T> source, Expression<Func<T, string>> keySelector, IEnumerable<IValueObject<string>> valueObjects) {
    var values = valueObjects.Select(x => x.Value).ToList();
    // Build the expression: entity => values.Contains(entity.Key)
    var parameter = keySelector.Parameters[0];
    var property = keySelector.Body;
    var valuesConstant = Expression.Constant(values);
    var containsMethod = typeof(List<string>).GetMethod(nameof(string.Contains));
    var containsCall = Expression.Call(valuesConstant, containsMethod, property);
    var lambda = Expression.Lambda<Func<T, bool>>(containsCall, parameter);
    return source.Where(lambda);
  }

}