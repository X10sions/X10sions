using System.Collections.Generic;
using System.Linq;

namespace System {
  public static class EnumExtensions {

    public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute {
      var type = enumValue.GetType();
      var name = Enum.GetName(type, enumValue);
      return type.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().FirstOrDefault();
    }

    public static IEnumerable<T> GetValuesCast<T>() where T : struct, IConvertible, IComparable, IFormattable => Enum.GetValues(typeof(T)).Cast<T>();
    public static IEnumerable<T> GetValuesOfType<T>() where T : Enum => Enum.GetValues(typeof(T)).OfType<T>();

    public static IEnumerable<T> GetValuesCast<T>(this T enumObj) where T : struct, IConvertible, IComparable, IFormattable => Enum.GetValues(enumObj.GetType()).Cast<T>();
    public static IEnumerable<T> GetValuesOfType<T>(this T enumObj) where T : Enum => Enum.GetValues(enumObj.GetType()).OfType<T>();

    //[Obsolete("Use GetAttribute")] public static string GetDescription(this Enum enumValue) => enumValue.GetAttribute<DescriptionAttribute>()?.Description ?? enumValue.ToString();
    //[Obsolete("Use GetAttribute")] public static string GetDisplay(this Enum enumValue) => enumValue.GetAttribute<DisplayAttribute>()?.Name ?? enumValue.ToString();
    //[Obsolete("Use GetAttribute")] public static string GetDisplayName(this Enum enumValue) => enumValue.GetAttribute<DisplayNameAttribute>()?.DisplayName ?? enumValue.ToString();

    //public static IEnumerable<SelectListItem> ToSelectList<TEnum>(this TEnum enumObj) where TEnum : Enum => EnumToSelectList(new[] { enumObj });
    //public static IEnumerable<SelectListItem> EnumToSelectList<T>(this T[] selectedValues) => Enum.GetValues(typeof(T)).Cast<T>().Select(x => new SelectListItem {
    //  Text = x.ToString() + ": " + x.GetAttribute<T, DescriptionAttribute>()?.Description,
    //  Value = x.ToString(),
    //  Selected = selectedValues.Contains(x)
    //});

  }
}