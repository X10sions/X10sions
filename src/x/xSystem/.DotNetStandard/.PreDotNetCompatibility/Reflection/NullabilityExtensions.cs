namespace System.Reflection;

[PreDotNetCompatibility("?")]
public static class NullabilityExtensions {

  public static MemberInfo GetMemberWithSameMetadataDefinitionAs(this Type type, MemberInfo member) {
    member.ThrowIfNull();
    const BindingFlags all = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
    foreach (var myMemberInfo in type.GetMembers(all)) {
      if (myMemberInfo.HasSameMetadataDefinitionAs(member)) {
        return myMemberInfo;
      }
    }
    throw new Exception("CreateGetMemberWithSameMetadataDefinitionAsNotFoundException(member");
  }

  public static bool HasSameMetadataDefinitionAs(this MemberInfo mi, MemberInfo other) { throw new NotImplementedException("ByDesign"); }

  public static bool IsGenericMethodParameter(this Type type) => type.IsGenericParameter && type.DeclaringMethod != null;


}
