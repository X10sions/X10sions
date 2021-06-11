using System.Linq.Expressions;

namespace System {
  public static class BooleanExtensions {

    //public static void DoIfTrue(this bool b, Action action) {
    //  if (b) {
    //    action();
    //  }
    //}

    //public static void DoIfTrue<T>(this bool b, Action<T> action, T value) {
    //  if (b) {
    //    action(value);
    //  }
    //}

    //public static void DoIfFalse(this bool b, Action action) {
    //  if (!b) {
    //    action();
    //  }
    //}

    //public static void DoIfFalse<T>(this bool b, Action<T> action, T value) {
    //  if (!b) {
    //    action(value);
    //  }
    //}

    #region Throw
    public static void Throw(this bool b, string trueMessage, string falseMessage) => throw new Exception(b ? trueMessage : falseMessage);
    public static bool ThrowIfTrue(this bool b, string message) => b ? throw new Exception(message) : b;
    public static bool ThrowIfFalse(this bool b, string message) => !b ? throw new Exception(message) : b;
    public static void ThrowIfDisposed(this bool disposable, Type type) {
      if (disposable) throw new ObjectDisposedException(type.Name);
    }
    public static void ThrowIfDisposed<T>(this bool disposed) => disposed.ThrowIfDisposed(typeof(T));
    #endregion

    public static Expression<Func<T, bool>> ToPredicate<T>(this bool defaultValue) => _ => defaultValue;
  }
}