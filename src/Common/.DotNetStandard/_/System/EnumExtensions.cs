using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace System;
public static class EnumNetStandardExtensions {

  public static string GetDisplayName(this Enum enumValue) {
    var type = enumValue.GetType();
    var name = Enum.GetName(type, enumValue);
    if (name is null) return enumValue.ToString(); // fallback for undefined values
    var field = type.GetField(name);
    if (field is null) return name;
    // Try DisplayAttribute.Name
    var displayAttr = type.GetCachedAttribute<DisplayAttribute>(name);
    if (!string.IsNullOrWhiteSpace(displayAttr?.Name)) return displayAttr.Name;
    // Try DescriptionAttribute.Description
    var descAttr = type.GetCachedAttribute<DescriptionAttribute>(name);
    if (!string.IsNullOrWhiteSpace(descAttr?.Description)) return descAttr.Description;
    // Fallback to Enum.GetName
    return name;
  }

  public static TAttribute GetAttribute<TAttribute>(this Enum @this) where TAttribute : Attribute {
    Type type = @this.GetType();
    string name = Enum.GetName(type, @this);
    return type.GetField(name).GetCustomAttributes(inherit: false).OfType<TAttribute>().FirstOrDefault();
  }

  public static TProperty GetCustomAttributeProperty<T, TProperty>(this Enum enumValue, Func<T, TProperty> getProperty, TProperty defaultValue, bool inherit = false) where T : Attribute {
    var attr = enumValue.GetCustomAttribute<T>();
    return attr is null ? defaultValue : getProperty(attr);
  }

  public static IEnumerable<T> GetCustomAttributes<T>(this Enum enumValue, bool inherit = false) where T : Attribute {
    var type = enumValue.GetType();
    var name = Enum.GetName(type, enumValue);
    return type.GetField(name).GetCustomAttributes<T>(inherit);
  }

  public static TAttribute? GetEnumAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute {
    var type = enumValue.GetType();
    var name = Enum.GetName(type, enumValue);
    return type.GetCachedAttribute<TAttribute>(name);
  }

  public static IEnumerable<T> GetEnumValues<T>() where T : Enum => typeof(T).GetEnumValues<T>();

  public static IEnumerable<T> GetEnumValues<T>(this Type type) where T : Enum {
    foreach (T value in Enum.GetValues(type)) {
      yield return value;
    }
  }
  public static IEnumerable<T> GetEnumValues<T>(this T enumValue) where T : Enum => GetEnumValues<T>();

  public static FieldInfo? GetFieldInfo(this Enum enumValue) {
    var type = enumValue.GetType();
    var name = Enum.GetName(type, enumValue);
    if (name is null) return null; // fallback for undefined values
    var field = type.GetField(name);
    return field;
  }
  public static string GetName(this Enum enumValue) {
    var type = enumValue.GetType();
    var name = Enum.GetName(type, enumValue);
    if (name is null) return enumValue.ToString(); // fallback for undefined values
    return name;
  }

  public static bool IsName<T>(this T e, string name) where T : Enum => e.ToString() == name;

}
