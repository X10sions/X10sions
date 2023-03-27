using Common.Data.Entities;
using System.Linq.Expressions;

namespace Common.Data.Repositories;

public interface ICommandRepository<T, TId> where T : class, IEntityWithId<TId> where TId : IEquatable<TId> {
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
