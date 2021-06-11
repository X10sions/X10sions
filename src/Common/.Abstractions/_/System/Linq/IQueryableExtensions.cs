using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq {
  public static class IQueryableExtensions {

    //public static async IQueryable<T> xFindAsync<T, TKey>(this IQueryable<T> source, object keyObject, CancellationToken cancellationToken = default)
    //  where T : IEquatable<TKey> => await Task.Run(() => source.Where(x => x.Equals(keyObject)), cancellationToken);

    //public static async IQueryable<T> xFindAsync<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> keyExpression, CancellationToken cancellationToken = default)
    //  where T : IEquatable<TKey> => Task.Run(() => source.Where(x => x.Equals(keyExpression)), cancellationToken);

    //public static IQueryable<T> WhereIdEquals<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> keyExpression, TKey otherKeyValue)
    //  where T : IEquatable<TKey> {
    //  var memberExpression = (MemberExpression)keyExpression.Body;
    //  var parameter = Expression.Parameter(typeof(T), "x");
    //  var property = Expression.Property(parameter, memberExpression.Member.Name);
    //  var equal = Expression.Equal(property, Expression.Constant(otherKeyValue));
    //  var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);
    //  return source.Where(lambda);
    //}

    public static async Task<T> xFirstOrDefaultAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
      => await Task.Run(() => source.FirstOrDefault(predicate), cancellationToken);

    public static async Task<T> xSingleOrDefaultAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
      => await Task.Run(() => source.SingleOrDefault(predicate), cancellationToken);

    public static async Task<List<T>> xToListAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default)
      => await Task.Run(() => source.ToList(), cancellationToken);

  }
}