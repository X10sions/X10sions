using NHibernate;
using NHibernate.Engine;
using System.Linq.Expressions;

namespace NHibernate_v5_2.Linq {
  // 6.0 TODO: merge into INhQueryProvider.
  public interface ISupportFutureBatchNhQueryProvider {
    IQuery GetPreparedQuery(Expression expression, out NhLinqExpression nhExpression);
    ISessionImplementor Session { get; }
  }

}