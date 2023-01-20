namespace Microsoft.AspNetCore.Http {
  public static class IFormCollectionExtensions {
    public static TResult GetValue<TResult>(this IFormCollection form, string key, TResult defaultValue) => form.TryGetValue(key, out var value) && value is TResult t ? t : defaultValue;
  }
}