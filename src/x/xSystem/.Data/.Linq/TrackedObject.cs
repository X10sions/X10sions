using System.Collections.Generic;
using System.Data.Linq.Mapping;

namespace System.Data.Linq {
  internal abstract class TrackedObject {
    internal abstract MetaType Type { get; }
    internal abstract object Current { get; }
    internal abstract object Original { get; }
    internal abstract bool IsInteresting { get; }
    internal abstract bool IsNew { get; }
    internal abstract bool IsDeleted { get; }
    internal abstract bool IsModified { get; }
    internal abstract bool IsUnmodified { get; }
    internal abstract bool IsPossiblyModified { get; }
    internal abstract bool IsRemoved { get; }
    internal abstract bool IsDead { get; }
    internal abstract bool IsWeaklyTracked { get; }
    internal abstract bool HasDeferredLoaders { get; }
    internal abstract bool HasChangedValues();
    internal abstract IEnumerable<ModifiedMemberInfo> GetModifiedMembers();
    internal abstract bool HasChangedValue(MetaDataMember mm);
    internal abstract bool CanInferDelete();
    internal abstract void AcceptChanges();
    internal abstract void ConvertToNew();
    internal abstract void ConvertToPossiblyModified();
    internal abstract void ConvertToPossiblyModified(object original);
    internal abstract void ConvertToUnmodified();
    internal abstract void ConvertToModified();
    internal abstract void ConvertToDeleted();
    internal abstract void ConvertToRemoved();
    internal abstract void ConvertToDead();
    internal abstract void Refresh(RefreshMode mode, object freshInstance);
    internal abstract void RefreshMember(MetaDataMember member, RefreshMode mode, object freshValue);
    internal abstract object CreateDataCopy(object instance);
    internal abstract bool SynchDependentData();
    internal abstract bool IsPendingGeneration(IEnumerable<MetaDataMember> keyMembers);
    internal abstract bool IsMemberPendingGeneration(MetaDataMember keyMember);
    internal abstract void InitializeDeferredLoaders();
  }
}