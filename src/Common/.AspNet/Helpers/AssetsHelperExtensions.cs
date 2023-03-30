using System.Web;

namespace Common.Helpers {
  public static class AssetsHelperExtensions {

    public static IHtmlString RenderStylesheets(this IAssetHelper assetHelper) => new HtmlString(assetHelper.GetStylesheetHtmlTagStrings());
    public static IHtmlString RenderScripts(this IAssetHelper assetHelper) => new HtmlString(assetHelper.GetScriptHtmlTagStrings());


  }
}
