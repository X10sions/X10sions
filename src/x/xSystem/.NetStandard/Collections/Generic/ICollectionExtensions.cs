namespace System.Collections.Generic {
  public static class ICollectionExtensions {

    public static ICollection<T> AddIf<T>(this ICollection<T> collection, bool predicate, T item) {
      if (predicate)
        collection.Add(item);
      return collection;
    }

    public static ICollection<T> AddIf<T>(this ICollection<T> collection, Func<bool> predicate, T item) => collection.AddIf(predicate.Invoke(), item);

    public static ICollection<T> AddIf<T>(this ICollection<T> collection, Func<T, bool> predicate, T item) => collection.AddIf(predicate.Invoke(item), item);

    [Obsolete] public static bool IsEmptyOrNull<T>(this ICollection<T> collection, Func<T, bool> predicate, T item) => collection is null || collection.Count == 0;

  }
}