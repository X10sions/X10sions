namespace System {
  public static class ArrayExtensions {
    public static T GetRandom<T>(this T[] values) => values[RandomExtensions.Shared.Next(values.Length)];
  }
}