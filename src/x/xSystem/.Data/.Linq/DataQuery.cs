using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq.Provider;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace System.Data.Linq {
  internal sealed class DataQuery<T> : IOrderedQueryable<T>, IQueryProvider, IListSource {
    private DataContext context;
    private Expression queryExpression;
    private IBindingList cachedList;
    Expression IQueryable.Expression => queryExpression;
    Type IQueryable.ElementType => typeof(T);
    IQueryProvider IQueryable.Provider => this;

    bool IListSource.ContainsListCollection => false;

    public DataQuery(DataContext context, Expression expression) {
      this.context = context;
      queryExpression = expression;
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    IQueryable IQueryProvider.CreateQuery(Expression expression) {
      if (expression == null) {
        throw Error.ArgumentNull("expression");
      }
      var elementType = TypeSystem.GetElementType(expression.Type);
      var type = typeof(IQueryable<>).MakeGenericType(elementType);
      if (!type.IsAssignableFrom(expression.Type)) {
        throw Error.ExpectedQueryableArgument("expression", type);
      }
      var type2 = typeof(DataQuery<>).MakeGenericType(elementType);
      return (IQueryable)Activator.CreateInstance(type2, context, expression);
    }

    IQueryable<S> IQueryProvider.CreateQuery<S>(Expression expression) {
      if (expression == null) {
        throw Error.ArgumentNull(nameof(expression));
      }
      if (!typeof(IQueryable<S>).IsAssignableFrom(expression.Type)) {
        throw Error.ExpectedQueryableArgument("expression", typeof(IEnumerable<S>));
      }
      return new DataQuery<S>(context, expression);
    }

    object IQueryProvider.Execute(Expression expression) => context.Provider.Execute(expression).ReturnValue;

    S IQueryProvider.Execute<S>(Expression expression) => (S)context.Provider.Execute(expression).ReturnValue;

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)context.Provider.Execute(queryExpression).ReturnValue).GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>)context.Provider.Execute(queryExpression).ReturnValue).GetEnumerator();

    IList IListSource.GetList() {
      if (cachedList == null) {
        cachedList = GetNewBindingList();
      }
      return cachedList;
    }

    internal IBindingList GetNewBindingList() => BindingList.Create<T>(context, this);

    public override string ToString() => context.Provider.GetQueryText(queryExpression);
  }
}