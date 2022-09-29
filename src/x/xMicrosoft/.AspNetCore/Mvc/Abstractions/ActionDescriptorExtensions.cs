using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace Microsoft.AspNetCore.Mvc.Abstractions;

public static class ActionDescriptorExtensions {

  public static string? AreaName(this ActionDescriptor actionDescriptor)
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
}

public enum ActionDescriptorType {
  _Unkown,
  MvcController,
  MvcRazorPage
}

