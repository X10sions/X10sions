using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Microsoft.AspNetCore.Mvc.Filters {
  public static class PageHandlerExecutingContextExtensions {
    public static CompiledPageActionDescriptor CompiledPageActionDescriptor(this PageHandlerExecutingContext context) => context.ActionDescriptor.CompiledPageActionDescriptor();
  }
}
