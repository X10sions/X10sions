namespace System.Reflection;
public static class MemberInfoExtensions {
  /// <summary>Base</summary>
  public static string GetDeclaringTypeFullName(this MemberInfo member) => string.Format($"{member.DeclaringType.FullName}.{member.Name}");
  /// <summary>Derived</summary>
  public static string GetReflectedTypeFullName(this MemberInfo member) => string.Format($"{member.ReflectedType.FullName}.{member.Name}");
  public static bool IsField(this MemberInfo memberInfo) => memberInfo.MemberType == MemberTypes.Field;
  public static bool IsMethod(this MemberInfo memberInfo) => memberInfo.MemberType == MemberTypes.Method;
  public static bool IsPropertyNullable(this MemberInfo memberInfo) => memberInfo.ReflectedType.GetProperty(memberInfo.Name).IsNullable();
  public static bool IsProperty(this MemberInfo memberInfo) => memberInfo.MemberType == MemberTypes.Property;
  public static bool IsPropertyWithSetter(this MemberInfo member) => (member as PropertyInfo)?.GetSetMethod(true) != null;
}