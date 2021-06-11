using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Microsoft.AspNetCore.Mvc.Filters {
  public static class ResultExecutingContextExtensions {
    public static ControllerActionDescriptor ControllerActionDescriptor(this ResultExecutingContext context) => context.ActionDescriptor.ControllerActionDescriptor();
  }
}
