using Common.Helpers;

namespace Microsoft.AspNetCore.Http {
  public static class HttpContextExtensions {

    public static AssetHelper GetAssetsHelper(this HttpContext httpContext) => httpContext.GetItem(nameof(AssetHelper), () => new AssetHelper());

    public static T GetItem<T>(this HttpContext httpContext, Func<T> valueFunction) => httpContext.GetItem(typeof(T).FullName ?? typeof(T).Name, valueFunction);

    public static T GetItem<T>(this HttpContext httpContext, string key, Func<T> valueFunction) {
      var item = httpContext.Items[key];
      if (item is T tValue) return tValue;
      tValue = valueFunction();
      httpContext.Items[key] = tValue;
      return tValue;
    }

  }
}
