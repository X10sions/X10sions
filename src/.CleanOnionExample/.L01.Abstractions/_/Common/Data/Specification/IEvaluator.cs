namespace Common.Data.Specification;

public interface IEvaluator {
  bool IsCriteriaEvaluator { get; }
  IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class;
}

public class OrderEvaluator : IEvaluator, IInMemoryEvaluator {
  private OrderEvaluator() { }
  public static OrderEvaluator Instance { get; } = new OrderEvaluator();

  public bool IsCriteriaEvaluator { get; } = false;

  public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class {
    if (specification.OrderExpressions != null) {
      if (specification.OrderExpressions.Count(x => x.OrderType == OrderTypeEnum.OrderBy
              || x.OrderType == OrderTypeEnum.OrderByDescending) > 1) {
        throw new Exception("DuplicateOrderChainException");
      }
      IOrderedQueryable<T>? orderedQuery = null;
      foreach (var orderExpression in specification.OrderExpressions) {
        if (orderExpression.OrderType == OrderTypeEnum.OrderBy) {
          orderedQuery = query.OrderBy(orderExpression.KeySelector);
        } else if (orderExpression.OrderType == OrderTypeEnum.OrderByDescending) {
          orderedQuery = query.OrderByDescending(orderExpression.KeySelector);
        } else if (orderExpression.OrderType == OrderTypeEnum.ThenBy) {
          orderedQuery = orderedQuery.ThenBy(orderExpression.KeySelector);
        } else if (orderExpression.OrderType == OrderTypeEnum.ThenByDescending) {
          orderedQuery = orderedQuery.ThenByDescending(orderExpression.KeySelector);
        }
      }
      if (orderedQuery != null) {
        query = orderedQuery;
      }
    }
    return query;
  }

  public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification) {
    if (specification.OrderExpressions != null) {
      if (specification.OrderExpressions.Count(x => x.OrderType == OrderTypeEnum.OrderBy
              || x.OrderType == OrderTypeEnum.OrderByDescending) > 1) {
        throw new Exception("DuplicateOrderChainException");
      }
      IOrderedEnumerable<T>? orderedQuery = null;
      foreach (var orderExpression in specification.OrderExpressions) {
        if (orderExpression.OrderType == OrderTypeEnum.OrderBy) {
          orderedQuery = query.OrderBy(orderExpression.KeySelectorFunc);
        } else if (orderExpression.OrderType == OrderTypeEnum.OrderByDescending) {
          orderedQuery = query.OrderByDescending(orderExpression.KeySelectorFunc);
        } else if (orderExpression.OrderType == OrderTypeEnum.ThenBy) {
          orderedQuery = orderedQuery.ThenBy(orderExpression.KeySelectorFunc);
        } else if (orderExpression.OrderType == OrderTypeEnum.ThenByDescending) {
          orderedQuery = orderedQuery.ThenByDescending(orderExpression.KeySelectorFunc);
        }
      }
      if (orderedQuery != null) {
        query = orderedQuery;
      }
    }
    return query;
  }
}

public class PaginationEvaluator : IEvaluator, IInMemoryEvaluator {
  private PaginationEvaluator() { }
  public static PaginationEvaluator Instance { get; } = new PaginationEvaluator();

  public bool IsCriteriaEvaluator { get; } = false;

  public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class {
    // If skip is 0, avoid adding to the IQueryable. It will generate more optimized SQL that way.
    if (specification.Skip != null && specification.Skip != 0) {
      query = query.Skip(specification.Skip.Value);
    }
    if (specification.Take != null) {
      query = query.Take(specification.Take.Value);
    }
    return query;
  }

  public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification) {
    if (specification.Skip != null && specification.Skip != 0) {
      query = query.Skip(specification.Skip.Value);
    }
    if (specification.Take != null) {
      query = query.Take(specification.Take.Value);
    }
    return query;
  }
}

public class SearchEvaluator : IInMemoryEvaluator {
  private SearchEvaluator() { }
  public static SearchEvaluator Instance { get; } = new SearchEvaluator();

  public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification) {
    foreach (var searchGroup in specification.SearchCriterias.GroupBy(x => x.SearchGroup)) {
      query = query.Where(x => searchGroup.Any(c => c.SelectorFunc(x).Like(c.SearchTerm)));
    }
    return query;
  }
}

public class WhereEvaluator : IEvaluator, IInMemoryEvaluator {
  private WhereEvaluator() { }
  public static WhereEvaluator Instance { get; } = new WhereEvaluator();

  public bool IsCriteriaEvaluator { get; } = true;

  public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class {
    foreach (var info in specification.WhereExpressions) {
      query = query.Where(info.Filter);
    }
    return query;
  }

  public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification) {
    foreach (var info in specification.WhereExpressions) {
      query = query.Where(info.FilterFunc);
    }
    return query;
  }
}