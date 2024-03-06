namespace System {
  public static class IComparableExtensions {
    public static int CompareNullable<T>(this T? x, T? y) where T : struct, IComparable, IComparable<T> => Nullable.Compare(x, y);
    public static T GetValueBetween<T>(this T value, T min, T max) where T : struct, IComparable, IComparable<T> => value.CompareTo(max) > 0 ? max : value.CompareTo(min) < 0 ? min : value;
    public static T? GetValueBetween<T>(this T? value, T? min, T? max) where T : struct, IComparable, IComparable<T> => value.CompareNullable(max) > 0 ? max : value.CompareNullable(min) < 0 ? min : value;

    public static bool IsBetween<T>(this T x, T min, T max, bool isMinExclusive = false, bool isMaxExclusive = false) where T : IComparable, IComparable<T>
      => (isMinExclusive ? x.IsGreaterThan(min) : x.IsGreaterThanOrEqualTo(min))
      && (isMaxExclusive ? x.IsLessThan(max) : x.IsLessThanOrEqualTo(max));

    public static bool IsEqualTo<T>(this T x, T otherValue) where T : IComparable, IComparable<T> => x.CompareTo(otherValue) == 0;
    public static bool IsGreaterThan<T>(this T x, T otherValue) where T : IComparable, IComparable<T> => x.CompareTo(otherValue) > 0;
    public static bool IsGreaterThanOrEqualTo<T>(this T x, T otherValue) where T : IComparable, IComparable<T> => x.CompareTo(otherValue) >= 0;
    public static bool IsLessThan<T>(this T value, T other) where T : IComparable, IComparable<T> => value.CompareTo(other) < 0;
    public static bool IsLessThanOrEqualTo<T>(this T x, T otherValue) where T : IComparable, IComparable<T> => x.CompareTo(otherValue) <= 0;
    public static T LimitTo<T>(this T x, T min, T max) where T : IComparable, IComparable<T> => x.LimitToMin(min).LimitToMax(max);
    public static T LimitToMax<T>(this T x, T max) where T : IComparable, IComparable<T> => x.CompareTo(max) > 0 ? max : x;
    public static T LimitToMin<T>(this T x, T min) where T : IComparable, IComparable<T> => x.CompareTo(min) > 0 ? min : x;
    public static bool IsMoreThan<T>(this T value, T other) where T : IComparable, IComparable<T> => value.CompareTo(other) > 0;

    public static bool IsCompareToType<T>(this T value, T otherValue, T compareToType) where T : IComparable, IComparable<T> {
      switch (compareToType) {
        case CompareToType.Equals: return value.IsEqualTo(otherValue);
        case CompareToType.GreaterThan: return value.IsGreaterThan(otherValue);
        case CompareToType.GreaterThanOrEquals: return value.IsGreaterThanOrEqualTo(otherValue);
        case CompareToType.LessThan: return value.IsLessThan(otherValue);
        case CompareToType.LessThanOrEquals: return value.IsLessThanOrEqualTo(otherValue);
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