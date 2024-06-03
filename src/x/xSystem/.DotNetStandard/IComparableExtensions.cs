namespace System {
  public static class IComparableExtensions {
    public static int NullableCompare<T>(this T? x, T? y) where T : struct, IComparable, IComparable<T> => Nullable.Compare(x, y);
    public static int NullableCompareTo<T>(this T? value, T? other) where T : struct, IComparable, IComparable<T> => value.NullableCompare(other);

    /// <summary> return value between inclusive range of min and max </summary>
    public static T Clamp<T>(this T value, T min, T max) where T : struct, IComparable, IComparable<T> => value.ClampMin(min).ClampMax(max);
    /// <summary> return nullable value between inclusive range of min and max </summary>
    //public static T? Clamp<T>(this T? value, T? min, T? max) where T : struct, IComparable, IComparable<T> => value.ClampMin(min).ClampMax(max);
    public static T ClampMax<T>(this T value, T max) where T : struct, IComparable, IComparable<T> => value.IsGreaterThan(max) ? max : value;
    //public static T? ClampMax<T>(this T? value, T? max) where T : struct, IComparable, IComparable<T> => value.CompareNullable(max) > 0 ? max : value;
    public static T ClampMin<T>(this T value, T min) where T : struct, IComparable, IComparable<T> => value.IsLessThan(min) ? min : value;
    //public static T? ClampMin<T>(this T? value, T? min) where T : struct, IComparable, IComparable<T> => value.CompareNullable(min) < 0 ? min : value;

    public static bool IsBetween<T>(this T x, T min, T max, bool isMinExclusive = false, bool isMaxExclusive = false) where T : IComparable, IComparable<T>
      => (isMinExclusive ? x.IsGreaterThan(min) : x.IsGreaterThanOrEqualTo(min))
      && (isMaxExclusive ? x.IsLessThan(max) : x.IsLessThanOrEqualTo(max));

    public static bool IsEqualTo<T>(this T x, T otherValue) where T : IComparable, IComparable<T> => x.CompareTo(otherValue) == 0;
    public static bool IsGreaterThan<T>(this T x, T otherValue) where T : IComparable, IComparable<T> => x.CompareTo(otherValue) > 0;
    public static bool IsGreaterThanOrEqualTo<T>(this T x, T otherValue) where T : IComparable, IComparable<T> => x.CompareTo(otherValue) >= 0;
    public static bool IsLessThan<T>(this T value, T other) where T : IComparable, IComparable<T> => value.CompareTo(other) < 0;
    public static bool IsLessThanOrEqualTo<T>(this T x, T otherValue) where T : IComparable, IComparable<T> => x.CompareTo(otherValue) <= 0;
    
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