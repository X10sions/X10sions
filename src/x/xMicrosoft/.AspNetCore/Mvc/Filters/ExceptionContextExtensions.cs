using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Microsoft.AspNetCore.Mvc.Filters {
  public static class ExceptionContextExtensions {
    public static ControllerActionDescriptor ControllerActionDescriptor(this ExceptionContext context) => context.ActionDescriptor.ControllerActionDescriptor();
  }
}
