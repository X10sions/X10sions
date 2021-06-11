using System.Collections.ObjectModel;
using System.Linq;

namespace System.Reflection {
  public static class MemberInfoExtensions {

    public static string DeclaringTypeFullName(this MemberInfo member) => string.Format($"{member.DeclaringType.FullName}.{member.Name}");
    public static string ReflectedTypeFullName(this MemberInfo member) => string.Format($"{member.ReflectedType.FullName}.{member.Name}");

    public static bool IsField(this MemberInfo memberInfo) => memberInfo.MemberType == MemberTypes.Field;
    public static bool IsMethod(this MemberInfo memberInfo) => memberInfo.MemberType == MemberTypes.Method;
    public static bool IsNullableHasValueMember(this MemberInfo member) => member.Name == nameof(Nullable<byte>.HasValue) && member.DeclaringType.IsGenericType && member.DeclaringType.GetGenericTypeDefinition() == typeof(Nullable<>);
    public static bool IsNullableValueMember(this MemberInfo member) => member.Name == nameof(Nullable<byte>.Value) && member.DeclaringType.IsGenericType && member.DeclaringType.GetGenericTypeDefinition() == typeof(Nullable<>);
    public static bool IsProperty(this MemberInfo memberInfo) => memberInfo.MemberType == MemberTypes.Property;
    public static bool IsPropertyWithSetter(this MemberInfo member) => (member as PropertyInfo)?.GetSetMethod(true) != null;

    public const string NullableAttributeFullName = "System.Runtime.CompilerServices.NullableAttribute";
    public const string NullableContextAttributeFullName = "System.Runtime.CompilerServices.NullableContextAttribute";

    public static bool IsNullable(this MemberInfo member) {
      // https://stackoverflow.com/questions/58453972/how-to-use-net-reflection-to-check-for-nullable-reference-type
      var type = member.GetType();
      if (type.IsValueType) return Nullable.GetUnderlyingType(type) != null;
      var nullable = member.CustomAttributes.FirstOrDefault(x => x.AttributeType.FullName == NullableAttributeFullName);
      if (nullable != null && nullable.ConstructorArguments.Count == 1) {
        var attributeArgument = nullable.ConstructorArguments[0];
        if (attributeArgument.ArgumentType == typeof(byte[])) {
          var args = (ReadOnlyCollection<CustomAttributeTypedArgument>)attributeArgument.Value;
          if (args.Count > 0 && args[0].ArgumentType == typeof(byte)) {
            return (byte)args[0].Value == 2;
          }
        } else if (attributeArgument.ArgumentType == typeof(byte)) {
          return (byte)attributeArgument.Value == 2;
        }
      }
      var context = member.DeclaringType.CustomAttributes.FirstOrDefault(x => x.AttributeType.FullName == NullableContextAttributeFullName);
      if (context != null && context.ConstructorArguments.Count == 1 && context.ConstructorArguments[0].ArgumentType == typeof(byte)) {
        return (byte)context.ConstructorArguments[0].Value == 2;
      }
      // Couldn't find a suitable attribute
      return false;
    }

  }
}