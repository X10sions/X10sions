using System.Runtime.CompilerServices;

namespace System.Reflection {
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

  }
}