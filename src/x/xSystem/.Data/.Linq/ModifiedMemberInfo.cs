using System.Reflection;

namespace System.Data.Linq {
  public struct ModifiedMemberInfo {
    public MemberInfo Member { get; }
    public object CurrentValue { get; }
    public object OriginalValue { get; }
    internal ModifiedMemberInfo(MemberInfo member, object current, object original) {
      Member = member;
      CurrentValue = current;
      OriginalValue = original;
    }
  }

}
