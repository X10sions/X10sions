using RCommon.Entities;
using System.Linq.Expressions;

namespace RCommon.Persistence.Crud;

public interface IEagerLoadableQueryable<T> : IReadOnlyRepository<T> where T: IBusinessEntity {//IQueryable<T>, 
  IEagerLoadableQueryable<T> Include(Expression<Func<T, object>> path);
  IEagerLoadableQueryable<T> ThenInclude<TPreviousProperty, TProperty>(Expression<Func<object, TProperty>> path);
}
