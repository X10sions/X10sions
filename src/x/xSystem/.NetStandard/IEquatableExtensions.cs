namespace System;
public static class IEquatableExtensions {

  public static TKey? ConvertIdFromString<TKey>(this string id) where TKey : IEquatable<TKey>
    => id == null ? default : (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(id);

  public static string? ConvertIdToString<TKey>(this TKey id) where TKey : IEquatable<TKey> => Equals(id, default) ? null : id.ToString();

}
