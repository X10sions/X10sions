using NHibernate;
using NHibernate.Engine;
using NHibernate.Multi;
using NHibernate.Type;
using NHibernate.Util;
using NHibernate_v5_2.Impl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace NHibernate_v5_2.Linq {
  public partial class DefaultQueryProvider : INhQueryProvider, IQueryProviderWithOptions, ISupportFutureBatchNhQueryProvider {

    // Since v5.1
    [Obsolete("Use ExecuteQuery(NhLinqExpression nhLinqExpression, IQuery query) instead")]
    protected virtual async Task<object> ExecuteQueryAsync(NhLinqExpression nhLinqExpression, IQuery query, NhLinqExpression nhQuery, CancellationToken cancellationToken) {
      cancellationToken.ThrowIfCancellationRequested();
      var results = await (query.ListAsync(cancellationToken)).ConfigureAwait(false);

      if (nhQuery.ExpressionToHqlTranslationResults?.PostExecuteTransformer != null) {
        try {
          return nhQuery.ExpressionToHqlTranslationResults.PostExecuteTransformer.DynamicInvoke(results.AsQueryable());
        } catch (TargetInvocationException e) {
          throw ReflectHelper.UnwrapTargetInvocationException(e);
        }
      }

      if (nhLinqExpression.ReturnType == NhLinqExpressionReturnType.Sequence) {
        return results.AsQueryable();
      }

      return results[0];
    }

    protected virtual Task<object> ExecuteQueryAsync(NhLinqExpression nhLinqExpression, IQuery query, CancellationToken cancellationToken) {
      if (cancellationToken.IsCancellationRequested) {
        return Task.FromCanceled<object>(cancellationToken);
      }
      // For avoiding breaking derived classes, call the obsolete method until it is dropped.
#pragma warning disable 618
      return ExecuteQueryAsync(nhLinqExpression, query, nhLinqExpression, cancellationToken);
#pragma warning restore 618
    }

    public Task<int> ExecuteDmlAsync<T>(QueryMode queryMode, Expression expression, CancellationToken cancellationToken) {
      if (Collection != null)
        throw new NotSupportedException("DML operations are not supported for filters.");
      if (cancellationToken.IsCancellationRequested) {
        return Task.FromCanceled<int>(cancellationToken);
      }
      try {

        var nhLinqExpression = new NhLinqDmlExpression<T>(queryMode, expression, Session.Factory);

        var query = Session.CreateQuery(nhLinqExpression);

        SetParameters(query, nhLinqExpression.ParameterValuesByName);
        _options?.Apply(query);
        return query.ExecuteUpdateAsync(cancellationToken);
      } catch (Exception ex) {
        return Task.FromException<int>(ex);
      }
    }
  }

  public partial class DefaultQueryProvider : INhQueryProvider, IQueryProviderWithOptions, ISupportFutureBatchNhQueryProvider {
    private static readonly MethodInfo CreateQueryMethodDefinition = ReflectHelper.GetMethodDefinition((INhQueryProvider p) => p.CreateQuery<object>(null));

    private readonly WeakReference<ISessionImplementor> _session;

    private readonly NhQueryableOptions _options;

    public DefaultQueryProvider(ISessionImplementor session) {
      // Short reference (no trackResurrection). If the session gets garbage collected, it will be in an unpredictable state:
      // better throw rather than resurrecting it.
      // https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/weak-references
      _session = new WeakReference<ISessionImplementor>(session);
    }

    public DefaultQueryProvider(ISessionImplementor session, object collection)
      : this(session) {
      Collection = collection;
    }

    private DefaultQueryProvider(ISessionImplementor session, object collection, NhQueryableOptions options)
      : this(session, collection) {
      _options = options;
    }

    public object Collection { get; }

    public virtual ISessionImplementor Session {
      get {
        if (!_session.TryGetTarget(out var target))
          throw new InvalidOperationException("Session has already been garbage collected");
        return target;
      }
    }

    public virtual object Execute(Expression expression) {
      var nhLinqExpression = PrepareQuery(expression, out var query);

      return ExecuteQuery(nhLinqExpression, query);
    }

    public TResult Execute<TResult>(Expression expression) => (TResult)Execute(expression);

    public IQueryProvider WithOptions(Action<NhQueryableOptions> setOptions) {
      if (setOptions == null) throw new ArgumentNullException(nameof(setOptions));

      var options = _options != null
        ? _options.Clone()
        : new NhQueryableOptions();
      setOptions(options);
      return new DefaultQueryProvider(Session, Collection, options);
    }

    public virtual IQueryable CreateQuery(Expression expression) {
      var m = CreateQueryMethodDefinition.MakeGenericMethod(expression.Type.GetGenericArguments()[0]);

      return (IQueryable)m.Invoke(this, new object[] { expression });
    }

    public virtual IQueryable<T> CreateQuery<T>(Expression expression) => new NhQueryable<T>(this, expression);

    //Since 5.2
    [Obsolete("Replaced by ISupportFutureBatchNhQueryProvider interface")]
    public virtual IFutureEnumerable<TResult> ExecuteFuture<TResult>(Expression expression) {
      var nhExpression = PrepareQuery(expression, out var query);

      var result = query.Future<TResult>();
      if (result is IDelayedValue delayedValue)
        SetupFutureResult(nhExpression, delayedValue);

      return result;
    }

    //Since 5.2
    [Obsolete("Replaced by ISupportFutureBatchNhQueryProvider interface")]
    public virtual IFutureValue<TResult> ExecuteFutureValue<TResult>(Expression expression) {
      var nhExpression = PrepareQuery(expression, out var query);
      var linqBatchItem = new Multi.LinqBatchItem<TResult>(query, nhExpression);
      return Engine.SessionImplementorExtensions.GetFutureBatch(Session).AddAsFutureValue(linqBatchItem);
    }

    //Since 5.2
    [Obsolete]
    private static void SetupFutureResult(NhLinqExpression nhExpression, IDelayedValue result) {
      if (nhExpression.ExpressionToHqlTranslationResults.PostExecuteTransformer == null)
        return;
      result.ExecuteOnEval = nhExpression.ExpressionToHqlTranslationResults.PostExecuteTransformer;
    }

    public async Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken) => (TResult)await ExecuteAsync(expression, cancellationToken).ConfigureAwait(false);

    public virtual Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken) {
      if (cancellationToken.IsCancellationRequested) {
        return Task.FromCanceled<object>(cancellationToken);
      }
      try {
        var nhLinqExpression = PrepareQuery(expression, out var query);
        return ExecuteQueryAsync(nhLinqExpression, query, cancellationToken);
      } catch (Exception ex) {
        return Task.FromException<object>(ex);
      }
    }

    protected virtual NhLinqExpression PrepareQuery(Expression expression, out IQuery query) {
      var nhLinqExpression = new NhLinqExpression(expression, Session.Factory);

      if (Collection == null) {
        query = Session.CreateQuery(nhLinqExpression);
      } else {
        query = Session.CreateFilter(Collection, nhLinqExpression);
      }

      SetParameters(query, nhLinqExpression.ParameterValuesByName);
      _options?.Apply(query);
      SetResultTransformerAndAdditionalCriteria(query, nhLinqExpression, nhLinqExpression.ParameterValuesByName);

      return nhLinqExpression;
    }

    // Since v5.1
    [Obsolete("Use ExecuteQuery(NhLinqExpression nhLinqExpression, IQuery query) instead")]
    protected virtual object ExecuteQuery(NhLinqExpression nhLinqExpression, IQuery query, NhLinqExpression nhQuery) {
      var results = query.List();

      if (nhQuery.ExpressionToHqlTranslationResults?.PostExecuteTransformer != null) {
        try {
          return nhQuery.ExpressionToHqlTranslationResults.PostExecuteTransformer.DynamicInvoke(results.AsQueryable());
        } catch (TargetInvocationException e) {
          throw ReflectHelper.UnwrapTargetInvocationException(e);
        }
      }

      if (nhLinqExpression.ReturnType == NhLinqExpressionReturnType.Sequence) {
        return results.AsQueryable();
      }

      return results[0];
    }

    protected virtual object ExecuteQuery(NhLinqExpression nhLinqExpression, IQuery query) =>
      // For avoiding breaking derived classes, call the obsolete method until it is dropped.
