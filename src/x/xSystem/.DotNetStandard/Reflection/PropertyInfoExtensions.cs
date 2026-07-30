using System.Globalization;
using System.Runtime.CompilerServices;

namespace System.Reflection;

public static class PropertyInfoExtensions {

  public static string GetPropertyName(this PropertyInfo property) => property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? property.Name;
  public static object GetPropertyValue(this PropertyInfo property, object instance) => property.GetValue(instance);
  public static bool IsComputedProperty(this PropertyInfo property) => property.SetMethod == null && property.GetMethod.GetCustomAttribute<CompilerGeneratedAttribute>() == null;

  //public static bool IsNullable(this PropertyInfo property) {
  //  // https://stackoverflow.com/questions/58453972/how-to-use-net-reflection-to-check-for-nullable-reference-type
  //  if (property.PropertyType.IsValueType) return Nullable.GetUnderlyingType(property.PropertyType) != null;
  //  var nullable = property.CustomAttributes.FirstOrDefault(x => x.AttributeType.FullName == NullableAttributeFullName);
  //  if (nullable != null && nullable.ConstructorArguments.Count == 1) {
  //    var attributeArgument = nullable.ConstructorArguments[0];
  //    if (attributeArgument.ArgumentType == typeof(byte[])) {
  //      var args = (ReadOnlyCollection<CustomAttributeTypedArgument>)attributeArgument.Value;
  //      if (args.Count > 0 && args[0].ArgumentType == typeof(byte)) {
  //        return (byte)args[0].Value == 2;
  //      }
  //    } else if (attributeArgument.ArgumentType == typeof(byte)) {
  //      return (byte)attributeArgument.Value == 2;
  //    }
  //  }
  //  var context = property.DeclaringType.CustomAttributes.FirstOrDefault(x => x.AttributeType.FullName == NullableContextAttributeFullName);
  //  if (context != null && context.ConstructorArguments.Count == 1 && context.ConstructorArguments[0].ArgumentType == typeof(byte)) {
  //    return (byte)context.ConstructorArguments[0].Value == 2;
  //  }
  //  // Couldn't find a suitable attribute
  //  return false;
  //}

  /// <summary>
  /// Currently handles string, int, DateTime, decimal, double
  /// </summary>
  /// <param name="prop"></param>
  /// <param name="entity"></param>
  /// <param name="value"></param>
  public static void ParsePrimitive(this PropertyInfo prop, object entity, object value) {
    if (prop.PropertyType == typeof(string)) {
      prop.SetValue(entity, value.ToString().Trim(), null);
    } else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?)) {
      if (value == null) {
        prop.SetValue(entity, null, null);
      } else {
        prop.SetValue(entity, int.Parse(value.ToString()), null);
      }
    } else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?)) {
      if (DateTime.TryParse(value.ToString(), out var date)) {
        prop.SetValue(entity, date, null);
      } else {
        //Making an assumption here about the format of dates in the source data.
        if (DateTime.TryParseExact(value.ToString(), "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.AssumeLocal, out date)) {
          prop.SetValue(entity, date, null);
        }
      }
    } else if (prop.PropertyType == typeof(decimal)) {
      prop.SetValue(entity, decimal.Parse(value.ToString()), null);
    } else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?)) {
      if (double.TryParse(value.ToString(), out var number)) {
        prop.SetValue(entity, number, null);
      }
    }
  }

}