#nullable enable
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Mvc.RazorPages {
  public static class PageModelExtensions {

    public static ContentResult JsonNet(this PageModel controller, object model) {
      var serialized = JsonConvert.SerializeObject(model, new JsonSerializerSettings {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
      });
      return new ContentResult { Content = serialized, ContentType = "application/json" };
    }

    public static ActionResult RedirectToPageJson<TPage>(this TPage controller, string pageName) where TPage : PageModel => controller.JsonNet(new { redirect = controller.Url.Page(pageName) });

  }
}