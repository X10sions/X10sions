using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.AspNetCore.Mvc.Abstractions {

  public static class ActionDescriptorExtensions {

    public static string AreaName(this ActionDescriptor actionDescriptor)
      => actionDescriptor.CompiledPageActionDescriptor()?.AreaName
      ?? actionDescriptor.PageActionDescriptor()?.AreaName
      ?? actionDescriptor.ControllerActionDescriptor()?.AreaName();

    public static ControllerActionDescriptor ControllerActionDescriptor(this ActionDescriptor actionDescriptor) => (ControllerActionDescriptor)actionDescriptor;
    //public string ControllerName { get; set; }
    //public virtual string ActionName { get; set; }
    //public MethodInfo MethodInfo { get; set; }
    //public TypeInfo ControllerTypeInfo { get; set; }
    //public override string DisplayName { get; set; }

    public static CompiledPageActionDescriptor CompiledPageActionDescriptor(this ActionDescriptor actionDescriptor) => (CompiledPageActionDescriptor)actionDescriptor;
    //public IList<HandlerMethodDescriptor> HandlerMethods { get; set; }
    //public TypeInfo HandlerTypeInfo { get; set; }
    //public TypeInfo DeclaredModelTypeInfo { get; set; }
    //public TypeInfo ModelTypeInfo { get; set; }
    //public TypeInfo PageTypeInfo { get; set; }

    //public string RelativePath { get; set; }
    //public string ViewEnginePath { get; set; }
    //public override string DisplayName { get; set; }

    public static bool HasAllowAnonymousAttribute(this ActionDescriptor actionDescriptor) => actionDescriptor.GetType().HasCustomAttribute<AllowAnonymousAttribute>();

    public static PageActionDescriptor PageActionDescriptor(this ActionDescriptor actionDescriptor) => (PageActionDescriptor)actionDescriptor;

    public static ActionDescriptorType GetActionDescriptorType(this ActionDescriptor actionDescriptor) {
      switch (actionDescriptor) {
        case ControllerActionDescriptor cad: return ActionDescriptorType.MvcController;
        case CompiledPageActionDescriptor cpad: return ActionDescriptorType.MvcRazorPage;
        case PageActionDescriptor pad: return ActionDescriptorType.MvcRazorPage;
        default: return ActionDescriptorType._Unkown;
      }
    }

    #region "DebugObject"
    public static ActionDescriptorDebugObject GetDebugObject(this ActionDescriptor actionDescriptor) => new ActionDescriptorDebugObject(actionDescriptor);
    #endregion
  }

  public class ActionDescriptorDebugObject : IDebugObject<ActionDescriptor> {
    public ActionDescriptorDebugObject(ActionDescriptor actionDescriptor) {
      this.actionDescriptor = actionDescriptor;
    }
    ActionDescriptor actionDescriptor;

    public string DisplayName => actionDescriptor.DisplayName;
    public string Id => actionDescriptor.Id;
    public AttributeRouteInfoDebugObject AttributeRouteInfo => actionDescriptor.AttributeRouteInfo?.GetDebugObject();
    public IEnumerable<ParameterDescriptorDebugObject> BoundProperties => actionDescriptor.BoundProperties?.Select(x => x.GetDebugObject());
    public IEnumerable<FilterDescriptorDebugObject> FilterDescriptors => actionDescriptor.FilterDescriptors?.Select(x => x.GetDebugObject());
    public IEnumerable<ParameterDescriptorDebugObject> Parameters => actionDescriptor.Parameters?.Select(x => x.GetDebugObject());

  }

  public enum ActionDescriptorType {
    _Unkown,
    MvcController,
    MvcRazorPage
  }

}