#pragma warning disable 618
      ExecuteQuery(nhLinqExpression, query, nhLinqExpression);
#pragma warning restore 618


    private static void SetParameters(IQuery query, IDictionary<string, Tuple<object, IType>> parameters) {
      foreach (var parameterName in query.NamedParameters) {
        var param = parameters[parameterName];

        if (param.Item1 == null) {
          if (typeof(IEnumerable).IsAssignableFrom(param.Item2.ReturnedClass) &&
            param.Item2.ReturnedClass != typeof(string)) {
            query.SetParameterList(parameterName, null, param.Item2);
          } else {
            query.SetParameter(parameterName, null, param.Item2);
          }
        } else {
          if (param.Item1 is IEnumerable && !(param.Item1 is string)) {
            query.SetParameterList(parameterName, (IEnumerable)param.Item1);
          } else if (param.Item2 != null) {
            query.SetParameter(parameterName, param.Item1, param.Item2);
          } else {
            query.SetParameter(parameterName, param.Item1);
          }
        }
      }
    }

    public virtual void SetResultTransformerAndAdditionalCriteria(IQuery query, NhLinqExpression nhExpression, IDictionary<string, Tuple<object, IType>> parameters) {
      if (nhExpression.ExpressionToHqlTranslationResults != null) {
        query.SetResultTransformer(nhExpression.ExpressionToHqlTranslationResults.ResultTransformer);

        foreach (var criteria in nhExpression.ExpressionToHqlTranslationResults.AdditionalCriteria) {
          criteria(query, parameters);
        }
      }
    }

    public int ExecuteDml<T>(QueryMode queryMode, Expression expression) {
      if (Collection != null)
        throw new NotSupportedException("DML operations are not supported for filters.");

      var nhLinqExpression = new NhLinqDmlExpression<T>(queryMode, expression, Session.Factory);

      var query = Session.CreateQuery(nhLinqExpression);

      SetParameters(query, nhLinqExpression.ParameterValuesByName);
      _options?.Apply(query);
      return query.ExecuteUpdate();
    }

    public IQuery GetPreparedQuery(Expression expression, out NhLinqExpression nhExpression) {
      nhExpression = PrepareQuery(expression, out var query);
      return query;
    }
  }

}