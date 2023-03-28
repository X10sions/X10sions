using Common.Data.Entities;
using System.Linq.Expressions;

namespace Common.Data.Repositories;

public abstract class RepositoryBase<T, TId> : IRepository<T, TId> where T : class, IEntityWithId<TId> where TId : IEquatable<TId> {
  private readonly ICommandRepository<T, TId> _commandRepository;
  private readonly IQueryRepository<T, TId> _queryRepository;

  protected RepositoryBase(ICommandRepository<T, TId> commandRepository, IQueryRepository<T, TId> queryRepository) {
    _commandRepository = commandRepository;
    _queryRepository = queryRepository;
  }

  public IQueryable<T> Queryable => _queryRepository.Queryable;

  public bool Any() => _queryRepository.Any();
  public bool Any(Expression<Func<T, bool>> where) => _queryRepository.Any(where);
  public Task<bool> AnyAsync(CancellationToken cancellationToken = default) => _queryRepository.AnyAsync(cancellationToken);
  public Task<bool> AnyAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default) => _queryRepository.AnyAsync(where, cancellationToken);
  public long Count() => _queryRepository.Count();
  public long Count(Expression<Func<T, bool>> where) => _queryRepository.Count(where);
  public Task<long> CountAsync(CancellationToken cancellationToken = default) => _queryRepository.CountAsync(cancellationToken);
  public Task<long> CountAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default) => _queryRepository.CountAsync(where, cancellationToken);
  public void Delete(TId key) => _commandRepository.Delete(key);
  public void Delete(Expression<Func<T, bool>> where) => _commandRepository.Delete(where);
  public Task DeleteAsync(TId key, CancellationToken cancellationToken = default) => _commandRepository.DeleteAsync(key, cancellationToken);
  public Task DeleteAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default) => _commandRepository.DeleteAsync(where, cancellationToken);
  public IEnumerable<T> GetAll() => _queryRepository.GetAll();
  public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default) => _queryRepository.GetAllAsync(cancellationToken);
  public T GetById(TId key) => _queryRepository.GetById(key);
  public Task<T> GetByIdAsync(TId key, CancellationToken cancellationToken = default) => _queryRepository.GetByIdAsync(key, cancellationToken);
  public void Insert(T item) => _commandRepository.Insert(item);
  public Task InsertAsync(T item, CancellationToken cancellationToken = default) => _commandRepository.InsertAsync(item, cancellationToken);
  public void InsertRange(IEnumerable<T> items) => _commandRepository.InsertRange(items);
  public Task InsertRangeAsync(IEnumerable<T> items, CancellationToken cancellationToken = default) => _commandRepository.InsertRangeAsync(items, cancellationToken);
  public void Update(TId key, T item) => _commandRepository.Update(key, item);
  public Task UpdateAsync(TId key, T item, CancellationToken cancellationToken = default) => _commandRepository.UpdateAsync(key, item, cancellationToken);
  public Task<T> UpdateAsync(T item, CancellationToken cancellationToken = default) => _commandRepository.UpdateAsync(item, cancellationToken);
  public void UpdatePartial(TId key, object item) => _commandRepository.UpdatePartial(key, item);
  public Task UpdatePartialAsync(TId key, object item, CancellationToken cancellationToken = default) => _commandRepository.UpdatePartialAsync(key, item, cancellationToken);
  public void UpdateRange(IEnumerable<T> items) => _commandRepository.UpdateRange(items);
  public Task UpdateRangeAsync(IEnumerable<T> items, CancellationToken cancellationToken = default) => _commandRepository.UpdateRangeAsync(items, cancellationToken);
}
