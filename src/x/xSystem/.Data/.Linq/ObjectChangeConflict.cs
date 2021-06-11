using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq.Mapping;
using System.Linq;

namespace System.Data.Linq {
  public sealed class ObjectChangeConflict {
    private ReadOnlyCollection<MemberChangeConflict> memberConflicts;

    private object database;
    private bool? isDeleted;

    internal ChangeConflictSession Session { get; }
    internal TrackedObject TrackedObject { get; }
    public object Object => TrackedObject.Current;
    internal object Original { get; }
    public bool IsResolved { get; private set; }
    public bool IsDeleted {
      get {
        if (isDeleted.HasValue) {
          return isDeleted.Value;
        }
        return Database == null;
      }
    }

    internal object Database {
      get {
        if (database == null) {
          var refreshContext = Session.RefreshContext;
          var keyValues = CommonDataServices.GetKeyValues(TrackedObject.Type, Original);
          database = refreshContext.Services.GetObjectByKey(TrackedObject.Type, keyValues);
        }
        return database;
      }
    }

    public ReadOnlyCollection<MemberChangeConflict> MemberConflicts {
      get {
        if (memberConflicts == null) {
          var list = new List<MemberChangeConflict>();
          if (Database != null) {
            foreach (var persistentDataMember in TrackedObject.Type.PersistentDataMembers) {
              if (!persistentDataMember.IsAssociation && HasMemberConflict(persistentDataMember)) {
                list.Add(new MemberChangeConflict(this, persistentDataMember));
              }
            }
          }
          memberConflicts = list.AsReadOnly();
        }
        return memberConflicts;
      }
    }

    internal ObjectChangeConflict(ChangeConflictSession session, TrackedObject trackedObject) {
      Session = session;
      TrackedObject = trackedObject;
      Original = trackedObject.CreateDataCopy(trackedObject.Original);
    }

    internal ObjectChangeConflict(ChangeConflictSession session, TrackedObject trackedObject, bool isDeleted)
      : this(session, trackedObject) {
      this.isDeleted = isDeleted;
    }

    public void Resolve() => Resolve(RefreshMode.KeepCurrentValues, true);

    public void Resolve(RefreshMode refreshMode) => Resolve(refreshMode, false);

    public void Resolve(RefreshMode refreshMode, bool autoResolveDeletes) {
      if (autoResolveDeletes && IsDeleted) {
        ResolveDelete();
      } else {
        if (Database == null) {
          throw System.Data.Linq.Error.RefreshOfDeletedObject();
        }
        TrackedObject.Refresh(refreshMode, Database);
        IsResolved = true;
      }
    }

    private void ResolveDelete() {
      if (!TrackedObject.IsDeleted) {
        TrackedObject.ConvertToDeleted();
      }
      Session.Context.Services.RemoveCachedObjectLike(TrackedObject.Type, TrackedObject.Original);
      TrackedObject.AcceptChanges();
      IsResolved = true;
    }

    private bool HasMemberConflict(MetaDataMember member) {
      var boxedValue = member.StorageAccessor.GetBoxedValue(Original);
      if (!member.DeclaringType.Type.IsAssignableFrom(database.GetType())) {
        return false;
      }
      var boxedValue2 = member.StorageAccessor.GetBoxedValue(database);
      return !AreEqual(member, boxedValue, boxedValue2);
    }

    private bool AreEqual(MetaDataMember member, object v1, object v2) {
      if (v1 == null && v2 == null) {
        return true;
      }
      if (v1 == null || v2 == null) {
        return false;
      }
      if (member.Type == typeof(char[])) {
        return AreEqual((char[])v1, (char[])v2);
      }
      if (member.Type == typeof(byte[])) {
        return AreEqual((byte[])v1, (byte[])v2);
      }
      return object.Equals(v1, v2);
    }

    private bool AreEqual(char[] a1, char[] a2) {
      if (a1.Length != a2.Length) {
        return false;
      }
      var i = 0;
      for (var num = a1.Length; i < num; i++) {
        if (a1[i] != a2[i]) {
          return false;
        }
      }
      return true;
    }

    private bool AreEqual(byte[] a1, byte[] a2) {
      if (a1.Length != a2.Length) {
        return false;
      }
      var i = 0;
      for (var num = a1.Length; i < num; i++) {
        if (a1[i] != a2[i]) {
          return false;
        }
      }
      return true;
    }

    internal void OnMemberResolved() {
      if (!IsResolved) {
        var num = memberConflicts.AsEnumerable().Count((MemberChangeConflict m) => m.IsResolved);
        if (num == memberConflicts.Count) {
          Resolve(RefreshMode.KeepCurrentValues, false);
        }
      }
    }
  }

}