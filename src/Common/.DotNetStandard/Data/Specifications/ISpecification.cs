using System.Linq.Expressions;

namespace Common.Data.Specifications;

/// <summary>
/// Based on https://github.com/ardalis/Specification/tree/main/Specification/src/Ardalis.Specification
/// </summary>
public interface ISpecification<T> {
  IEnumerable<IWhereExpressionInfo<T>> WhereExpressions { get; }
  IEnumerable<IOrderExpressionInfo<T>> OrderExpressions { get; }
  IEnumerable<IIncludeExpressionInfo> IncludeExpressions { get; }
  IEnumerable<string> IncludeStrings { get; }
  IEnumerable<ISearchExpressionInfo<T>> SearchCriterias { get; }
  int? Take { get; set; }
  int? Skip { get; set; }
  Func<IEnumerable<T>, IEnumerable<T>>? PostProcessingAction { get; set; }
  bool CacheEnabled { get; set; }
  string? CacheKey { get; set; }
  bool AsNoTracking { get; set; }
  bool AsSplitQuery { get; set; }
  bool AsNoTrackingWithIdentityResolution { get; set; }
  bool IgnoreQueryFilters { get; set; }
  IEnumerable<T> Evaluate(IEnumerable<T> entities);
  bool IsSatisfiedBy(T entity);
}

public interface ISpecification<T, TResult> : ISpecification<T> {
  Expression<Func<T, TResult>>? Selector { get; set; }
  new Func<IEnumerable<TResult>, IEnumerable<TResult>>? PostProcessingAction { get; set; }
  new IEnumerable<TResult> Evaluate(IEnumerable<T> entities);
}