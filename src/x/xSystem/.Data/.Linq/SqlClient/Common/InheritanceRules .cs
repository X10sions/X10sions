using System.Data.Linq.SqlClient;
using System.Reflection;

namespace xSystem.Data.Linq.SqlClient.Common;
/// <summary>
/// https://github.com/nlh774/DotNetReferenceSource/blob/master/ndp/fx/src/DLinq/Dlinq/SqlClient/Common/InheritanceRules.cs
/// </summary>
static class InheritanceRules {
  /// <summary>
  /// Creates a name that is the same when the member should be considered 'same'
  /// for the purposes of the inheritance feature.
  /// </summary>
  internal static object DistinguishedMemberName(MemberInfo mi) {
    PropertyInfo pi = mi as PropertyInfo;
    FieldInfo fi = mi as FieldInfo;
    if (fi != null) {
      // Human readable variant:
      // return "fi:" + mi.Name + ":" + mi.DeclaringType;
      return new MetaPosition(mi);
    } else if (pi != null) {
      MethodInfo meth = null;

      if (pi.CanRead) {
        meth = pi.GetGetMethod();
      }
      if (meth == null && pi.CanWrite) {
        meth = pi.GetSetMethod();
      }
      bool isVirtual = meth != null && meth.IsVirtual;

      // Human readable variant:
      // return "pi:" + mi.Name + ":" + (isVirtual ? "virtual" : mi.DeclaringType.ToString());

      if (isVirtual) {
        return mi.Name;
      } else {
        return new MetaPosition(mi);
      }
    } else {
      throw Error.ArgumentOutOfRange("mi");
    }
  }

  /// <summary>
  /// Compares two MemberInfos for 'same-ness'.
  /// </summary>
  internal static bool AreSameMember(MemberInfo mi1, MemberInfo mi2) {
    return DistinguishedMemberName(mi1).Equals(DistinguishedMemberName(mi2));
  }

  /// <summary>
  /// The representation of a inheritance code when mapped to a specific provider type.
  /// </summary>
  internal static object InheritanceCodeForClientCompare(object rawCode, System.Data.Linq.SqlClient.ProviderType providerType) {
    // If its a fixed-size string type in the store then pad it with spaces so that 
    // comparing the string on the client agrees with the value returnd on the store.
    if (providerType.IsFixedSize && rawCode.GetType() == typeof(string)) {
      string s = (string)rawCode;
      if (providerType.Size.HasValue && s.Length != providerType.Size) {
        s = s.PadRight(providerType.Size.Value).Substring(0, providerType.Size.Value);
      }
      return s;
    }
    return rawCode;
  }

}