namespace Microsoft.Extensions.Caching.Memory {
  public static class IMemoryCacheExtensions {

    public static T Get<T>(this IMemoryCache memoryCache) => memoryCache.Get<T>(typeof(T).FullName);
    public static T Set<T>(this IMemoryCache memoryCache, T value) => memoryCache.Set(typeof(T).FullName, value);

    public static T GetOrCreate<T>(this IMemoryCache memoryCache, Func<ICacheEntry, T> factory) => memoryCache.GetOrCreate(typeof(T).FullName, factory);
    public static Task<T> GetOrCreateAsync<T>(this IMemoryCache memoryCache, Func<ICacheEntry, Task<T>> factory) => memoryCache.GetOrCreateAsync(typeof(T).FullName, factory);

  }
}