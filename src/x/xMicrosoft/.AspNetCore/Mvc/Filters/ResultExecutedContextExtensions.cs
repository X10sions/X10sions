using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Microsoft.AspNetCore.Mvc.Filters {
  public static class ResultExecutedContextExtensions {
    public static ControllerActionDescriptor ControllerActionDescriptor(this ResultExecutedContext context) => context.ActionDescriptor.ControllerActionDescriptor();
  }
}
