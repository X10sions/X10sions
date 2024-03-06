using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.AspNetCore.Mvc.RazorPages;
public static class PageModelExtensions {

  public static void AddModelStateErrorIf<T, TProp>(this PageModel pageModel, T model, Expression<Func<T, bool>> predicate, Expression<Func<T, TProp>> key, string errorMessage) {
    var errorIfTrue = predicate.Compile()(model);
    pageModel.ModelState.AddModelErrorIf(errorIfTrue, key.GetMemberName(), errorMessage);
  }

  public static ContentResult AsJsonContentResult(this object model) {
    var serialized = JsonSerializer.Serialize(model, new JsonSerializerOptions {
      ReferenceHandler = ReferenceHandler.IgnoreCycles,
    });
    return new ContentResult { Content = serialized, ContentType = "application/json" };
  }

  public static ModelExpressionProvider GetModelExpressionProvider(this PageModel pageModel) => pageModel.HttpContext.RequestServices.GetService<ModelExpressionProvider>();

  public static PageResult Page(this PageModel pageModel, string modelStateErrorKey, Exception modelStateException, ModelMetadata modelStateMetadata) {
    pageModel.ModelState.AddModelError(modelStateErrorKey, modelStateException, modelStateMetadata);
    return pageModel.Page();
  }

  public static PageResult Page(this PageModel pageModel, string modelStateErrorKey, string modelStateErrorMessage) {
    pageModel.ModelState.AddModelError(modelStateErrorKey, modelStateErrorMessage);
    return pageModel.Page();
  }

  public static string PageUrl(this PageModel pageModel, string pageName, string? pageHandler = null, object? values = null, string? protocol = null, string? host = null, string? fragment = null, string? pathBase = null) {
    var value = pageModel.Url.Page(pageName, pageHandler, values, protocol, host, fragment);
    if (pathBase != null) {
      value = value.Replace(pageModel.Request.PathBase, pathBase);
    }
    return value;
  }

  public static string PageLinkUrl(this PageModel pageModel, string pageName, string? pageHandler = null, object? values = null, string? protocol = null, string? host = null, string? fragment = null, string? pathBase = null) {
    var value = pageModel.Url.PageLink(pageName, pageHandler, values, protocol, host, fragment);
    if (pathBase != null) {
      value = value.Replace(pageModel.Request.PathBase, pathBase);
    }
    return value;
  }

  //public static ActionResult RedirectToPageJson<TPage>(this TPage page, string pageName) where TPage : PageModel =>  page.JsonNet(new { redirect = page.Url.Page(pageName) });
  public static JsonResult RedirectToPageJson<TPage>(this TPage page, string pageName) where TPage : PageModel => new JsonResult(new { redirect = page.Url.Page(pageName) });

}