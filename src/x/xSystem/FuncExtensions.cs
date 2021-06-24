using System.Linq.Expressions;

namespace System {
  public static class FuncExtensions {
    public static Expression<Func<T, TOut>> AsExpr<T, TOut>(this Func<T, TOut> func) => x => func(x);
  }
}