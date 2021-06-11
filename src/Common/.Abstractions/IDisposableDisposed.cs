using System;
using System.Threading;

namespace Common {
  public interface IDisposableDisposed : IDisposable {
    bool Disposed { get; set; }
  }

  public static class IDisposableDisposedExtensions {
    public static void Dispose(this IDisposableDisposed disposableDisposed) => disposableDisposed.Disposed = true;
    public static void ThrowIfDisposed(this IDisposableDisposed disposableDisposed) => disposableDisposed.Disposed.ThrowIfDisposed(disposableDisposed.GetType());
    public static void ThrowIfCancelledRequestOrDisposed(this IDisposableDisposed disposableDisposed, CancellationToken cancellationToken) {
      cancellationToken.ThrowIfCancellationRequested();
      disposableDisposed.ThrowIfDisposed();
    }
    public static void ThrowIfCancelledRequestOrDisposedOrNull<T>(this IDisposableDisposed disposableDisposed, T value, string valueName, CancellationToken cancellationToken) {
      cancellationToken.ThrowIfCancellationRequestedOrNull(value, valueName);
      disposableDisposed.ThrowIfDisposed();
    }

    public static void ThrowIfCancelledRequestOrDisposedOrRoleNull<T>(this IDisposableDisposed disposableDisposed, T role, CancellationToken cancellationToken) => disposableDisposed.ThrowIfCancelledRequestOrDisposedOrNull(role, nameof(role), cancellationToken);

    public static void ThrowIfCancelledRequestOrDisposedOrUserNull<T>(this IDisposableDisposed disposableDisposed, T user, CancellationToken cancellationToken) => disposableDisposed.ThrowIfCancelledRequestOrDisposedOrNull(user, nameof(user), cancellationToken);


  }
}