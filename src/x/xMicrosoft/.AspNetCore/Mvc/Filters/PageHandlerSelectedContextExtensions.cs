using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Microsoft.AspNetCore.Mvc.Filters {
  public static class PageHandlerSelectedContextExtensions {
    public static CompiledPageActionDescriptor CompiledPageActionDescriptor(this PageHandlerSelectedContext context) => context.ActionDescriptor.CompiledPageActionDescriptor();
  }
}
