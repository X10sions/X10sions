using System.Linq.Expressions;

namespace Common.Data.Specifications;

public interface IOrderedSpecificationBuilder<T> : ISpecificationBuilder<T> {
  bool IsChainDiscarded { get; set; }
}

public class OrderedSpecificationBuilder<T> : IOrderedSpecificationBuilder<T> {
  public ISpecification<T> Specification { get; }
  public bool IsChainDiscarded { get; set; }

  public OrderedSpecificationBuilder(ISpecification<T> specification) : this(specification, false) { }

  public OrderedSpecificationBuilder(ISpecification<T> specification, bool isChainDiscarded) {
    Specification = specification;
    IsChainDiscarded = isChainDiscarded;
  }
}

public static class IOrderedBuilderExtensions {
  public static IOrderedSpecificationBuilder<T> ThenBy<T>(this IOrderedSpecificationBuilder<T> orderedBuilder, Expression<Func<T, object?>> orderExpression)
      => ThenBy(orderedBuilder, orderExpression, true);

  public static IOrderedSpecificationBuilder<T> ThenBy<T>(this IOrderedSpecificationBuilder<T> orderedBuilder, Expression<Func<T, object?>> orderExpression,
      bool condition) {
    if (condition && !orderedBuilder.IsChainDiscarded) {
      ((List<OrderExpressionInfo<T>>)orderedBuilder.Specification.OrderExpressions).Add(new OrderExpressionInfo<T>(orderExpression, OrderTypeEnum.ThenBy));
    } else {
      orderedBuilder.IsChainDiscarded = true;
    }
    return orderedBuilder;
  }

  public static IOrderedSpecificationBuilder<T> ThenByDescending<T>(this IOrderedSpecificationBuilder<T> orderedBuilder, Expression<Func<T, object?>> orderExpression)
      => ThenByDescending(orderedBuilder, orderExpression, true);

  public static IOrderedSpecificationBuilder<T> ThenByDescending<T>(this IOrderedSpecificationBuilder<T> orderedBuilder, Expression<Func<T, object?>> orderExpression,
      bool condition) {
    if (condition && !orderedBuilder.IsChainDiscarded) {
      ((List<OrderExpressionInfo<T>>)orderedBuilder.Specification.OrderExpressions).Add(new OrderExpressionInfo<T>(orderExpression, OrderTypeEnum.ThenByDescending));
    } else {
      orderedBuilder.IsChainDiscarded = true;
    }
    return orderedBuilder;
  }
}