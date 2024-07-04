using System.Collections;
using System.Reflection;

namespace xSystem.Data.Linq.SqlClient;
internal struct MetaPosition : IEqualityComparer<MetaPosition>, IEqualityComparer {
  private int metadataToken;

  private Assembly assembly;

  internal MetaPosition(MemberInfo mi) {
    this = new MetaPosition(mi.DeclaringType.Assembly, mi.MetadataToken);
  }

  private MetaPosition(Assembly assembly, int metadataToken) {
    this.assembly = assembly;
    this.metadataToken = metadataToken;
  }

  public override bool Equals(object obj) {
    if (obj == null) {
      return false;
    }
    if (obj.GetType() != GetType()) {
      return false;
    }
    return AreEqual(this, (MetaPosition)obj);
  }

  public override int GetHashCode() => metadataToken;
  public bool Equals(MetaPosition x, MetaPosition y) => AreEqual(x, y);
  public int GetHashCode(MetaPosition obj) => obj.metadataToken;

  bool IEqualityComparer.Equals(object x, object y) => Equals((MetaPosition)x, (MetaPosition)y);

  int IEqualityComparer.GetHashCode(object obj) => GetHashCode((MetaPosition)obj);

  private static bool AreEqual(MetaPosition x, MetaPosition y) {
    if (x.metadataToken == y.metadataToken) {
      return x.assembly == y.assembly;
    }
    return false;
  }

  public static bool operator ==(MetaPosition x, MetaPosition y) => AreEqual(x, y);
  public static bool operator !=(MetaPosition x, MetaPosition y) => !AreEqual(x, y);

  internal static bool AreSameMember(MemberInfo x, MemberInfo y) {
    if (x.MetadataToken != y.MetadataToken || x.DeclaringType.Assembly != y.DeclaringType.Assembly) {
      return false;
    }
    return true;
  }
}
