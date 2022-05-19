using System.Linq.Expressions;
using System.Reflection;

namespace System {
  public static class FuncExtensions {
    [Obsolete($"Use {nameof(AsExpression)} instead.")]public static Expression<Func<T, TOut>> AsExpr<T, TOut>(this Func<T, TOut> func) => x => func(x);

    public static Expression<Func<T, TValue>> AsExpression<T, TValue>(this Func<T, TValue> func) => x => func(x);

    #region Helper methods to obtain MethodInfo in a safe way
    public static MethodInfo GetMethodInfo<T1, T2>(this Func<T1, T2> f, T1 unused1) => f.Method;
    public static MethodInfo GetMethodInfo<T1, T2, T3>(this Func<T1, T2, T3> f, T1 unused1, T2 unused2) => f.Method;
    public static MethodInfo GetMethodInfo<T1, T2, T3, T4>(this Func<T1, T2, T3, T4> f, T1 unused1, T2 unused2, T3 unused3) => f.Method;
    public static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5> f, T1 unused1, T2 unused2, T3 unused3, T4 unused4) => f.Method;
    public static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5, T6>(this Func<T1, T2, T3, T4, T5, T6> f, T1 unused1, T2 unused2, T3 unused3, T4 unused4, T5 unused5) => f.Method;
    public static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, T2, T3, T4, T5, T6, T7> f, T1 unused1, T2 unused2, T3 unused3, T4 unused4, T5 unused5, T6 unused6) => f.Method;
    #endregion

  }
}