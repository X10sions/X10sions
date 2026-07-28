using System.Collections.ObjectModel;

namespace Microsoft.AspNetCore.Http {
  public static class IFormCollectionExtensions {
    public static TResult GetValue<TResult>(this IFormCollection form, string key, TResult defaultValue) => form.TryGetValue(key, out var value) && value is TResult t ? t : defaultValue;
    public static ReadOnlyDictionary<string, string> Items(this IFormCollection form) {
      var items = new Dictionary<string, string>();
      foreach (var key in form.Keys) {
        items.Add(key, form[key]);
      }
      return new ReadOnlyDictionary<string, string>(items);
    }
  }
}