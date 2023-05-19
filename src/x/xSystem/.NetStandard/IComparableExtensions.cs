namespace System {
  public static class IComparableExtensions {

    public static int CompareNullable<T>(this T? x, T? y) where T : struct, IComparable<T> => Nullable.Compare(x, y);

    public static T GetValueBetween<T>(this T value, T min, T max) where T : struct, IComparable<T> => value.CompareTo(max) > 0 ? max : value.CompareTo(min) < 0 ? min : value;
    public static T? GetValueBetween<T>(this T? value, T? min, T? max) where T : struct, IComparable<T> => value.CompareNullable(max) > 0 ? max : value.CompareNullable(min) < 0 ? min : value;

    //[LinqToDB.Sql.Expression("{0} BETWEEN {1} AND {2}",PreferServerSide = true)]
    public static bool IsBetween<T>(this T x, T min, T max) where T : IComparable => x.IsGreaterThanOrEquals(min) && x.IsLessThanOrEquals(max);
    //public static bool IsBetween<T>(this T item, T start, T end) where T : IComparable, IComparable<T> => Comparer<T>.Default.Compare(item, start) >= 0 && Comparer<T>.Default.Compare(item, end) <= 0;

    public static bool IsBetween<T>(this T x, T min, T max, bool isMinExclusive = false, bool isMaxExclusive = false) where T : IComparable//<T>
      => !isMinExclusive && !isMaxExclusive ? x.IsBetween(min, max)
      : isMinExclusive && isMaxExclusive ? x.IsGreaterThan(min) && x.IsLessThanOrEquals(max)
      : isMinExclusive ? x.IsGreaterThan(min) && x.IsLessThanOrEquals(max)
      : x.IsGreaterThanOrEquals(min) && x.IsLessThan(max);

    public static bool IsEqualTo<T>(this T x, T otherValue) where T : IComparable => x.CompareTo(otherValue) == 0;
    public static bool IsGreaterThan<T>(this T x, T otherValue) where T : IComparable => x.CompareTo(otherValue) > 0;
    public static bool IsGreaterThanOrEquals<T>(this T x, T otherValue) where T : IComparable => x.CompareTo(otherValue) >= 0;
    public static bool IsLessThan<T>(this T x, T otherValue) where T : IComparable => x.CompareTo(otherValue) < 0;
    public static bool IsLessThanOrEquals<T>(this T x, T otherValue) where T : IComparable => x.CompareTo(otherValue) <= 0;

    public static T LimitTo<T>(this T x, T min, T max) where T : IComparable => x.LimitToMin(min).LimitToMax(max);

    public static T LimitToMax<T>(this T x, T max) where T : IComparable => x.CompareTo(max) > 0 ? max : x;
    public static T LimitToMin<T>(this T x, T min) where T : IComparable => x.CompareTo(min) > 0 ? min : x;

    public static bool IsCompareToType<T>(this IComparable value, IComparable otherValue, CompareToType compareToType) {
      switch (compareToType) {
        case CompareToType.Equals: return value.IsEqualTo(otherValue);
        case CompareToType.GreaterThan: return value.IsGreaterThan(otherValue);
        case CompareToType.GreaterThanOrEquals: return value.IsGreaterThanOrEquals(otherValue);
        case CompareToType.LessThan: return value.IsLessThan(otherValue);
        case CompareToType.LessThanOrEquals: return value.IsLessThanOrEquals(otherValue);
      }
      return false;
    }

    public enum CompareToType {
      Equals,
      LessThan,
      LessThanOrEquals,
      GreaterThan,
      GreaterThanOrEquals
    }

  }
}