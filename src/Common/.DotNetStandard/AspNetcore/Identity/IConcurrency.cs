using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {

  public interface IConcurrency<TKey> : IId<TKey>
    where TKey : IEquatable<TKey> {
    string ConcurrencyStamp { get; set; }
    //IsConcurrencyToken: ConcurrencyStamp  = Guid.NewGuid().ToString();
  }

  public static class IConcurrencyExtensions {

    public static bool HasIdAndConcurrencyStamp<TKey>(this IConcurrency<TKey> concurrency, TKey id, string concurrencyStamp)
      where TKey : IEquatable<TKey> => concurrency.Id.Equals(id) && concurrency.ConcurrencyStamp.Equals(concurrencyStamp);

    public static bool HasIdAndConcurrencyStamp<TKey>(this IConcurrency<TKey> concurrency, IConcurrency<TKey> other)
      where TKey : IEquatable<TKey> => concurrency.HasIdAndConcurrencyStamp(other.Id, other.ConcurrencyStamp);

    public static Expression<Func<IConcurrency<TKey>, bool>> HasIdAndConcurrencyStampExpression<TKey>(this IConcurrency<TKey> concurrency, TKey id, string concurrencyStamp)
      where TKey : IEquatable<TKey> => _ => concurrency.HasIdAndConcurrencyStamp(id, concurrencyStamp);

    public static Expression<Func<IConcurrency<TKey>, bool>> HasIdAndConcurrencyStampExpression<TKey>(this IConcurrency<TKey> concurrency, IConcurrency<TKey> other)
      where TKey : IEquatable<TKey> => _ => concurrency.HasIdAndConcurrencyStamp(other);

    public static IConcurrency<TKey> FindByIdAndConcurrencyStamp<TKey>(this IQueryable<IConcurrency<TKey>> queryable, TKey id, string concurrencyStamp)
      where TKey : IEquatable<TKey> => queryable.FirstOrDefault(x => x.HasIdAndConcurrencyStamp(id, concurrencyStamp));

    public static IConcurrency<TKey> FindByIdAndConcurrencyStamp<TKey>(this IQueryable<IConcurrency<TKey>> queryable, IConcurrency<TKey> other)
      where TKey : IEquatable<TKey> => queryable.FindByIdAndConcurrencyStamp(other.Id, other.ConcurrencyStamp);

    public static async Task<IConcurrency<TKey>> FindByIdAndConcurrencyStampAsync<TKey>(this IQueryable<IConcurrency<TKey>> queryable, IConcurrency<TKey> other, CancellationToken cancellationToken = default)
      where TKey : IEquatable<TKey> => await Task.Run(() => queryable.FindByIdAndConcurrencyStamp(other), cancellationToken);

    public static async Task<IConcurrency<TKey>> FindByIdAndConcurrencyStampAsync<TKey>(this IQueryable<IConcurrency<TKey>> queryable, TKey id, string concurrencyStamp, CancellationToken cancellationToken = default)
      where TKey : IEquatable<TKey> => await Task.Run(() => queryable.FindByIdAndConcurrencyStamp(id, concurrencyStamp), cancellationToken);

  }
}