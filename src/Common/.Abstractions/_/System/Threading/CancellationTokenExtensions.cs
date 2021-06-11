namespace System.Threading {
  public static class CancellationTokenExtensions {

    public static void ThrowIfCancellationRequestedOrUserNull<T>(this CancellationToken cancellationToken, T user) => cancellationToken.ThrowIfCancellationRequestedOrNull(user, nameof(user));

    public static void ThrowIfCancellationRequestedOrRoleNull<T>(this CancellationToken cancellationToken, T role) => cancellationToken.ThrowIfCancellationRequestedOrNull(role, nameof(role));
  }

}