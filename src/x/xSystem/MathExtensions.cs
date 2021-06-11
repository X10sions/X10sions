using System.Collections.Generic;
//using System.Linq;

namespace System {
  public static class MathExtensions {

    //public static DateTime Min(this DateTime val1, DateTime val2) => new DateTime(Math.Min(val1.Ticks, val2.Ticks));
    //public static TimeSpan Min(this TimeSpan val1, TimeSpan val2) => new TimeSpan(Math.Min(val1.Ticks, val2.Ticks));

    public static T Max<T>(this T value1, T value2) => Comparer<T>.Default.Compare(value1, value2) > 0 ? value1 : value2;
    public static T Min<T>(this T value1, T value2) => Comparer<T>.Default.Compare(value1, value2) < 0 ? value1 : value2;

    //public static T Max<T>(params T[] values) => values.Max();
    //public static T Min<T>(params T[] values) => values.Min();

  }
}