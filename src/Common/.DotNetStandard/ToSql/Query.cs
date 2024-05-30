using System.Collections;
using System.Linq.Expressions;

namespace Common.ToSql {
  public class Query<T> : IQueryable<T> {
    public Query(IQueryProvider provider, Expression expression) {
      Provider = provider;
      Expression = expression;
    }

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)this.Provider.Execute(this.Expression)).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this.Provider.Execute(this.Expression)).GetEnumerator();
    public Type ElementType => typeof(T);
    public Expression Expression { get; }
    public IQueryProvider Provider { get; }
  }

}
