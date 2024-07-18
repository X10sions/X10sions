using Common.Domain.Entities;

namespace Common.Domain.Repositories;

public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T> 
  where T : class { }

public interface IRepository<T, TKey> : IReadRepository<T, TKey>, IWriteRepository<T, TKey>
  where T : class, IEntityWithId<TKey> 
  where TKey : IEquatable<TKey> {
}

public interface IRepositoryAsync<T> : IReadRepositoryAsync<T>, IWriteRepositoryAsync<T>
  where T : class { }

public interface IRepositoryAsync<T, TKey> : IReadRepositoryAsync<T, TKey>, IWriteRepositoryAsync<T, TKey>
  where T : class, IEntityWithId<TKey>
  where TKey : IEquatable<TKey> { }

public interface IRepositoryAsync<T, TDatabase, TTable> : IRepositoryAsync<T> 
  where T : class 
  where TTable : IQueryable<T> {
  TDatabase Database { get; }
  TTable Table { get; }
}

