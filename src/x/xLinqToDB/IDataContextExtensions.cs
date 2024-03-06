using System.Linq.Expressions;

namespace LinqToDB;
public static class IDataContextExtensions {
  public static IModificationHandler<T> GetModificationHandler<T>(this IDataContext dataContext, Expression<Func<T, bool>> wherePredicate) where T : class => new ModificationHandler<T>(dataContext, wherePredicate);
  public static IQueryable<T> GetQueryable<T>(this IDataContext dataContext, Expression<Func<T, bool>> where) where T : class => dataContext.GetTable<T>().Where(where);
}