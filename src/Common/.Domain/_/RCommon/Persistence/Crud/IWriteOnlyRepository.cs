using Common.Domain.Entities;

namespace RCommon.Persistence.Crud;

public interface IWriteOnlyRepository<T> : INamedDataSource {
  Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
  Task InsertAsync(T entity, CancellationToken cancellationToken = default);
  Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
}
public interface IWriteOnlyRepository<T, TKey> : IWriteOnlyRepository<T>
  where T : IEntityWithId<TKey>
  where TKey : IEquatable<TKey> {
  Task DeleteByPrimaryKeyAsync(TKey id, CancellationToken cancellationToken = default);
  //void Delete(TKey key);
  //Task<TKey> InsertWithIdAsync<TKey>(T row, Func<T, TKey>? idSelector = null, CancellationToken token = default);
  //void Update(TKey key, T item);
  //void UpdatePartial(TKey key, object item);
  //Task UpdatePartialAsync(TKey key, object item, CancellationToken cancellationToken = default);
  //Task UpdateByPrimaryKeyAsync(TKey id, T item, CancellationToken cancellationToken = default);
}

public static class IWriteOnlyRepositoryExtensions {

  public static async Task<int> DeleteAsync<T>(this IWriteOnlyRepository<T> repository, IEnumerable<T> rows, CancellationToken cancellationToken = default)
    => await rows.ForEachCountAsync(async (row) => await repository.DeleteAsync(row, cancellationToken));

  public static async Task<int> InsertAsync<T>(this IWriteOnlyRepository<T> repository, IEnumerable<T> rows, CancellationToken cancellationToken = default)
    => await rows.ForEachCountAsync(async (row) => await repository.InsertAsync(row, cancellationToken));

  public static async Task<int> UpdateAsync<T>(this IWriteOnlyRepository<T> repository, IEnumerable<T> rows, CancellationToken cancellationToken = default)
    => await rows.ForEachCountAsync(async (row) => await repository.UpdateAsync(row, cancellationToken));

}