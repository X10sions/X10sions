using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Mvc.Infrastructure {
  public static class ActionDescriptorCollectionExtensions {
    public static IEnumerable<ControllerActionDescriptor> ControllerActionDescriptors(this ActionDescriptorCollection actionDescriptorCollection) => actionDescriptorCollection.Items.OfType<ControllerActionDescriptor>();
    public static IEnumerable<PageActionDescriptor> PageActionDescriptors(this ActionDescriptorCollection actionDescriptorCollection) => actionDescriptorCollection.Items.OfType<PageActionDescriptor>();
  }
}