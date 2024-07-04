using Common.Domain.Entities;
using System.Linq.Expressions;

namespace Common.Data.Repositories;

public interface ICommandRepository<T, TId> where T : class, IEntityWithId<TId> //where TId : IEquatable<TId>
  {
  void Delete(TId key);
  void Delete(Expression<Func<T, bool>> where);
  Task DeleteAsync(TId key, CancellationToken cancellationToken = default);
  Task DeleteAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default);
  void Insert(T item);
  Task InsertAsync(T item, CancellationToken cancellationToken = default);
  void InsertRange(IEnumerable<T> items);
  Task InsertRangeAsync(IEnumerable<T> items, CancellationToken cancellationToken = default);
  void Update(TId key, T item);
  Task<T> UpdateAsync(T item, CancellationToken cancellationToken = default);
  Task UpdateAsync(TId key, T item, CancellationToken cancellationToken = default);
  void UpdatePartial(TId key, object item);
  Task UpdatePartialAsync(TId key, object item, CancellationToken cancellationToken = default);
  void UpdateRange(IEnumerable<T> items);
  Task UpdateRangeAsync(IEnumerable<T> items, CancellationToken cancellationToken = default);

}

public interface IWriteRepository<T> where T : class {
  Task<int> DeleteAsync(IEnumerable<T> rows, CancellationToken token = default);
  Task<int> InsertAsync(IEnumerable<T> rows, CancellationToken token = default);
  Task<TKey> InsertWithIdAsync<TKey>(T row, Func<T, TKey>? idSelector = null, CancellationToken token = default);
  Task<int> UpdateAsync(T row, CancellationToken token = default);
}

public static class IWriteRepositoryExtetnsions {
  public async static Task<int> DeleteAsync<T>(this IWriteRepository<T> repo, T row, CancellationToken token = default) where T : class => await repo.DeleteAsync(new[] { row }, token);
  public async static Task<int> InsertAsync<T>(this IWriteRepository<T> repo, T row, CancellationToken token = default) where T : class => await repo.InsertAsync(new[] { row }, token);

}