namespace Common.Data;

public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T> where T : class { }

public interface IRepository<T, TDb, TTable> : IRepository<T> where T : class where TTable : IQueryable<T> {
  TDb Database { get; }
  TTable Table { get; }
}

public interface IReadRepository<T>   where T : class {
  IQuery<T> Query { get; }
}

public static class IReadRepositoryExtetnsions {
  //  public async static Task<T?> FirstOrDefaultAsync<T>(this IReadRepository<T> repo, CancellationToken token = default) where T : class => await repo.FirstOrDefaultAsync(x => true, token);
  //  public async static Task<List<T>> ListAsync<T>(this IReadRepository<T> repo, Expression<Func<T, bool>> prediacte, CancellationToken token = default) where T : class    => await repo.ListAsync(prediacte, null, null, token);
  //  public async static Task<List<T>> ListAsync<T>(this IReadRepository<T> repo, CancellationToken token = default) where T : class => await repo.ListAsync(x => true, token);
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