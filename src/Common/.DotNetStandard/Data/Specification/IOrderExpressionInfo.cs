using System.Linq.Expressions;

namespace Common.Data.Specification;

public interface IOrderExpressionInfo<T> {
  Expression<Func<T, object?>> KeySelector { get; }
  OrderTypeEnum OrderType { get; }
  Func<T, object?> KeySelectorFunc { get; }
}

public class OrderExpressionInfo<T> : IOrderExpressionInfo<T> {
  private readonly Lazy<Func<T, object?>> keySelectorFunc;

  public Expression<Func<T, object?>> KeySelector { get; }
  public OrderTypeEnum OrderType { get; }
  public Func<T, object?> KeySelectorFunc => keySelectorFunc.Value;

  public OrderExpressionInfo(Expression<Func<T, object?>> keySelector, OrderTypeEnum orderType) {
    if (keySelector == null) {
      throw new ArgumentNullException(nameof(keySelector));
    }
    KeySelector = keySelector;
    OrderType = orderType;
    keySelectorFunc = new Lazy<Func<T, object>>(new Func<Func<T, object>>(KeySelector.Compile));
  }
}

public enum OrderTypeEnum {
  OrderBy = 1,
  OrderByDescending,
  ThenBy,
  ThenByDescending
}
