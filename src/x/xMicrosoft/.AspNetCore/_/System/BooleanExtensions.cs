using Microsoft.AspNetCore.Mvc.Rendering;

namespace System {
  public static class BooleanExtensions {

    public static IEnumerable<SelectListItem> AsSelectListItems(this bool b, string falseText = "false", string trueText = "true") => new[]{
      new SelectListItem(falseText, bool.FalseString , !b),
      new SelectListItem(trueText, bool.TrueString, b)
    };

  }
}