using NHibernate;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace NHibernate_v5_2.Linq {
  public partial interface INhQueryProvider : IQueryProvider {
    Task<int> ExecuteDmlAsync<T>(QueryMode queryMode, Expression expression, CancellationToken cancellationToken);
  }

  public partial interface INhQueryProvider : IQueryProvider {
    //Since 5.2
    [Obsolete("Replaced by ISupportFutureBatchNhQueryProvider interface")]
    IFutureEnumerable<TResult> ExecuteFuture<TResult>(Expression expression);

    //Since 5.2
    [Obsolete("Replaced by ISupportFutureBatchNhQueryProvider interface")]
    IFutureValue<TResult> ExecuteFutureValue<TResult>(Expression expression);
    void SetResultTransformerAndAdditionalCriteria(IQuery query, NhLinqExpression nhExpression, IDictionary<string, Tuple<object, IType>> parameters);
    int ExecuteDml<T>(QueryMode queryMode, Expression expression);
    Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken);
  }

}