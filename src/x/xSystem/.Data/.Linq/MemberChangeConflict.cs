using System.Data.Linq.Mapping;
using System.Reflection;

namespace System.Data.Linq {
  public sealed class MemberChangeConflict {
    private ObjectChangeConflict conflict;
    private MetaDataMember metaMember;
    public object OriginalValue { get; }
    public object DatabaseValue { get; }
    public object CurrentValue { get; }
    public MemberInfo Member => metaMember.Member;
    public bool IsModified => conflict.TrackedObject.HasChangedValue(metaMember);
    public bool IsResolved { get; private set; }

    internal MemberChangeConflict(ObjectChangeConflict conflict, MetaDataMember metaMember) {
      this.conflict = conflict;
      this.metaMember = metaMember;
      OriginalValue = metaMember.StorageAccessor.GetBoxedValue(conflict.Original);
      DatabaseValue = metaMember.StorageAccessor.GetBoxedValue(conflict.Database);
      CurrentValue = metaMember.StorageAccessor.GetBoxedValue(conflict.TrackedObject.Current);
    }

    public void Resolve(object value) {
      conflict.TrackedObject.RefreshMember(metaMember, RefreshMode.OverwriteCurrentValues, value);
      IsResolved = true;
      conflict.OnMemberResolved();
    }

    public void Resolve(RefreshMode refreshMode) {
      conflict.TrackedObject.RefreshMember(metaMember, refreshMode, DatabaseValue);
      IsResolved = true;
      conflict.OnMemberResolved();
    }
  }

}