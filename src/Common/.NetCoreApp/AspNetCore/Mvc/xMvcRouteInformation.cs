using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Linq;

namespace Common.AspNetCore.Mvc {
  public class xMvcRouteInformation : IMvcRouteInformation {
    public string? AreaName { get; set; } = string.Empty;
    public string AreaPath => string.IsNullOrWhiteSpace(AreaName) ? string.Empty : Path.AltDirectorySeparatorChar + AreaName;

    public string AreaControllerActionPath => AreaPath + ControllerPath + ControllerActionPath;
    public string AreaControllerActionHandlerPath => AreaControllerActionPath + HandlerPath(string.Empty);
    public string AreaRazorPagePath => AreaPath + RazorPageViewEnginePath;
    public string AreaRazorPageHandlerPath => AreaRazorPagePath + HandlerPath(RazorPageHandlerName);

    string HandlerPath(string? handlerName) => $"?handler=On{HttpMethod}{(string.IsNullOrWhiteSpace(handlerName) ? string.Empty : handlerName)}";

    //public string ControllerMvcPath => $"{ControllerViewEnginePath}?handler=On{HttpMethod}";

    public string AttributeRouteInfoTemplate { get; set; } = string.Empty;
    public string AttributeRouteInfoTemplatePath => Path.AltDirectorySeparatorChar + AttributeRouteInfoTemplate;

    public string? ControllerActionName { get; set; }
    public string ControllerActionPath => Path.AltDirectorySeparatorChar + ControllerActionName;

    public string? ControllerName { get; set; }
    public string ControllerPath => Path.AltDirectorySeparatorChar + ControllerName;

    //public string ControllerRelativePath => $"{ControllerName}{nameof(Controller)}.{ControllerActionName}";
    //public string ControllerViewEnginePath => $"{Path.AltDirectorySeparatorChar}{ControllerName}";

    public string DisplayName { get; set; }
    public string HttpMethod { get; set; } = HttpMethods.Get;

    public bool IsController => !string.IsNullOrWhiteSpace(ControllerName);

    public bool IsRazorPage => !string.IsNullOrWhiteSpace(RazorPageViewEnginePath);

    public string MvcPath => (IsController ? AreaControllerActionHandlerPath : AreaRazorPageHandlerPath);

    //public string MvcRelativePath => IsController ? ControllerRelativePath : RazorPageRelativePath;
    //public string MvcViewEnginePath => IsController ? ControllerViewEnginePath : RazorPageViewEnginePath;
    public string? RazorPageHandlerName { get; set; }
    //public string RazorPageHandlerPath => string.IsNullOrWhiteSpace(RazorPageHandlerName) ? string.Empty : "?" + RazorPageHandlerName;
    //public string RazorPageMvcPath => $"{RazorPageViewEnginePath}?handler={RazorPageHandlerName}";
    public string? RazorPageRelativePath { get; set; }
    public string? RazorPageViewEnginePath { get; set; }
    //    public string Invocation { get; set; } = string.Empty; //RelativePath or NameContoller.Action

    //    public string RelativePath { get; set; }
    //    public string ViewEnginePath { get; set; }

    public void SetValues(ActionDescriptor actionDescriptor) {
      if (actionDescriptor is PageActionDescriptor pad) {
        SetValues(pad);
      }
      if (actionDescriptor is ControllerActionDescriptor cad) {
        SetValues(cad);
      }
      if (actionDescriptor.ActionConstraints != null && actionDescriptor.ActionConstraints.Select(t => t.GetType()).Contains(typeof(HttpMethodActionConstraint))) {
        var httpMethodAction = actionDescriptor.ActionConstraints.FirstOrDefault(a => a.GetType() == typeof(HttpMethodActionConstraint)) as HttpMethodActionConstraint;
        if (httpMethodAction != null) {
          HttpMethod = string.Join(",", httpMethodAction.HttpMethods);
        }
      }
    }

    public void SetValues(PageActionDescriptor pageActionDescriptor) {
      SetValuesCommon(pageActionDescriptor);
      // Path and Invocation of Razor Pages
      AreaName = pageActionDescriptor.AreaName;
      RazorPageViewEnginePath = pageActionDescriptor.ViewEnginePath;
      RazorPageRelativePath = pageActionDescriptor.RelativePath;
    }

    public void SetValues(ControllerActionDescriptor controllerActionDescriptor) {
      SetValuesCommon(controllerActionDescriptor);
      // Path and Invocation of Controller/Action
      AreaName = controllerActionDescriptor.AreaName();
      ControllerName = controllerActionDescriptor.ControllerName;
      ControllerActionName = controllerActionDescriptor.ActionName;
    }

    public void SetValuesCommon(ActionDescriptor actionDescriptor) {
      if (actionDescriptor.AttributeRouteInfo != null) {
        // Path of Route Attribute
        AttributeRouteInfoTemplate = actionDescriptor?.AttributeRouteInfo.Template;
      }
      DisplayName = actionDescriptor?.DisplayName;
    }

    public override string ToString() => $"RouteInformation{{Area:\"{AreaName}\", HttpMethod: \"{HttpMethod}\", MvcPath:\"{MvcPath}\", RelativePath:\"{string.Empty}\"}}";
  }
}