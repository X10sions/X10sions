using System.Linq.Expressions;

namespace Common;

public enum WhereCompare {
  IsNull,
  IsNotNull,
  In,
  Between,
  GreaterThan,
  GreaterThanOrEqual,
  LessThan,
  LessThanOrEqual
}

public class WhereCompareHelper {
  //Expression<Func<T, bool>>
}

public static class WhereCompareExtensions {

  private static readonly Dictionary<WhereCompare, Expression<Func<IComparable, IComparable[], bool>>> WhereCompareExpressions = new Dictionary<WhereCompare, Expression<Func<IComparable, IComparable[], bool>>>  {
    { WhereCompare.IsNull, (x, values)  => x == null },
    { WhereCompare.IsNotNull , (x, values)  =>  x != null },
    { WhereCompare.GreaterThan ,(x, values) =>   x.CompareTo(values.Max()) > 0 },
    { WhereCompare.GreaterThanOrEqual, (x, values) => x.CompareTo(values.Max()) >= 0 },
    { WhereCompare.In , (x, values) => values.Contains(x) },
    { WhereCompare.LessThan , (x, values) => x.CompareTo(values.Min()) < 0 },
    { WhereCompare.LessThanOrEqual , (x, values) => x.CompareTo(values.Min()) <= 0 },
    { WhereCompare.Between ,  (x, values) => x.CompareTo(values.Min()) >= 0 && x.CompareTo(values.Max()) <= 0 }
  };

  //queryable = desc? queryable.OrderByDescending(OrderFunctions[orderByField])	: queryable.OrderBy(OrderFunctions[orderByField]);


  //  private static readonly Dictionary<WhereCompare, Expression<Func<object, bool>> WhereDic = new Dictionary<WhereCompare, Expression<Func<object, bool>> {
  //  { WhereCompare.IsNull, x => qry.Where(x => selector(x) == null) }
  //  };

  public static IQueryable<T> WhereDic<T, TV>(this IQueryable<T> qry, Func<T, TV> selector, WhereCompare comparison, params TV[] values) where TV : IComparable<TV> {
    //Expression<Func<TV, TV[],bool>> exprFunc = WhereCompareExpressions[comparison];
    var exprFunc = WhereCompareExpressions[comparison];
    if (exprFunc is null) throw new NotImplementedException();
    //var rrr = qry.Where(x => exprFunc.Compile()(selector(x), values));
    throw new NotImplementedException();
  }

  //public static IQueryable<T> Where<T, TV>(this IQueryable<T> qry, Func<T, TV> selector, WhereCompare comparison, params TV[] values) where TV : IComparable<TV> {
  //  return comparison switch {
  //    WhereCompare.IsNull => qry.Where(x => selector(x) == null),
  //    WhereCompare.IsNotNull => qry.Where(x => selector(x) != null),
  //    WhereCompare.GreaterThan => qry.Where(x => selector(x).CompareTo(values.Max()) > 0),
  //    WhereCompare.GreaterThanOrEqual => qry.Where(x => selector(x).CompareTo(values.Max()) >= 0),
  //    WhereCompare.In => qry.Where(x => values.Contains(selector(x))),
  //    WhereCompare.LessThan => qry.Where(x => selector(x).CompareTo(values.Min()) < 0),
  //    WhereCompare.LessThanOrEqual => qry.Where(x => selector(x).CompareTo(values.Min()) <= 0),
  //    WhereCompare.Between => qry.Where(x => selector(x).CompareTo(values.Min()) >= 0 && selector(x).CompareTo(values.Max()) <= 0),
  //    _ => throw new NotImplementedException()
  //  };
  //}

  public static IQueryable<T> Where<T, TV>(this IQueryable<T> qry, Expression<Func<T, TV>> selector, WhereCompare comparison, params TV[] values) where TV : IComparable<TV> {
    var parameter = selector.Parameters[0];
    var member = selector.Body;
    // Handle Null/NotNull checks directly
    if (comparison == WhereCompare.IsNull) {
      var nullCheck = Expression.Equal(member, Expression.Constant(null, typeof(object)));
      return qry.Where(Expression.Lambda<Func<T, bool>>(nullCheck, parameter));
    }
    if (comparison == WhereCompare.IsNotNull) {
      var notNullCheck = Expression.NotEqual(member, Expression.Constant(null, typeof(object)));
      return qry.Where(Expression.Lambda<Func<T, bool>>(notNullCheck, parameter));
    }
    // For structural comparisons, resolve boundary values
    TV maxValue = values != null && values.Length > 0 ? values.Max() : default!;
    TV minValue = values != null && values.Length > 0 ? values.Min() : default!;
    Expression body = comparison switch {
      WhereCompare.GreaterThan => Expression.GreaterThan(member, Expression.Constant(maxValue, typeof(TV))),
      WhereCompare.GreaterThanOrEqual => Expression.GreaterThanOrEqual(member, Expression.Constant(maxValue, typeof(TV))),
      WhereCompare.LessThan => Expression.LessThan(member, Expression.Constant(minValue, typeof(TV))),
      WhereCompare.LessThanOrEqual => Expression.LessThanOrEqual(member, Expression.Constant(minValue, typeof(TV))),
      WhereCompare.Between => Expression.AndAlso(
          Expression.GreaterThanOrEqual(member, Expression.Constant(minValue, typeof(TV))),
          Expression.LessThanOrEqual(member, Expression.Constant(maxValue, typeof(TV)))
      ),
      WhereCompare.In => Expression.Call(typeof(Enumerable), nameof(Enumerable.Contains), new[] { typeof(TV) }, Expression.Constant(values), member),
      _ => throw new NotImplementedException()
    };
    return qry.Where(Expression.Lambda<Func<T, bool>>(body, parameter));
  }


}