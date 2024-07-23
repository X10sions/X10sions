using System.Data;
using System.Reflection;
using System.Collections.Concurrent;

namespace RCommon;

public static class TypeExtensions {
  public static string GetGenericTypeName(this Type type) {
    var typeName = string.Empty;

    if (type.IsGenericType) {
      var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
      typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
    } else {
      typeName = type.Name;
    }

    return typeName;
  }

  private static readonly ConcurrentDictionary<Type, string> PrettyPrintCache = new ConcurrentDictionary<Type, string>();

  public static string PrettyPrint(this Type type) {
    return PrettyPrintCache.GetOrAdd(
        type,
        t => {
          try {
            return PrettyPrintRecursive(t, 0);
          } catch (Exception) {
            return t.Name;
          }
        });
  }

  private static readonly ConcurrentDictionary<Type, string> TypeCacheKeys = new ConcurrentDictionary<Type, string>();
  public static string GetCacheKey(this Type type) {
    return TypeCacheKeys.GetOrAdd(
        type,
        t => $"{t.PrettyPrint()}[hash: {t.GetHashCode()}]");
  }

  private static string PrettyPrintRecursive(Type type, int depth) {
    if (depth > 3) {
      return type.Name;
    }

    var nameParts = type.Name.Split('`');
    if (nameParts.Length == 1) {
      return nameParts[0];
    }

    var genericArguments = type.GetTypeInfo().GetGenericArguments();
    return !type.IsConstructedGenericType
        ? $"{nameParts[0]}<{new string(',', genericArguments.Length - 1)}>"
        : $"{nameParts[0]}<{string.Join(",", genericArguments.Select(t => PrettyPrintRecursive(t, depth + 1)))}>";
  }

  public static bool HasConstructorParameterOfType(this Type type, Predicate<Type> predicate) {
    return type.GetTypeInfo().GetConstructors()
        .Any(c => c.GetParameters()
            .Any(p => predicate(p.ParameterType)));
  }

  public static bool IsAssignableTo<T>(this Type type) {
    return typeof(T).GetTypeInfo().IsAssignableFrom(type);
  }
}
