//https://stackoverflow.com/questions/30755827/getting-absolute-urls-using-asp-net-core

using Common;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.IO;
using System.Reflection;
using static System.Reflection.TypeInfoExtensions;

namespace Microsoft.AspNetCore.Mvc.Controllers {
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

    #region "DebugObject"

    public static ControllerActionDescriptorDebugObject GetDebugObject(this ControllerActionDescriptor controllerActionDescriptor) => new ControllerActionDescriptorDebugObject(controllerActionDescriptor);

    #endregion

  }

  public class ControllerActionDescriptorDebugObject : IDebugObject<ControllerActionDescriptor> {
    public ControllerActionDescriptorDebugObject(ControllerActionDescriptor controllerActionDescriptor) {
      this.controllerActionDescriptor = controllerActionDescriptor;
    }
    ControllerActionDescriptor controllerActionDescriptor;

    public string DisplayName => controllerActionDescriptor.DisplayName;
    public string? AreaName => controllerActionDescriptor.AreaName();
    public string ControllerName => controllerActionDescriptor.ControllerName;
    public string ActionName => controllerActionDescriptor.ActionName;
    public MethodInfoDebugObject MethodInfo => controllerActionDescriptor.MethodInfo.GetDebugObject();
    public TypeInfoDebugObject ControllerTypeInfo => controllerActionDescriptor.ControllerTypeInfo.GetDebugObject();
  }

}