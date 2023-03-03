namespace System.Collections.Generic {
  public static class ICollectionExtensions {

    public static ICollection<T> AddIf<T>(this ICollection<T> collection, bool predicate, T item) {
      if(predicate)
        collection.Add(item);
      return collection;
    }

    public static ICollection<T> AddIf<T>(this ICollection<T> collection, Func<bool> predicate, T item) => collection.AddIf<T>(predicate.Invoke(), item);

    public static ICollection<T> AddIf<T>(this ICollection<T> collection, Func<T, bool> predicate, T item) => collection.AddIf<T>(predicate.Invoke(item), item);

  }
}