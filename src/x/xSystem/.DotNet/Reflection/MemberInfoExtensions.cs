namespace System.Reflection;
public static class MemberInfoExtensions {

  public static bool IsPropertyNullable(this MemberInfo memberInfo) => memberInfo.ReflectedType.GetProperty(memberInfo.Name).IsNullable();
}