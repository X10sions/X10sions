using Common.Domain.Entities;
using System.Linq.Expressions;

namespace Common.Domain.Repositories;

public interface IReadRepository<T> where T : class {
  //bool Any();
  //bool Any(Expression<Func<T, bool>> where);
  //long Count();
  //long Count(Expression<Func<T, bool>> predicate);
  //long Count(Expression<Func<T, bool>> where);
  //IEnumerable<T> GetAll();
  //List<T> GetAll(params Expression<Func<T, object>>[] navigationPropertiesToLoad);
  //IPagedList<T> GetAllPaged(string orderBy, int startRowIndex = 0, int maxRows = 10, params Expression<Func<T, object>>[] navigationPropertiesToLoad);
  //List<T> GetAllFiltered(Expression<Func<T, bool>> prediacte, params Expression<Func<T, object>>[] navigationPropertiesToLoad);
  //IPagedList<T> GetAllFilteredPaged(Expression<Func<T, bool>> predicate, string orderBy, int startRowIndex = 0, int maxRows = 10, params Expression<Func<T, object>>[] navigationPropertiesToLooad);
  //T GetById(TKey Id);
  //T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertiesToLoad);
  IQuery<T> Query { get; }
  IQueryable<T> Queryable { get; }
  //void Update(T entity);
}

public interface IReadRepository<T, TKey> : IReadRepository<T> where T : class, IEntityWithId<TKey> where TKey : IEquatable<TKey> {
  //T GetById(TKey key);
  //IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate);
}

public interface IReadRepositoryAsync<T> : IReadRepository<T>  where T : class {
  //Task<bool> AnyAsync(CancellationToken cancellationToken = default);
  //Task<bool> AnyAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default);
  //Task<long> CountAsync(CancellationToken cancellationToken = default);
  //Task<long> CountAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default);
  //Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
  Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
  Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
  //Task<List<T>> GetPagedReponseAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}

public interface IReadRepositoryAsync<T, TKey> : IReadRepositoryAsync<T> where T : class, IEntityWithId<TKey> where TKey : IEquatable<TKey> {
  Task<T?> GetByIdAsync(TKey key, CancellationToken cancellationToken = default);
}

public static class IReadRepositoryExtensions {
  public async static Task<IEnumerable<T>> GetListAsync<T>(this IReadRepositoryAsync<T> repo, CancellationToken token = default) where T : class => await repo.GetListAsync(x => true, token);
}