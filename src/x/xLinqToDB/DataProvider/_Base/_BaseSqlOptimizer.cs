using LinqToDB.Extensions;
using LinqToDB.SqlProvider;
using LinqToDB.SqlQuery;

namespace LinqToDB.DataProvider {
  public abstract class _BaseSqlOptimizer : BasicSqlOptimizer {
    public _BaseSqlOptimizer(SqlProviderFlags sqlProviderFlags) : base(sqlProviderFlags) { }

  }
}