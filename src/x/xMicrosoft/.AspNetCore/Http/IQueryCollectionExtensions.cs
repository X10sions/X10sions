namespace Microsoft.AspNetCore.Http {
  public static class IQueryCollectionExtensions {
    public static TResult GetValue<TResult>(this IQueryCollection query, string key, TResult defaultValue) => query.TryGetValue(key, out var value) && value is TResult t ? t : defaultValue;
  }
}