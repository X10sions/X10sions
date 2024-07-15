using Common.Domain.Entities;
using Common.Domain.Repositories;

namespace Common.Domain;

public interface IUnitOfWork {
  IRepository<T> GetRepository<T, TKey>() where T : class, IEntityWithId<TKey>;
  IBusinessRepository<T, TKey> GetBusinessRepository<T, TKey>() where T : class, IEntityWithId<TKey>;
  int SaveChanges();
}
