namespace System {
  public static class Throw {
    public static bool IfNull<T>(T value, string message) => (value == null).ThrowIfTrue(message);

  }
}