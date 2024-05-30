using System.Linq.Expressions;

namespace Common.Data.Specification;
public static class ISpecificationBuilderExtensions {
  public static ISpecificationBuilder<T> Where<T>(this ISpecificationBuilder<T> specificationBuilder, Expression<Func<T, bool>> criteria)
      => Where(specificationBuilder, criteria, true);

  public static ISpecificationBuilder<T> Where<T>(
      this ISpecificationBuilder<T> specificationBuilder,
      Expression<Func<T, bool>> criteria,
      bool condition) {
    if (condition) {
      ((List<WhereExpressionInfo<T>>)specificationBuilder.Specification.WhereExpressions).Add(new WhereExpressionInfo<T>(criteria));
    }
    return specificationBuilder;
  }

  public static IOrderedSpecificationBuilder<T> OrderBy<T>(this ISpecificationBuilder<T> specificationBuilder, Expression<Func<T, object?>> orderExpression)
      => OrderBy(specificationBuilder, orderExpression, true);

  public static IOrderedSpecificationBuilder<T> OrderBy<T>(this ISpecificationBuilder<T> specificationBuilder, Expression<Func<T, object?>> orderExpression,
      bool condition) {
    if (condition) {
      ((List<OrderExpressionInfo<T>>)specificationBuilder.Specification.OrderExpressions).Add(new OrderExpressionInfo<T>(orderExpression, OrderTypeEnum.OrderBy));
    }
    var orderedSpecificationBuilder = new OrderedSpecificationBuilder<T>(specificationBuilder.Specification, !condition);
    return orderedSpecificationBuilder;
  }

  public static IOrderedSpecificationBuilder<T> OrderByDescending<T>(this ISpecificationBuilder<T> specificationBuilder, Expression<Func<T, object?>> orderExpression)
      => OrderByDescending(specificationBuilder, orderExpression, true);

  public static IOrderedSpecificationBuilder<T> OrderByDescending<T>(this ISpecificationBuilder<T> specificationBuilder, Expression<Func<T, object?>> orderExpression,
      bool condition) {
    if (condition) {
      ((List<OrderExpressionInfo<T>>)specificationBuilder.Specification.OrderExpressions).Add(new OrderExpressionInfo<T>(orderExpression, OrderTypeEnum.OrderByDescending));
    }

    var orderedSpecificationBuilder = new OrderedSpecificationBuilder<T>(specificationBuilder.Specification, !condition);

    return orderedSpecificationBuilder;
  }

  public static IIncludableSpecificationBuilder<T, TProperty> Include<T, TProperty>(this ISpecificationBuilder<T> specificationBuilder,
      Expression<Func<T, TProperty>> includeExpression) where T : class
      => Include(specificationBuilder, includeExpression, true);

  public static IIncludableSpecificationBuilder<T, TProperty> Include<T, TProperty>(
      this ISpecificationBuilder<T> specificationBuilder,
      Expression<Func<T, TProperty>> includeExpression,
      bool condition) where T : class {
    if (condition) {
      var info = new IncludeExpressionInfo(includeExpression, typeof(T), typeof(TProperty));
      ((List<IncludeExpressionInfo>)specificationBuilder.Specification.IncludeExpressions).Add(info);
    }
    var includeBuilder = new IncludableSpecificationBuilder<T, TProperty>(specificationBuilder.Specification, !condition);
    return includeBuilder;
  }

  public static ISpecificationBuilder<T> Include<T>(this ISpecificationBuilder<T> specificationBuilder, string includeString) where T : class
      => Include(specificationBuilder, includeString, true);

  public static ISpecificationBuilder<T> Include<T>(this ISpecificationBuilder<T> specificationBuilder, string includeString,
      bool condition) where T : class {
    if (condition) {
      ((List<string>)specificationBuilder.Specification.IncludeStrings).Add(includeString);
    }
    return specificationBuilder;
  }

  public static ISpecificationBuilder<T> Search<T>(this ISpecificationBuilder<T> specificationBuilder, Expression<Func<T, string>> selector,
      string searchTerm,
      int searchGroup = 1) where T : class
      => Search(specificationBuilder, selector, searchTerm, true, searchGroup);

  public static ISpecificationBuilder<T> Search<T>(this ISpecificationBuilder<T> specificationBuilder, Expression<Func<T, string>> selector, string searchTerm, bool condition,
      int searchGroup = 1) where T : class {
    if (condition) {
      ((List<SearchExpressionInfo<T>>)specificationBuilder.Specification.SearchCriterias).Add(new SearchExpressionInfo<T>(selector, searchTerm, searchGroup));
    }
    return specificationBuilder;
  }

