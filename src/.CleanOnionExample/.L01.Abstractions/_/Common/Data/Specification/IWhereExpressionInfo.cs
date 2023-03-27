using System.Linq.Expressions;

namespace Common.Data.Specification;

public interface IWhereExpressionInfo<T> {
  public Expression<Func<T, bool>> Filter { get; }
  public Func<T, bool> FilterFunc { get; }
}

public class WhereExpressionInfo<T> : IWhereExpressionInfo<T> {
  private readonly Lazy<Func<T, bool>> filterFunc;
  public Expression<Func<T, bool>> Filter { get; }
  public Func<T, bool> FilterFunc => filterFunc.Value;

  public WhereExpressionInfo(Expression<Func<T, bool>> filter) {
    if (filter == null) {
      throw new ArgumentNullException("filter");
    }
    Filter = filter;
    filterFunc = new Lazy<Func<T, bool>>(new Func<Func<T, bool>>(Filter.Compile));
  }
}
