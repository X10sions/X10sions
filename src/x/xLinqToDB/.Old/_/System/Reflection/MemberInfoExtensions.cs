using LinqToDB.Expressions;

namespace System.Reflection {
  public static class MemberInfoExtensions {


    public static readonly MemberInfo LinqToDbSqlPropertyMethod = MemberHelper.MethodOf(() => LinqToDB.Sql.Property<string>(null, null)).GetGenericMethodDefinition();

    public static bool IsLinqToDbSqlPropertyMethod(this MemberInfo memberInfo)
      => memberInfo is MethodInfo methodCall && methodCall.IsGenericMethod && methodCall.GetGenericMethodDefinition() == LinqToDbSqlPropertyMethod;

  }
}
