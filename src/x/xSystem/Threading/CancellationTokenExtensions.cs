namespace System.Threading {
  public static class CancellationTokenExtensions {

    public static void ThrowIfCancellationRequestedOrNull<T>(this CancellationToken cancellationToken, T value, string parameterName) {
      cancellationToken.ThrowIfCancellationRequested();
      Check.NotNull(value, parameterName);
    }

  }
}
