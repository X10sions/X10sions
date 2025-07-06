using System.Reflection;
namespace System;
public static class AttributeExtensions {
  public static TAttr? GetCustomAttribute<TAttr, T>(this T source, bool inherit = false) where TAttr : Attribute => source?.GetType().GetField(source.ToString()).GetCustomAttribute<TAttr>(inherit);
  public static IEnumerable<TAttr>? GetCustomAttributes<TAttr, T>(this T source, bool inherit = false) where TAttr : Attribute => source?.GetType().GetField(source.ToString()).GetCustomAttributes<TAttr>(inherit);
}
