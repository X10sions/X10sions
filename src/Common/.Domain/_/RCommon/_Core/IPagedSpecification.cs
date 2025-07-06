using System.Linq.Expressions;

namespace RCommon;

public interface IPagedSpecification<T> : ISpecification<T> {
  public int PageIndex { get; }
  public int PageSize { get; }

  public Expression<Func<T, object>> OrderByExpression { get; }

  public bool OrderByAscending { get; set; }
}
