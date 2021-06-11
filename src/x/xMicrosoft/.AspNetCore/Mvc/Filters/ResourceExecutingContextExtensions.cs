using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Microsoft.AspNetCore.Mvc.Filters {
  public static class ResourceExecutingContextExtensions {
    public static ControllerActionDescriptor ControllerActionDescriptor(this ResourceExecutingContext context) => context.ActionDescriptor.ControllerActionDescriptor();
  }
}
