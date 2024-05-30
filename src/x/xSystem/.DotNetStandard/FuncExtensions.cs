using System.Linq.Expressions;
using System.Reflection;

namespace System {
  public static class FuncExtensions {
    [Obsolete($"Use {nameof(AsExpression)} instead.")]public static Expression<Func<T, TOut>> AsExpr<T, TOut>(this Func<T, TOut> func) => x => func(x);

    public static Expression<Func<T, TValue>> AsExpression<T, TValue>(this Func<T, TValue> func) => x => func(x);
    public static Expression<Func<T1, T2, bool>> AsExpressionFunc<T1, T2>(this Func<T1, T2, bool> func) => (t1, t2) => func(t1, t2);

    #region Helper methods to obtain MethodInfo in a safe way
    public static MethodInfo GetMethodInfo<T1, T2>(this Func<T1, T2> f, T1 unused1) => f.Method;
    public static MethodInfo GetMethodInfo<T1, T2, T3>(this Func<T1, T2, T3> f, T1 unused1, T2 unused2) => f.Method;
    public static MethodInfo GetMethodInfo<T1, T2, T3, T4>(this Func<T1, T2, T3, T4> f, T1 unused1, T2 unused2, T3 unused3) => f.Method;
    public static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5> f, T1 unused1, T2 unused2, T3 unused3, T4 unused4) => f.Method;
    public static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5, T6>(this Func<T1, T2, T3, T4, T5, T6> f, T1 unused1, T2 unused2, T3 unused3, T4 unused4, T5 unused5) => f.Method;
    public static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, T2, T3, T4, T5, T6, T7> f, T1 unused1, T2 unused2, T3 unused3, T4 unused4, T5 unused5, T6 unused6) => f.Method;
    #endregion

    public static Task<T> GetTask<T>(this Func<T> func) {
      var task = new Task<T>(func);
      task.Start();
      return task;
    }

    public static Task<T> GetTask<T>(this Func<T> func, TaskCreationOptions options) {
      var task = new Task<T>(func, options);
      task.Start();
      return task;
    }

    public static Func<T2, T1, bool> SwapParameters<T1, T2>(this Func<T1, T2, bool> func) => (t1, t2) => func(t2, t1);
    public static Expression<Func<T2, T1, bool>> SwapParametersAsExpressionFunc<T1, T2>(this Func<T1, T2, bool> func) => (t1, t2) => func(t2, t1);

    public static TReturn TryCatch<TReturn>(this Func<TReturn> action, Action<Exception> onError) {
      TReturn result = default!;
      try {
        result = action();
      } catch (Exception ex) {
        onError(ex);
      }
      return result;
    }

  }
}