using System.Collections.Concurrent;

namespace Common.Helpers;

public static class CacheHelper<T> where T : Attribute {
  public static  ConcurrentDictionary<Type, T[]> TypeAttributes { get; } = new ConcurrentDictionary<Type, T[]>();

  public static T[] GetAttributes(Type type, bool inherit = false) {
    if (type == null) return Array.Empty<T>();
    return TypeAttributes.GetOrAdd(type, t => (T[])Attribute.GetCustomAttributes(t, typeof(T), inherit));
  }
}