  public static ISpecificationBuilder<T> Take<T>(this ISpecificationBuilder<T> specificationBuilder, int take) => Take(specificationBuilder, take, true);

  public static ISpecificationBuilder<T> Take<T>(this ISpecificationBuilder<T> specificationBuilder, int take, bool condition) {
    if (condition) {
      if (specificationBuilder.Specification.Take != null) throw new Exception("DuplicateTakeException");
      specificationBuilder.Specification.Take = take;
    }
    return specificationBuilder;
  }

  public static ISpecificationBuilder<T> Skip<T>(this ISpecificationBuilder<T> specificationBuilder, int skip)
      => Skip(specificationBuilder, skip, true);

  public static ISpecificationBuilder<T> Skip<T>(this ISpecificationBuilder<T> specificationBuilder, int skip, bool condition) {
    if (condition) {
      if (specificationBuilder.Specification.Skip != null) throw new Exception("DuplicateSkipException");
      specificationBuilder.Specification.Skip = skip;
    }
    return specificationBuilder;
  }

  /// <summary>
  /// Specify a transform function to apply to the <typeparamref name="T"/> element
  /// to produce another <typeparamref name="TResult"/> element.
  /// </summary>
  public static ISpecificationBuilder<T, TResult> Select<T, TResult>(this ISpecificationBuilder<T, TResult> specificationBuilder, Expression<Func<T, TResult>> selector) {
    specificationBuilder.Specification.Selector = selector;
    return specificationBuilder;
  }

  /// <summary>
  /// Specify a transform function to apply to the result of the query
  /// and returns the same <typeparamref name="T"/> type
  /// </summary>
  public static ISpecificationBuilder<T> PostProcessingAction<T>(this ISpecificationBuilder<T> specificationBuilder, Func<IEnumerable<T>, IEnumerable<T>> predicate) {
    specificationBuilder.Specification.PostProcessingAction = predicate;
    return specificationBuilder;
  }

  /// <summary>
  /// Specify a transform function to apply to the result of the query.
  /// and returns another <typeparamref name="TResult"/> type
  /// </summary>
  public static ISpecificationBuilder<T, TResult> PostProcessingAction<T, TResult>(
      this ISpecificationBuilder<T, TResult> specificationBuilder,
      Func<IEnumerable<TResult>, IEnumerable<TResult>> predicate) {
    specificationBuilder.Specification.PostProcessingAction = predicate;

    return specificationBuilder;
  }

  /// <summary>
  /// Must be called after specifying criteria
  /// </summary>
  /// <param name="specificationName"></param>
  /// <param name="args">Any arguments used in defining the specification</param>
  public static ICacheSpecificationBuilder<T> EnableCache<T>(
      this ISpecificationBuilder<T> specificationBuilder,
      string specificationName,
      params object[] args) where T : class
      => EnableCache(specificationBuilder, specificationName, true, args);

  /// <summary>
  /// Must be called after specifying criteria
  /// </summary>
  /// <param name="specificationName"></param>
  /// <param name="args">Any arguments used in defining the specification</param>
  /// <param name="condition">If false, the caching won't be enabled.</param>
  public static ICacheSpecificationBuilder<T> EnableCache<T>(
      this ISpecificationBuilder<T> specificationBuilder,
      string specificationName,
      bool condition,
      params object[] args) where T : class {
    if (condition) {
      if (string.IsNullOrEmpty(specificationName)) {
        throw new ArgumentException($"Required input {specificationName} was null or empty.", specificationName);
      }
      specificationBuilder.Specification.CacheKey = $"{specificationName}-{string.Join("-", args)}";
      specificationBuilder.Specification.CacheEnabled = true;
    }
    var cacheBuilder = new CacheSpecificationBuilder<T>(specificationBuilder.Specification, !condition);
    return cacheBuilder;
  }

  /// <summary>
  /// If the entity instances are modified, this will not be detected
  /// by the change tracker.
  /// </summary>
  /// <param name="specificationBuilder"></param>
  public static ISpecificationBuilder<T> AsNoTracking<T>(
      this ISpecificationBuilder<T> specificationBuilder) where T : class
      => AsNoTracking(specificationBuilder, true);

