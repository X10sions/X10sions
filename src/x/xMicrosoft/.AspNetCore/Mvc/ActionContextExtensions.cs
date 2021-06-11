using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Reflection;

namespace Microsoft.AspNetCore.Mvc {
  public static class ActionContextExtensions {

    public static ActionDescriptorType GetActionDescriptorType(this ActionContext actionContext) => actionContext.ActionDescriptor.GetActionDescriptorType();

    public static bool IsMvcController(this ActionContext actionContext) => actionContext.GetActionDescriptorType() == ActionDescriptorType.MvcController;
    public static bool IsMvcRazorPage(this ActionContext actionContext) => actionContext.GetActionDescriptorType() == ActionDescriptorType.MvcRazorPage;

    public static bool HasAllowAnonymousAttribute(this ActionContext actionContext) => actionContext.ActionDescriptor.GetType().HasCustomAttribute<AllowAnonymousAttribute>();

  }
}
