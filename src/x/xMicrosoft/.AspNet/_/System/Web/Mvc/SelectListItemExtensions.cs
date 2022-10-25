namespace System.Web.Mvc {
  public static class SelectListItemExtensions {

    public static IEnumerable<SelectListItem> GetSelectListItems<T>(this IEnumerable<T> enumerable, Func<T, object> getText, Func<T, string>? getValue = null, Func<T, bool>? getSelected = null, string? defaultOption = null) {
      var items = new List<SelectListItem>();
      if (defaultOption != null) {
        items.Add(new SelectListItem() {
          Text = defaultOption,
          Value = string.Empty
        });
      }
      foreach (var p in enumerable) {
        var text = getText(p).ToString();
        items.Add(new SelectListItem {
          Text = text,
          Value = (getValue == null) ? text : getValue(p),
          Selected = getSelected != null && getSelected(p)
        });
      }
      return items;
    }

    public static IEnumerable<SelectListItem> AsOrderedSelectListItems(this IEnumerable<string> enumerable, params string[] selectedValues)
      => from x in enumerable
         orderby x
         select new SelectListItem {
           Selected = selectedValues.Contains(x),
           Text = x,
           Value = x
         };

  }
}
