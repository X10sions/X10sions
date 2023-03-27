namespace Common.Data;

public interface IUnitOfWork {
  IRepository<T> GetRepository<T, TKey>() where T : class, IEntity<TKey> where TKey : IEquatable<TKey>;
  IBusinessRepository<T, TKey> GetBusinessRepository<T, TKey>() where T : class, IEntity<TKey> where TKey : IEquatable<TKey>;
  int SaveChanges();
}
