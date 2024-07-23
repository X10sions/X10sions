using RCommon.Collections;
using RCommon.Entities;
using System.Linq.Expressions;

namespace RCommon.Persistence.Crud;

public interface ILinqRepository<T> : IReadOnlyRepository<T>, IWriteOnlyRepository<T>, IEagerLoadableQueryable<T>//, IQueryable<T>
  where T : IBusinessEntity {
  IQueryable<T> Queryable { get; }
}

public static class ILinqRepositoryExtensions {
  public static async Task<IPaginatedList<T>> GetPagedAsync<T>(this ILinqRepository<T> repository, Expression<Func<T, bool>> expression, Expression<Func<T, object>> orderByExpression, bool orderByAscending, int? pageIndex, int pageSize = 1, CancellationToken token = default)
    where T : IBusinessEntity {
    IQueryable<T> query;
    if (orderByAscending) {
      query = repository.GetQueryable(expression).OrderBy(orderByExpression);
    } else {
      query = repository.GetQueryable(expression).OrderByDescending(orderByExpression);
    }
    return await Task.FromResult(query.ToPaginatedList(pageIndex, pageSize));
  }

  public static async Task<IPaginatedList<T>> GetPagedAsync<T>(this ILinqRepository<T> repository, IPagedSpecification<T> specification, CancellationToken token = default) where T : IBusinessEntity
    => await repository.GetPagedAsync(specification.Predicate, specification.OrderByExpression, specification.OrderByAscending, specification.PageIndex, specification.PageSize, token);

  public static IQueryable<T> GetQueryable<T>(this ILinqRepository<T> repository, Expression<Func<T, bool>> predicate) where T : IBusinessEntity => repository.Queryable.Where(predicate);
  public static IQueryable<T> GetQueryable<T>(this ILinqRepository<T> repository, ISpecification<T> specification) where T : IBusinessEntity => repository.GetQueryable(specification.Predicate);

  public static async Task DeleteByPrimaryKeyAsync<T, TKey>(this ILinqRepository<T> repository, TKey primaryKey, CancellationToken cancellationToken = default) where T : IBusinessEntity {
    var entity = await repository.GetByPrimaryKeyAsync(primaryKey, cancellationToken);
    await repository.DeleteAsync(entity, cancellationToken);
  }


  //public static IEnumerator<T> GetEnumerator<T>(this ILinqRepository<T> repository) where T : IBusinessEntity => repository.Queryable.GetEnumerator();
  //public static Expression Expression<T>(this ILinqRepository<T> repository) where T : IBusinessEntity => repository.Queryable.Expression;
  //public static Type ElementType<T>(this ILinqRepository<T> repository) where T : IBusinessEntity => repository.Queryable.ElementType;
  //public static IQueryProvider Provider<T>(this ILinqRepository<T> repository) where T : IBusinessEntity => repository.Queryable.Provider;

}