using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.AspNetCore.Mvc.RazorPages;
public static class PageModelExtensions {

  public static ContentResult AsJsonContentResult(this object model) {
    var serialized = JsonSerializer.Serialize(model, new JsonSerializerOptions {
      ReferenceHandler = ReferenceHandler.IgnoreCycles,
    });
    return new ContentResult { Content = serialized, ContentType = "application/json" };
  }

  //public static ActionResult RedirectToPageJson<TPage>(this TPage page, string pageName) where TPage : PageModel =>  page.JsonNet(new { redirect = page.Url.Page(pageName) });
  public static JsonResult RedirectToPageJson<TPage>(this TPage page, string pageName) where TPage : PageModel => new JsonResult(new { redirect = page.Url.Page(pageName) });

}