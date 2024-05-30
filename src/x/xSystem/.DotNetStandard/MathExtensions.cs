namespace System;
public static class MathExtensions {

  public static T Max<T>(this T value1, T value2) => Comparer<T>.Default.Compare(value1, value2) > 0 ? value1 : value2;
  public static T Min<T>(this T value1, T value2) => Comparer<T>.Default.Compare(value1, value2) < 0 ? value1 : value2;

  //public static T Max<T>(params T[] values) => values.Max();
  //public static T Min<T>(params T[] values) => values.Min();

}