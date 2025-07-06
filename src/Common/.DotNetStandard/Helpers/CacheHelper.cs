using System;
using System.Collections.Concurrent;

namespace Common.Helpers {
  public static class CacheHelper<T> {
    public static readonly ConcurrentDictionary<Type, T[]> TypeAttributes = new ConcurrentDictionary<Type, T[]>();
  }
}
