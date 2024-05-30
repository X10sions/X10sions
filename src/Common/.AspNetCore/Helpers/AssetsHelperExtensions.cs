using Microsoft.AspNetCore.Html;

namespace Common.Helpers;
public static class AssetsHelperExtensions {
  public static HtmlString RenderStylesheets(this IAssetHelper assetHelper) => new HtmlString(assetHelper.GetStylesheetHtmlTagStrings());
  public static HtmlString RenderScripts(this IAssetHelper assetHelper) => new HtmlString(assetHelper.GetScriptHtmlTagStrings());

}
