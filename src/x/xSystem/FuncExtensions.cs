using System.Linq.Expressions;

namespace System {
  public static class FuncExtensions {
    //[Obsolete("Use AsExpr")] public static Expression<Func<T, TOut>> ConvertToExpr<T, TOut>(this Func<T, TOut> func) => func.AsExpr();
    public static Expression<Func<T, TOut>> AsExpr<T, TOut>(this Func<T, TOut> func) => x => func(x);
  }
}