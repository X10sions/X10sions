using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace System.Collections.Generic {
  public static class IEnumerableExtensions {

    public static SelectList AsSelectList<T, TValue, TText>(this IEnumerable<T> source, Func<T, TValue>? valueFunc = null, Func<T, TText>? textFunc = null, Func<T, bool>? selectedFunc = null)
      => new SelectList(source.AsSelectListItems(valueFunc, textFunc, selectedFunc));

    public static IEnumerable<SelectListItem> AsSelectListItems<T>(this IEnumerable<string> source) => source.AsSelectListItems(x => x, x => x);
    public static IEnumerable<SelectListItem> AsSelectListItems<T>(this IEnumerable<T> source) where T : struct => source.AsSelectListItems(x => x.ToString(), x => x.ToString());

    public static IEnumerable<SelectListItem> AsSelectListItems<T, TValue>(this IEnumerable<T> source, Func<T, TValue> valueFunc, Func<T, bool>? selectedFunc = null, Func<T, bool>? whereFunc = null)
      => source.AsSelectListItems(valueFunc, valueFunc, selectedFunc, whereFunc);

    public static IEnumerable<SelectListItem> AsSelectListItems<T, TValue, TText>(this IEnumerable<T> source
      , Func<T, TValue>? valueFunc = null
      , Func<T, TText>? textFunc = null
      , Func<T, bool>? selectedFunc = null
      , Func<T, bool>? whereFunc = null
      ) =>
      from x in source.Where(whereFunc ?? (x => true))
      select new SelectListItem {
        Value = valueFunc != null ? valueFunc(x)?.ToString() : x.ToString(),
        Selected = selectedFunc != null ? selectedFunc(x) : false,
        Text = textFunc != null ? textFunc(x)?.ToString() : valueFunc != null ? valueFunc(x)?.ToString() : x.ToString()
      };

    //public static IEnumerable<SelectListItem> ConvertToSelectListItemCollection<T>(IEnumerable<T> source, Func<T, string> text, Func<T, string> value, bool createEmpty = true) where T : class {
    //  var selectListItems = new List<SelectListItem>();
    //  if (createEmpty) {
    //    selectListItems.Add(new SelectListItem { Text = "Please Select", Value = "", Selected = true });
    //  }
    //  foreach (var item in source) {
    //    selectListItems.Add(new SelectListItem { Text = text(item), Value = value(item) });
    //  }
    //  return selectListItems;
    //}

    //public static IEnumerable<SelectListItem> ConvertToSelectListItemCollection<T>(IEnumerable<T> source, Func<T, string> textAndValue, bool createEmpty = true) where T : class => ConvertToSelectListItemCollection(source, textAndValue, textAndValue, createEmpty);

    public static T? GetFromSystemTextJson<T>(this IEnumerable<KeyValuePair<string, string>> kvps, string key, T defaultValue) {
      var json = kvps.GetValue(key);
      return json is null ? defaultValue : JsonSerializer.Deserialize<T>(json, options);
    }

    // public static SelectList ToSelectList<T>(this IEnumerable<T> source) => source.ToSelectList(x => x.ToString(), x => x.ToString(), _ => false);
    // public static SelectList ToSelectList<T, TValue>(this IEnumerable<T> source, Func<T, TValue> valueFunc) => source.ToSelectList(valueFunc, valueFunc, _ => false);
    // public static SelectList ToSelectList<T, TValue>(this IEnumerable<T> source, Func<T, TValue> valueFunc, Func<T, bool> selectedFunc) => source.ToSelectList(valueFunc, valueFunc, selectedFunc);
    // public static SelectList ToSelectList<T, TValue, TText>(this IEnumerable<T> source, Func<T, TValue> valueFunc, Func<T, TText> textFunc) => source.ToSelectList(valueFunc, textFunc, _ => false);
    // public static IEnumerable<SelectListItem> ToSelectListItems<T, TValue>(this IEnumerable<T> source, Func<T, TValue> valueFunc) => source.ToSelectListItems(valueFunc, valueFunc, _ => false);
    // public static IEnumerable<SelectListItem> ToSelectListItems<T, TValue>(this IEnumerable<T> source, Func<T, TValue> valueFunc, Func<T, bool> selectedFunc) => source.ToSelectListItems(valueFunc, valueFunc, selectedFunc);
    // public static IEnumerable<SelectListItem> ToSelectListItems<T, TValue, TText>(this IEnumerable<T> source, Func<T, TValue> valueFunc, Func<T, TText> textFunc) => source.ToSelectListItems(valueFunc, textFunc, _ => false);
    // public static IEnumerable<SelectListItem> ToSelectListItemsWhere<T, TValue, TText>(this IEnumerable<T> source, Func<T, bool> whereFunc, Func<T, TValue> valueFunc, Func<T, TText> textFunc) => source.Where(whereFunc).ToSelectListItems(valueFunc, textFunc, _ => false);
    // public static IList<SelectListItem> ToSelectList<T, TValue, TText>(this IEnumerable<T> source, Func<T, TValue>? valueFunc = null, Func<T, bool>? selectedFunc = null) => source.AsSelectListItems(valueFunc, valueFunc, selectedFunc).ToList();

    public static IList<SelectListItem> ToSelectList<T, TValue, TText>(
      this IEnumerable<T> source,
      Func<T, TValue>? valueFunc = null,
      Func<T, TText>? textFunc = null,
      Func<T, bool>? selectedFunc = null,
      Func<T, bool>? whereFunc = null)
      => source.AsSelectListItems(valueFunc, textFunc, selectedFunc, whereFunc).ToList();

    private static readonly JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

  }
}
