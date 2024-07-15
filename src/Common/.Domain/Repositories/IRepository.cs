using Common.Domain.Entities;

namespace Common.Domain.Repositories;

public interface IRepository<T, TId> : ICommandRepository<T, TId>, IQueryRepository<T, TId>
  where T : class, IEntityWithId<TId>
  //where TId : IEquatable<TId> 
  { }

public interface IRepositoryAsync<T> where T : class {
  IQueryable<T> Entities { get; }
  Task<T> GetByIdAsync(int id);
  Task<List<T>> GetAllAsync();
  Task<List<T>> GetPagedReponseAsync(int pageNumber, int pageSize);
  Task<T> AddAsync(T entity);
  Task UpdateAsync(T entity);
  Task DeleteAsync(T entity);
}

public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T> where T : class { }

public interface IRepository<T, TDb, TTable> : IRepository<T> where T : class where TTable : IQueryable<T> {
  TDb Database { get; }
  TTable Table { get; }
}

