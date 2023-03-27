using Common.Data.Entities;
using System.Linq.Expressions;

namespace Common.Data.Repositories;

public interface IQueryRepository<T, TId> where T : class, IEntityWithId<TId> where TId : IEquatable<TId> {
  IQueryable<T> Queryable { get; }
  bool Any();
  bool Any(Expression<Func<T, bool>> where);
  Task<bool> AnyAsync(CancellationToken cancellationToken = default);
  Task<bool> AnyAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default);
  long Count();
  long Count(Expression<Func<T, bool>> where);
  Task<long> CountAsync(CancellationToken cancellationToken = default);
  Task<long> CountAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default);
  IEnumerable<T> GetAll();
  Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
  T GetById(TId key);
  Task<T> GetByIdAsync(TId key, CancellationToken cancellationToken = default);
  //Task<T?> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
  //Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
  //IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate);
}