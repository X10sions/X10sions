using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace Microsoft.AspNetCore.Mvc.Filters {
  public static class FilterContextExtensions {

    public static bool HasAllowAnonymousFilter(this FilterContext filterContext) => filterContext.Filters.Any(item => item is IAllowAnonymousFilter);

    public static ControllerActionDescriptor ControllerActionDescriptor(this FilterContext filterContext) => filterContext.ActionDescriptor.ControllerActionDescriptor();
    public static CompiledPageActionDescriptor CompiledPageActionDescriptor(this FilterContext filterContext) => filterContext.ActionDescriptor.CompiledPageActionDescriptor();

  }
}
