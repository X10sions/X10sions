using System.Linq.Expressions;

namespace System {
  public static class FuncExtensions {
    [Obsolete($"Use {nameof(AsExpression)} instead.")]public static Expression<Func<T, TOut>> AsExpr<T, TOut>(this Func<T, TOut> func) => x => func(x);

    public static Expression<Func<T, TValue>> AsExpression<T, TValue>(this Func<T, TValue> func) => x => func(x);

  }
}