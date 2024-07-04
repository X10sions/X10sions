using Common.NetCoreApp.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Common.AspNetCore.Mvc.Razor.TagHelpers {

  [HtmlTargetElement("secure-content")]
  public class SecureContentTagHelper : TagHelper {
    public SecureContentTagHelper(MsEmpDbContext dbContext) {
      _dbContext = dbContext;
    }

    readonly MsEmpDbContext _dbContext;

    [HtmlAttributeName("asp-area")] public string Area { get; set; }
    [HtmlAttributeName("asp-controller")] public string Controller { get; set; }
    [HtmlAttributeName("asp-action")] public string Action { get; set; }
    [HtmlAttributeName("asp-page")] public string Page { get; set; }
    [HtmlAttributeName("asp-page-handler")] public string PageHandler { get; set; }

    [ViewContext, HtmlAttributeNotBound] public ViewContext ViewContext { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
      output.TagName = null;
      var user = ViewContext.HttpContext.User;
      if (!user.Identity.IsAuthenticated) {
        output.SuppressOutput();
        return;
      }
      var roles = await (
          from usr in _dbContext.Users
          join userRole in _dbContext.UserRoles on usr.Id equals userRole.UserId
          join role in _dbContext.Roles on userRole.RoleId equals role.Id
          where usr.UserName == user.Identity.Name
          select role
      ).ToListAsync();
      var actionId = $"{ViewContext.HttpContext.Request.Method}:{Area}:{Controller}:{Action}:{Page}:{PageHandler}";
      foreach (var role in roles) {
        if (role.Access == null) continue;
        var accessList = JsonConvert.DeserializeObject<IEnumerable<xMvcControllerInfo>>(role.Access);
        if (accessList.SelectMany(c => c.Actions).Any(a => a.Id == actionId)) return;
      }
      output.SuppressOutput();
    }

  }
}
