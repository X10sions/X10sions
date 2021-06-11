using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Mvc.Rendering {
  public static class SelectListItemExtensions {

    public static IEnumerable<T> SelectedValues<T>(this IEnumerable<SelectListItem> SelectListItems)
      => from x in SelectListItems where x.Selected select x.Value.As<string, T>();

  }
}