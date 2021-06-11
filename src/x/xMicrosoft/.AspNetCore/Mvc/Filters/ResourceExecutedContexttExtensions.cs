using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Microsoft.AspNetCore.Mvc.Filters {
  public static class ResourceExecutedContexttExtensions {
    public static ControllerActionDescriptor ControllerActionDescriptor(this ResourceExecutedContext context) => context.ActionDescriptor.ControllerActionDescriptor();
  }
}
