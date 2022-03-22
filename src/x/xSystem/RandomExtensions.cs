namespace System {
  public static class RandomExtensions {
    [Obsolete("Not introdcues until .NET 6")] public static Random Shared { get; } = new Random();

  }
}