//https://stackoverflow.com/questions/30755827/getting-absolute-urls-using-asp-net-core

using System.Reflection;


namespace Microsoft.AspNetCore.Mvc.Controllers;
public static class ControllerActionDescriptorExtensions {

  public static (string? areaName, string controllerName, string actionName, string path) GetAreaControllerAction(this ControllerActionDescriptor actionDescriptor) {
    var area = actionDescriptor.AreaName();
    var controller = actionDescriptor.ControllerName;
    var action = actionDescriptor.ActionName;
    var separator = Path.AltDirectorySeparatorChar;
    return (area, controller, action, $"{separator}{area}{separator}{controller}{separator}{action}");
  }

  public static string? AreaName(this ControllerActionDescriptor actionDescriptor) => actionDescriptor.ControllerTypeInfo.GetCustomAttribute<AreaAttribute>()?.RouteValue;

  //public static (string areaName, string controllerName, string actionName, string path) MvcRoutePath(this ControllerActionDescriptor actionDescriptor) {

}