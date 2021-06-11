using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Microsoft.AspNetCore.Mvc.Filters {
  public static class ActionExecutingContextExtensions {
    public static ControllerActionDescriptor ControllerActionDescriptor(this ActionExecutingContext context) => context.ActionDescriptor.ControllerActionDescriptor();
  }
}
