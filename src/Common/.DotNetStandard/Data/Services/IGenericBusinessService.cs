using Common.Domain.Entities;
using System.Linq.Expressions;

namespace Common.Data.Services;

public interface IGenericBusinessService<T, TKey> : IBusinessService<T, TKey> where T : class, IEntityWithId<TKey>
  //where TKey : IEquatable<TKey>
  {
  void OnAdding(T entity);
  void OnAdded(T entity);
  void OnUpdating(T entity);
  void OnUpdated(T entity);
  void OnDeleting(T entity);
  void OnDeleted(T entity);
  Expression<Func<T, object>>[] GetDefaultLoadProperties();
}
