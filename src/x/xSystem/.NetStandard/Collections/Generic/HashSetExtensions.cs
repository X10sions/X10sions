namespace System.Collections.Generic {
  public static class HashSetExtensions {

    public static void AddOrThrow<T>(this HashSet<T> hash, T item) {
      if (!hash.Add(item)) throw new Exception($"Value already exists: {item}");
    }

  }
}