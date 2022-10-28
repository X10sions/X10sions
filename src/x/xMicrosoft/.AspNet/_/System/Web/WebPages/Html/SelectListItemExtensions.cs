using System.ComponentModel;

namespace System.Web.WebPages.Html;
public static class SelectListItemExtensions {

  public static IEnumerable<SelectListItem> AsSelectListItems<T>(this IEnumerable<T> enumerable, params T[] selectedValues) {
    var items = new List<SelectListItem>();
    foreach (var p in enumerable) {
      var text = p.ToString();
      items.Add(new SelectListItem {
        Text = text,
        Value = text,
        Selected = selectedValues.Contains(p)
      });
    }
    return items;
  }

  public static IEnumerable<SelectListItem> AsSelectListItems<T>(this IEnumerable<T> enumerable, Func<T, object> getText, Func<T, string>? getValue = null, Func<T, bool>? getSelected = null, string? defaultOption = null) {
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

  public static IEnumerable<SelectListItem> EnumToSelectList<T>(this T selectedValue) where T : Enum => EnumToSelectList(new[] { selectedValue });

  public static IEnumerable<SelectListItem> EnumToSelectList<T>(this T[] selectedValues) where T : Enum
    => from T x in Enum.GetValues(typeof(T))//.Cast<T>()
       select new SelectListItem {
         Text = x.ToString() + ": " + x.GetAttribute<T, DescriptionAttribute>()?.Description,
         Value = x.ToString(),
         Selected = selectedValues.Contains(x)
       };

}
