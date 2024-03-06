using System.Text.Json;

namespace Microsoft.Extensions.Caching.Distributed {
  public static class IDistributedCacheExtensions {
    public static async Task<T> GetAsync<T>(this IDistributedCache distributedCache, string cacheKey, CancellationToken token = default) {
      Throw.IfNull(distributedCache, nameof(distributedCache));
      Throw.IfNull(cacheKey, nameof(cacheKey));
      byte[] utf8Bytes = await distributedCache.GetAsync(cacheKey, token).ConfigureAwait(false);
      if (utf8Bytes != null) {
        return JsonSerializer.Deserialize<T>(utf8Bytes);
      }
      return default;
    }

    public static async Task RemoveAsync(this IDistributedCache distributedCache, string cacheKey, CancellationToken token = default) {
      Throw.IfNull(distributedCache, nameof(distributedCache));
      Throw.IfNull(cacheKey, nameof(cacheKey));
      await distributedCache.RemoveAsync(cacheKey, token).ConfigureAwait(false);
    }

    public static async Task SetAsync<T>(this IDistributedCache distributedCache, string cacheKey, T obj, int cacheExpirationInMinutes = 30, CancellationToken token = default) {
      Throw.IfNull(distributedCache, nameof(distributedCache));
      Throw.IfNull(cacheKey, nameof(cacheKey));
      Throw.IfNull(obj, nameof(obj));
      DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
       .SetSlidingExpiration(TimeSpan.FromMinutes(cacheExpirationInMinutes));
      byte[] utf8Bytes = JsonSerializer.SerializeToUtf8Bytes<T>(obj);
      await distributedCache.SetAsync(cacheKey, utf8Bytes, options, token).ConfigureAwait(false);
    }
  }
}
