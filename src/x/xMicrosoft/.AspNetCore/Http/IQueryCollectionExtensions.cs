using System.Collections.ObjectModel;

namespace Microsoft.AspNetCore.Http;

public static class IQueryCollectionExtensions {
  public static TResult GetValue<TResult>(this IQueryCollection query, string key, TResult defaultValue) => query.TryGetValue(key, out var value) && value is TResult t ? t : defaultValue;
  public static ReadOnlyDictionary<string, string> Items(this IQueryCollection query) {
    var items = new Dictionary<string, string>();
    foreach (var key in query.Keys) {
      items.Add(key, query[key]);
    }
    return new ReadOnlyDictionary<string, string>(items);
  }

}