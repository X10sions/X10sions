using System.Linq;
using System.Linq.Expressions;

namespace DynamicQueryNet.Utility {
  public sealed class OrderingHelper {

    static IOrderedQueryable<T> Ordering<T>(IQueryable<T> source, ParameterExpression parameter, string propertyName, string methodName) {
      var type = typeof(T);
      var propertyExpr = Expression.PropertyOrField(parameter, propertyName);
      var sort = Expression.Lambda(propertyExpr, parameter);
      var call = Expression.Call(typeof(Queryable), methodName, new[] { type, propertyExpr.Type }, source.Expression, Expression.Quote(sort));
      return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
    }

    public static IOrderedQueryable<T> OrderBy<T>(IQueryable<T> source, string propertyName, ParameterExpression parameter) => Ordering(source, parameter, propertyName, nameof(Queryable.OrderBy));
    public static IOrderedQueryable<T> OrderByDescending<T>(IQueryable<T> source, string propertyName, ParameterExpression parameter) => Ordering(source, parameter, propertyName, nameof(Queryable.OrderByDescending));
    public static IOrderedQueryable<T> ThenBy<T>(IOrderedQueryable<T> source, string propertyName, ParameterExpression parameter) => Ordering(source, parameter, propertyName, nameof(Queryable.ThenBy));
    public static IOrderedQueryable<T> ThenByDescending<T>(IOrderedQueryable<T> source, string propertyName, ParameterExpression parameter) => Ordering(source, parameter, propertyName, nameof(Queryable.ThenByDescending));

  }
}
