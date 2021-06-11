using System.IO;

namespace Common.AspNetCore.Mvc {

  public interface IMvcRouteInformation {
    string? AreaName { get; set; }
    string? ControllerName { get; set; }
    string? ControllerActionName { get; set; }
    string? RazorPageRelativePath { get; set; }
    string? RazorPageViewEnginePath { get; set; }
    string? RazorPageHandlerName { get; set; }
  }

  public static class IMvcRouteInformationExtensions {
    public static string AreaPartPath(this IMvcRouteInformation info) => string.IsNullOrWhiteSpace(info.AreaName) ? string.Empty : Path.AltDirectorySeparatorChar + info.AreaName;
    public static string ControllerPartPath(this IMvcRouteInformation info) => Path.AltDirectorySeparatorChar + info.ControllerName;
    public static string ActionPartPath(this IMvcRouteInformation info) => Path.AltDirectorySeparatorChar + info.ControllerActionName;
    public static string ControllerActionPartPath(this IMvcRouteInformation info) => info.ControllerPartPath() + info.ActionPartPath();
    public static bool IsMvcRazorPage(this IMvcRouteInformation info) => info.RazorPageViewEnginePath != null;
    public static bool IsMvcControllerView(this IMvcRouteInformation info) => info.ControllerActionName != null;
    public static string RazorPagePartPath(this IMvcRouteInformation info) => info.ControllerActionName + Path.AltDirectorySeparatorChar;
    public static string RazorPageHandlerPartPath(this IMvcRouteInformation info) => "?" + info.RazorPageHandlerName;
    public static string MvcRoutePath(this IMvcRouteInformation info, bool includeArea = true, bool includePageHandler = true) => includeArea ? info.AreaPartPath() : string.Empty
          //+ aspNetCoreRoute.IsMvcRazorPage() ? aspNetCoreRoute.RazorPagePath() : aspNetCoreRoute.ControllerActionPartPath()
          //+ includePageHandler ? aspNetCoreRoute.RazorPageHandlerPartPath() : string.Empty
          ;
  }

}
