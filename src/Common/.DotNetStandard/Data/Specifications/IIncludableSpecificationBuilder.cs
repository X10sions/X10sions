using System.Linq.Expressions;

namespace Common.Data.Specifications;

public interface IIncludableSpecificationBuilder<T, out TProperty> : ISpecificationBuilder<T> where T : class {
  bool IsChainDiscarded { get; set; }
}

public class IncludableSpecificationBuilder<T, TProperty> : IIncludableSpecificationBuilder<T, TProperty> where T : class {
  public ISpecification<T> Specification { get; }
  public bool IsChainDiscarded { get; set; }

  public IncludableSpecificationBuilder(ISpecification<T> specification) : this(specification, false) { }

  public IncludableSpecificationBuilder(ISpecification<T> specification, bool isChainDiscarded) {
    Specification = specification;
    IsChainDiscarded = isChainDiscarded;
  }
}

public static class IIncludableBuilderExtensions {
  public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
      this IIncludableSpecificationBuilder<TEntity, TPreviousProperty> previousBuilder,
      Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression)
      where TEntity : class
      => ThenInclude(previousBuilder, thenIncludeExpression, true);

  public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
      this IIncludableSpecificationBuilder<TEntity, TPreviousProperty> previousBuilder,
      Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression,
      bool condition)
      where TEntity : class {
    if (condition && !previousBuilder.IsChainDiscarded) {
      var info = new IncludeExpressionInfo(thenIncludeExpression, typeof(TEntity), typeof(TProperty), typeof(TPreviousProperty));

      ((List<IncludeExpressionInfo>)previousBuilder.Specification.IncludeExpressions).Add(info);
    }
    var includeBuilder = new IncludableSpecificationBuilder<TEntity, TProperty>(previousBuilder.Specification, !condition || previousBuilder.IsChainDiscarded);

    return includeBuilder;
  }

  public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
      this IIncludableSpecificationBuilder<TEntity, IEnumerable<TPreviousProperty>> previousBuilder,
      Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression)
      where TEntity : class
      => ThenInclude(previousBuilder, thenIncludeExpression, true);

  public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
      this IIncludableSpecificationBuilder<TEntity, IEnumerable<TPreviousProperty>> previousBuilder,
      Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression,
      bool condition)
      where TEntity : class {
    if (condition && !previousBuilder.IsChainDiscarded) {
      var info = new IncludeExpressionInfo(thenIncludeExpression, typeof(TEntity), typeof(TProperty), typeof(IEnumerable<TPreviousProperty>));

      ((List<IncludeExpressionInfo>)previousBuilder.Specification.IncludeExpressions).Add(info);
    }
    var includeBuilder = new IncludableSpecificationBuilder<TEntity, TProperty>(previousBuilder.Specification, !condition || previousBuilder.IsChainDiscarded);
    return includeBuilder;
  }
}