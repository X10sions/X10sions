using Common.Domain.Entities;

namespace Common.Domain.Repositories;

public interface IWriteRepository<T> where T : class {
  //void Delete(Expression<Func<T, bool>> where);
  //void Delete(T entity);
  //void Insert(T item);
  //void Insert(IEnumerable<T> items);
  //void Update(IEnumerable<T> items);
  //void Update(Expression<Func<T, bool>> where);
}
public interface IWriteRepository<T, TKey> : IWriteRepository<T> where T : class, IEntityWithId<TKey> where TKey : IEquatable<TKey> {
  //void Delete(TKey key);
  //void Update(TKey key, T item);
  //void UpdatePartial(TKey key, object item);
}


public interface IWriteRepositoryAsync<T>: IWriteRepository<T> where T : class {
  //Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
  Task<int> DeleteAsync(IEnumerable<T> rows, CancellationToken token = default);
  //Task DeleteAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default);
  Task<int> InsertAsync(IEnumerable<T> rows, CancellationToken token = default);
  //Task InsertAsync(IEnumerable<T> items, CancellationToken cancellationToken = default);
  //Task<T> UpdateAsync(T item, CancellationToken cancellationToken = default);
  //Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
  Task<int> UpdateAsync(IEnumerable<T> rows, CancellationToken token = default);
  //Task UpdateAsync(IEnumerable<T> items, CancellationToken cancellationToken = default);
  //Task<int> InsertAsync(T item, CancellationToken cancellationToken = default);
}

public interface IWriteRepositoryAsync<T, TKey> : IWriteRepositoryAsync<T> where T : class, IEntityWithId<TKey> where TKey : IEquatable<TKey> {
  Task DeleteByIdAsync(TKey key, CancellationToken cancellationToken = default);
  //Task<TKey> InsertWithIdAsync<TKey>(T row, Func<T, TKey>? idSelector = null, CancellationToken token = default);
  Task UpdateByIdAsync(TKey key, T item, CancellationToken cancellationToken = default);
  //Task UpdatePartialAsync(TKey key, object item, CancellationToken cancellationToken = default);
}


public static class IWriteRepositoryExtensions {
  public async static Task<int> DeleteAsync<T>(this IWriteRepositoryAsync<T> repo, T row, CancellationToken token = default) where T : class => await repo.DeleteAsync([row], token);
  public async static Task<int> InsertAsync<T>(this IWriteRepositoryAsync<T> repo, T row, CancellationToken token = default) where T : class => await repo.InsertAsync([row], token);
  public async static Task<int> UpdateAsync<T>(this IWriteRepositoryAsync<T> repo, T row, CancellationToken token = default) where T : class => await repo.UpdateAsync([row], token);
}