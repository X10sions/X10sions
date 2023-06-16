﻿//using System.Collections.ObjectModel;

namespace System.Reflection;
public static class MemberInfoExtensions {
  public static string GetDeclaringTypeFullName(this MemberInfo member) => string.Format($"{member.DeclaringType.FullName}.{member.Name}");//base
  public static string GetReflectedTypeFullName(this MemberInfo member) => string.Format($"{member.ReflectedType.FullName}.{member.Name}");//derived
  public static bool IsField(this MemberInfo memberInfo) => memberInfo.MemberType == MemberTypes.Field;
  public static bool IsMethod(this MemberInfo memberInfo) => memberInfo.MemberType == MemberTypes.Method;
  public static bool IsPropertyNullable(this MemberInfo memberInfo) => memberInfo.ReflectedType.GetProperty(memberInfo.Name).IsNullable();

  //public const string NullableAttributeFullName = "System.Runtime.CompilerServices.NullableAttribute";
  //public const string NullableContextAttributeFullName = "System.Runtime.CompilerServices.NullableContextAttribute";

  //public static bool IsNullable(this MemberInfo member) {
  //  // https://stackoverflow.com/questions/58453972/how-to-use-net-reflection-to-check-for-nullable-reference-type
  //  var type = member.GetType();
  //  if (type.IsValueType) return Nullable.GetUnderlyingType(type) != null;
  //  var nullable = member.CustomAttributes.FirstOrDefault(x => x.AttributeType.FullName == NullableAttributeFullName);
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
  //  var context = member.DeclaringType.CustomAttributes.FirstOrDefault(x => x.AttributeType.FullName == NullableContextAttributeFullName);
  //  if (context != null && context.ConstructorArguments.Count == 1 && context.ConstructorArguments[0].ArgumentType == typeof(byte)) {
  //    return (byte)context.ConstructorArguments[0].Value == 2;
  //  }
  //  // Couldn't find a suitable attribute
  //  return false;
  //}

  public static bool IsProperty(this MemberInfo memberInfo) => memberInfo.MemberType == MemberTypes.Property;
  public static bool IsPropertyWithSetter(this MemberInfo member) => (member as PropertyInfo)?.GetSetMethod(true) != null;

}