namespace System {
  public static class TExtensions {
    public static bool EqualsOrLessThan<T>(this T value, T other) where T : IComparable, IComparable<T> => value.CompareTo(other) <= 0;
    public static bool EqualsOrMoreThan<T>(this T value, T other) where T : IComparable, IComparable<T> => value.CompareTo(other) >= 0;

    public static bool IsLessThan<T>(this T value, T other) where T : IComparable, IComparable<T> => value.CompareTo(other) < 0;
    public static bool IsMoreThan<T>(this T value, T other) where T : IComparable, IComparable<T> => value.CompareTo(other) > 0;

    public static TAttr GetAttribute<T, TAttr>(this T source) where TAttr : Attribute {
      var fi = source.GetType().GetField(source.ToString());
      var attributes = (TAttr[])fi.GetCustomAttributes(typeof(TAttr), false);
      return (attributes != null && attributes.Length > 0) ? attributes[0] : null;
    }

  }
}
