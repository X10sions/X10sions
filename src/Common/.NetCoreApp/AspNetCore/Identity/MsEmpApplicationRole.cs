using Microsoft.AspNetCore.Identity;

namespace Common.NetCoreApp.AspNetCore.Identity;

public class MsEmpApplicationRole : IdentityRole<int> {
  public string Access => $"{HttpMethod}/{MvcArea}/{MvcController}/{MvcControllerAction}/{Path}/{MvcRazorPage}/{MvcRazorPageHandler}";

  public string HttpMethod { get; set; }
  public string MvcArea { get; set; }
  public string MvcController { get; set; }
  public string MvcControllerAction { get; set; }
  public string Path { get; set; }
  public string MvcRazorPage { get; set; }
  public string MvcRazorPageHandler { get; set; }

}