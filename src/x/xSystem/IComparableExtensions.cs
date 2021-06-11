namespace System {
  public static class IComparableExtensions {

    public static int CompareNullable<T>(this T? x, T? y) where T : struct, IComparable<T> => !x.HasValue && !y.HasValue ? 0 : x.HasValue && !y.HasValue ? 1 : y.HasValue && !x.HasValue ? -1 : x.Value.CompareTo(y.Value);

    public static T GetValueBetween<T>(this T value, T min, T max) where T : struct, IComparable<T> => value.CompareTo(max) > 0 ? max : value.CompareTo(min) < 0 ? min : value;
    public static T? GetValueBetween<T>(this T? value, T? min, T? max) where T : struct, IComparable<T> => value.CompareNullable(max) > 0 ? max : value.CompareNullable(min) < 0 ? min : value;

    //[LinqToDB.Sql.Expression("{0} BETWEEN {1} AND {2}",PreferServerSide = true)]
    public static bool IsBetween(this IComparable x, IComparable min, IComparable max) => x.IsGreaterThanOrEquals(min) && x.IsLessThanOrEquals(max);

    public static bool IsBetween<T>(this T x, T min, T max, bool isMinExclusive = false, bool isMaxExclusive = false) where T : IComparable<T>
      => !isMinExclusive && !isMaxExclusive ? x.CompareTo(min) >= 0 && x.CompareTo(max) <= 0
      : isMinExclusive && isMaxExclusive ? x.CompareTo(min) > 0 && x.CompareTo(max) < 0
      : isMinExclusive ? x.CompareTo(min) > 0 && x.CompareTo(max) <= 0
      : x.CompareTo(min) >= 0 && x.CompareTo(max) < 0;

    public static bool IsEqualTo(this IComparable x, IComparable otherValue) => x.CompareTo(otherValue) == 0;
    public static bool IsGreaterThan(this IComparable x, IComparable otherValue) => x.CompareTo(otherValue) > 0;
    public static bool IsGreaterThanOrEquals(this IComparable x, IComparable otherValue) => x.CompareTo(otherValue) >= 0;
    public static bool IsLessThan(this IComparable x, IComparable otherValue) => x.CompareTo(otherValue) < 0;
    public static bool IsLessThanOrEquals(this IComparable x, IComparable otherValue) => x.CompareTo(otherValue) <= 0;

    public static T LimitTo<T>(this T x, T min, T max) where T : IComparable => x.LimitToMin(min).LimitToMax(max);

    public static T LimitToMax<T>(this T x, T max) where T : IComparable => x.CompareTo(max) > 0 ? max : x;
    public static T LimitToMin<T>(this T x, T min) where T : IComparable => x.CompareTo(min) > 0 ? min : x;

    public static bool IsCompareToType(this IComparable value, IComparable otherValue, CompareToType compareToType) {
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