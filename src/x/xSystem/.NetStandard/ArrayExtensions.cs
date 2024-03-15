namespace System {
  public static class ArrayExtensions {
    public static bool Exists<T>(this T[] values, Predicate<T> match) => Array.Exists(values, match);
    public static T Find<T>(this T[] values, Predicate<T> match) => Array.Find(values, match);
    public static T GetRandom<T>(this T[] values) => values[RandomExtensions.Shared.Next(values.Length)];
  }
}