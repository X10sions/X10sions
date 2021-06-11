using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Caching.Distributed {
  public static class IDistributedCacheExtensions {

    public static T GetUsingBytes<T>(this IDistributedCache distributedCache) => distributedCache.GetUsingBytes<T>(typeof(T).FullName);
    public static T GetUsingBytes<T>(this IDistributedCache distributedCache, string key) => distributedCache.Get(key).Get<T>();
    public static async Task<T> GetUsingBytesAsync<T>(this IDistributedCache distributedCache, CancellationToken token = default(CancellationToken)) => await distributedCache.GetUsingBytesAsync<T>(typeof(T).FullName, token);
    public static async Task<T> GetUsingBytesAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken token = default(CancellationToken)) => (await distributedCache.GetAsync(key, token)).Get<T>();

    public static T GetUsingJson<T>(this IDistributedCache distributedCache) => distributedCache.GetUsingJson<T>(typeof(T).FullName);
    public static T GetUsingJson<T>(this IDistributedCache distributedCache, string key) => JsonConvert.DeserializeObject<T>(distributedCache.GetString(key));
    public static async Task<T> GetUsingJsonAsync<T>(this IDistributedCache distributedCache, CancellationToken token = default(CancellationToken)) => await distributedCache.GetUsingJsonAsync<T>(typeof(T).FullName, token);
    public static async Task<T> GetUsingJsonAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken token = default(CancellationToken)) => JsonConvert.DeserializeObject<T>(await distributedCache.GetStringAsync(key, token));

    public static void Refresh<T>(this IDistributedCache distributedCache) => distributedCache.Refresh(typeof(T).FullName);
    public static Task RefreshAsync<T>(this IDistributedCache distributedCache, CancellationToken token = default(CancellationToken)) => distributedCache.RefreshAsync(typeof(T).FullName, token);
    public static void Remove<T>(this IDistributedCache distributedCache) => distributedCache.Remove(typeof(T).FullName);
    public static Task RemoveAsync<T>(this IDistributedCache distributedCache, CancellationToken token = default(CancellationToken)) => distributedCache.RemoveAsync(typeof(T).FullName, token);

    public static void SetUsingBytes<T>(this IDistributedCache distributedCache, T value, DistributedCacheEntryOptions options) => distributedCache.SetUsingBytes(typeof(T).FullName, value, options);
    public static void SetUsingBytes<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options) => distributedCache.Set(key, value.ToByteArray(), options);
    public static async Task SetUsingBytesAsync<T>(this IDistributedCache distributedCache, T value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken)) => await distributedCache.SetUsingBytesAsync(typeof(T).FullName, value, options, token);
    public static async Task SetUsingBytesAsync<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken)) => await distributedCache.SetAsync(key, value.ToByteArray(), options, token);

    public static void SetUsingJson<T>(this IDistributedCache distributedCache, T value, DistributedCacheEntryOptions options) => distributedCache.SetUsingJson(typeof(T).FullName, value, options);
    public static void SetUsingJson<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options) => distributedCache.SetString(key, JsonConvert.SerializeObject(value), options);
    public static async Task SetUsingJsonAsync<T>(this IDistributedCache distributedCache, T value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken)) => await distributedCache.SetUsingJsonAsync(typeof(T).FullName, value, options, token);
    public static async Task SetUsingJsonAsync<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken)) => await distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value), options, token);

  }
}