  /// <summary>
  /// If the entity instances are modified, this will not be detected
  /// by the change tracker.
  /// </summary>
  /// <param name="specificationBuilder"></param>
  /// <param name="condition">If false, the setting will be discarded.</param>
  public static ISpecificationBuilder<T> AsNoTracking<T>(
      this ISpecificationBuilder<T> specificationBuilder,
      bool condition) where T : class {
    if (condition) {
      specificationBuilder.Specification.AsNoTracking = true;
    }
    return specificationBuilder;
  }

  /// <summary>
  /// The generated sql query will be split into multiple SQL queries
  /// </summary>
  /// <remarks>
  /// This feature was introduced in EF Core 5.0. It only works when using Include
  /// for more info: https://docs.microsoft.com/en-us/ef/core/querying/single-split-queries
  /// </remarks>
  /// <typeparam name="T"></typeparam>
  /// <param name="specificationBuilder"></param>
  public static ISpecificationBuilder<T> AsSplitQuery<T>(
      this ISpecificationBuilder<T> specificationBuilder) where T : class
      => AsSplitQuery(specificationBuilder, true);

  /// <summary>
  /// The generated sql query will be split into multiple SQL queries
  /// </summary>
  /// <remarks>
  /// This feature was introduced in EF Core 5.0. It only works when using Include
  /// for more info: https://docs.microsoft.com/en-us/ef/core/querying/single-split-queries
  /// </remarks>
  /// <typeparam name="T"></typeparam>
  /// <param name="specificationBuilder"></param>
  /// <param name="condition">If false, the setting will be discarded.</param>
  public static ISpecificationBuilder<T> AsSplitQuery<T>(
      this ISpecificationBuilder<T> specificationBuilder,
      bool condition) where T : class {
    if (condition) {
      specificationBuilder.Specification.AsSplitQuery = true;
    }
    return specificationBuilder;
  }

  /// <summary>
  /// The query will then keep track of returned instances
  /// (without tracking them in the normal way)
  /// and ensure no duplicates are created in the query results
  /// </summary>
  /// <remarks>
  /// for more info: https://docs.microsoft.com/en-us/ef/core/change-tracking/identity-resolution#identity-resolution-and-queries
  /// </remarks>
  /// <typeparam name="T"></typeparam>
  /// <param name="specificationBuilder"></param>
  public static ISpecificationBuilder<T> AsNoTrackingWithIdentityResolution<T>(
      this ISpecificationBuilder<T> specificationBuilder) where T : class
      => AsNoTrackingWithIdentityResolution(specificationBuilder, true);

  /// <summary>
  /// The query will then keep track of returned instances
  /// (without tracking them in the normal way)
  /// and ensure no duplicates are created in the query results
  /// </summary>
  /// <remarks>
  /// for more info: https://docs.microsoft.com/en-us/ef/core/change-tracking/identity-resolution#identity-resolution-and-queries
  /// </remarks>
  /// <typeparam name="T"></typeparam>
  /// <param name="specificationBuilder"></param>
  /// <param name="condition">If false, the setting will be discarded.</param>
  public static ISpecificationBuilder<T> AsNoTrackingWithIdentityResolution<T>(
      this ISpecificationBuilder<T> specificationBuilder,
      bool condition) where T : class {
    if (condition) {
      specificationBuilder.Specification.AsNoTrackingWithIdentityResolution = true;
    }
    return specificationBuilder;
  }

  /// <summary>
  /// The query will ignore the defined global query filters
  /// </summary>
  /// <remarks>
  /// for more info: https://docs.microsoft.com/en-us/ef/core/querying/filters
  /// </remarks>
  /// <typeparam name="T"></typeparam>
  /// <param name="specificationBuilder"></param>
  public static ISpecificationBuilder<T> IgnoreQueryFilters<T>(
      this ISpecificationBuilder<T> specificationBuilder) where T : class
      => IgnoreQueryFilters(specificationBuilder, true);

  /// <summary>
  /// The query will ignore the defined global query filters
  /// </summary>
  /// <remarks>
  /// for more info: https://docs.microsoft.com/en-us/ef/core/querying/filters
  /// </remarks>
  /// <typeparam name="T"></typeparam>
  /// <param name="specificationBuilder"></param>
  /// <param name="condition">If false, the setting will be discarded.</param>
  public static ISpecificationBuilder<T> IgnoreQueryFilters<T>(
      this ISpecificationBuilder<T> specificationBuilder,
      bool condition) where T : class {
    if (condition) {
      specificationBuilder.Specification.IgnoreQueryFilters = true;
    }
    return specificationBuilder;
  }
}