using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Microsoft.AspNetCore.Mvc.Filters {

  public static class ActionExecutedContextExtensions {
    public static ControllerActionDescriptor ControllerActionDescriptor(this ActionExecutedContext context) => context.ActionDescriptor.ControllerActionDescriptor();
  }
}
