using Common.Domain.Entities;
using System.Linq.Expressions;

namespace Common.Domain.Repositories;

public abstract class RepositoryBase<T, TId>(
  IWriteRepositoryAsync<T, TId> commandRepository,
  IReadRepositoryAsync<T, TId> queryRepository
  ) : IRepositoryAsync <T, TId>
  where T : class, IEntityWithId<TId>
  where TId : IEquatable<TId> {
  public IQuery<T> Query => queryRepository.Query;
  public IQueryable<T> Queryable => queryRepository.Queryable;

  //public bool Any() => queryRepository.Any();
  //public bool Any(Expression<Func<T, bool>> where) => queryRepository.Any(where);
  //public Task<bool> AnyAsync(CancellationToken cancellationToken = default) => queryRepository.AnyAsync(cancellationToken);
  //public Task<bool> AnyAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default) => queryRepository.AnyAsync(where, cancellationToken);
  //public long Count() => queryRepository.Count();
  //public long Count(Expression<Func<T, bool>> where) => queryRepository.Count(where);
  //public Task<long> CountAsync(CancellationToken cancellationToken = default) => queryRepository.CountAsync(cancellationToken);
  //public Task<long> CountAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default) => queryRepository.CountAsync(where, cancellationToken);
  //public void Delete(TId key) => commandRepository.Delete(key);
  //public void Delete(Expression<Func<T, bool>> where) => commandRepository.Delete(where);
  //public Task DeleteAsync(TId key, CancellationToken cancellationToken = default) => commandRepository.DeleteAsync(key, cancellationToken);
  public Task<int> DeleteAsync(IEnumerable<T> rows, CancellationToken token = default) => commandRepository.DeleteAsync(rows, token);
  public Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => queryRepository.GetAsync(predicate, cancellationToken);
  public Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => queryRepository.GetListAsync(predicate, cancellationToken);
  //public Task DeleteAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default) => commandRepository.DeleteAsync(where, cancellationToken);
  //public IEnumerable<T> GetAll() => queryRepository.GetAll();
  //public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default) => queryRepository.GetAllAsync(cancellationToken);
  //public T GetById(TId key) => queryRepository.GetById(key);
  //public Task<T> GetByIdAsync(TId key, CancellationToken cancellationToken = default) => queryRepository.GetByIdAsync(key, cancellationToken);
  //public void Insert(T item) => commandRepository.Insert(item);
  public Task InsertAsync(T item, CancellationToken cancellationToken = default) => commandRepository.InsertAsync(item, cancellationToken);
  public Task<int> InsertAsync(IEnumerable<T> rows, CancellationToken token = default) => commandRepository.InsertAsync(rows, token);
  //public Task<TKey> InsertWithIdAsync<TKey>(T row, Func<T, TKey>? idSelector = null, CancellationToken token = default) => commandRepository.InsertWithIdAsync(row, idSelector, token);
  //public void InsertRange(IEnumerable<T> items) => commandRepository.InsertRange(items);
  //public Task InsertRangeAsync(IEnumerable<T> items, CancellationToken cancellationToken = default) => commandRepository.InsertRangeAsync(items, cancellationToken);
  //public void Update(TId key, T item) => commandRepository.Update(key, item);
  //public Task UpdateAsync(TId key, T item, CancellationToken cancellationToken = default) => commandRepository.UpdateAsync(key, item, cancellationToken);
  public Task<int> UpdateAsync(T item, CancellationToken cancellationToken = default) => commandRepository.UpdateAsync(item, cancellationToken);

  public Task<int> UpdateAsync(IEnumerable<T> rows, CancellationToken token = default) {
    throw new NotImplementedException();
  }
  //public Task<T> UpdateAsync(T item, CancellationToken cancellationToken = default) => commandRepository.UpdateAsync(item, cancellationToken);
  //public void UpdatePartial(TId key, object item) => commandRepository.UpdatePartial(key, item);
  //public Task UpdatePartialAsync(TId key, object item, CancellationToken cancellationToken = default) => commandRepository.UpdatePartialAsync(key, item, cancellationToken);
  //public void UpdateRange(IEnumerable<T> items) => commandRepository.UpdateRange(items);
  //public Task UpdateRangeAsync(IEnumerable<T> items, CancellationToken cancellationToken = default) => commandRepository.UpdateRangeAsync(items, cancellationToken);
}
