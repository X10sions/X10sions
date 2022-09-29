using System.Linq.Expressions;

namespace LinqToDB;
public static class IDataContextExtensions {
  public static IModificationHandler<T> GetModificationHandler<T>(this IDataContext dataContext, Expression<Func<T, bool>> wherePredicate) where T : class => new ModificationHandler<T>(dataContext, wherePredicate);
}