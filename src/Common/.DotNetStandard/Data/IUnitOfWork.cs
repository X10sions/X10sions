using Common.Data.Entities;

namespace Common.Data;

public interface IUnitOfWork {
  IRepository<T> GetRepository<T, TKey>() where T : class, IEntityWithId<TKey>;
  IBusinessRepository<T, TKey> GetBusinessRepository<T, TKey>() where T : class, IEntityWithId<TKey>;
  int SaveChanges();
}
