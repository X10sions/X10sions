using System.Collections.Concurrent;
using System.Reflection;

namespace System;
  public static class AttributeExtensions {

    private static readonly ConcurrentDictionary<(Type, string), Attribute?> _cache = new();

    public static TAttribute? GetCachedAttribute<TAttribute>(this Type type, string name) where TAttribute : Attribute {
      if (name is null) return null;
      var key = (type, name);
      if (_cache.TryGetValue(key, out var cached) && cached is TAttribute typedCached)
        return typedCached;
      var field = type.GetField(name);
      var attribute = field?.GetCustomAttribute<TAttribute>(inherit: false);
      _cache[key] = attribute;
      return attribute;
    }

  }
