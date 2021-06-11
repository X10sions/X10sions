using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Microsoft.AspNetCore.Mvc.Filters {
  public static class PageHandlerExecutedContextExtensions {
    public static CompiledPageActionDescriptor CompiledPageActionDescriptor(this PageHandlerExecutedContext context) => context.ActionDescriptor.CompiledPageActionDescriptor();
  }
}
