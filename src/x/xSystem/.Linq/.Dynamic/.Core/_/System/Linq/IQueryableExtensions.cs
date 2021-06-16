using System.Linq;
using System.Linq.Dynamic.Core;

namespace System.Linq {
  public static class IQueryableExtensions {

    public static IQueryable<t> DynamicLinqOrderBy<t>(this IQueryable<t> queryable, string ordering, params object[] values) => string.IsNullOrWhiteSpace(ordering) ? queryable : queryable.OrderBy(ordering, values);

  }
}