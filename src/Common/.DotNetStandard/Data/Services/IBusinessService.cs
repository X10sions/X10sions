using Common.Collections.Paged;
using Common.Domain.Entities;
using System.Linq.Expressions;

namespace Common.Data.Services;

public interface IBusinessService<T, TKey> where T : class, IEntityWithId<TKey>
  //where TKey : IEquatable<TKey>
  {
  T GetById(TKey id, params Expression<Func<T, object>>[] navigationPropertiesToLoad);
  T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertiesToLoad);
  List<T> GetAll(params Expression<Func<T, object>>[] navigationPropertiesToLoad);
  IPagedList<T> GetAllPaged(string orderBy, int startRowIndex = 1, int maxRows = 10, params Expression<Func<T, object>>[] navigationPropertiesToLoad);
  List<T> GetAllFiltered(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertiesToLoad);
  IPagedList<T> GetAllFilteredPaged(Expression<Func<T, bool>> predicate, string orderBy, int startRowIndex = 1, int maxRows = 10, params Expression<Func<T, object>>[] navigationPropertiesToLoad);

  IBusinessResult Add(T entity);
  IBusinessResult Update(T entity);
  IBusinessResult Delete(T entity);
}
