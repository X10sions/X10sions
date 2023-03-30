using Common.Helpers;

namespace System.Web.WebPages.Html {
  public static class WebPageContextExtensions {

    public static AssetHelper? GetAssetsHelper(this WebPageContext webPageContext) => webPageContext.GetPageData(() => new AssetHelper());

    public static T GetPageData<T>(this WebPageContext webPageContext, Func<T> valueFunction) => webPageContext.GetPageData(typeof(T).FullName ?? typeof(T).Name, valueFunction);

    public static T GetPageData<T>(this WebPageContext webPageContext, string key, Func<T> valueFunction) {
      var item = webPageContext.PageData[key];
      if (item is T tValue) return tValue;
      tValue = valueFunction();
      webPageContext.PageData[key] = tValue;
      return tValue;
    }

  }
}
