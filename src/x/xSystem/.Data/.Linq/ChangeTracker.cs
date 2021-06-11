using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace System.Data.Linq {
  internal abstract class ChangeTracker {
    private class StandardChangeTracker : ChangeTracker {
      private class StandardTrackedObject : TrackedObject {
        private enum State {
          New,
          Deleted,
          PossiblyModified,
          Modified,
          Removed,
          Dead
        }

        private StandardChangeTracker tracker;
        private MetaType type;
        private object current;
        private object original;
        private State state;
        private BitArray dirtyMemberCache;
        private bool haveInitializedDeferredLoaders;
        private bool isWeaklyTracked;
        internal override bool IsWeaklyTracked => isWeaklyTracked;
        internal override MetaType Type => type;
        internal override object Current => current;
        internal override object Original => original;
        internal override bool IsNew => state == State.New;
        internal override bool IsDeleted => state == State.Deleted;
        internal override bool IsRemoved => state == State.Removed;
        internal override bool IsDead => state == State.Dead;
        internal override bool IsModified {
          get {
            if (state != State.Modified) {
              if (state == State.PossiblyModified && current != original) {
                return HasChangedValues();
              }
              return false;
            }
            return true;
          }
        }

        internal override bool IsUnmodified {
          get {
            if (state == State.PossiblyModified) {
              if (current != original) {
                return !HasChangedValues();
              }
              return true;
            }
            return false;
          }
        }

        internal override bool IsPossiblyModified {
          get {
            if (state != State.Modified) {
              return state == State.PossiblyModified;
            }
            return true;
          }
        }

        internal override bool IsInteresting {
          get {
            if (state != 0 && state != State.Deleted && state != State.Modified && (state != State.PossiblyModified || current == original)) {
              return CanInferDelete();
            }
            return true;
          }
        }

        internal override bool HasDeferredLoaders {
          get {
            foreach (var association in Type.Associations) {
              if (HasDeferredLoader(association.ThisMember)) {
                return true;
              }
            }
            var enumerable = Type.PersistentDataMembers.Where(delegate (MetaDataMember p) {
              if (p.IsDeferred) {
                return !p.IsAssociation;
              }
              return false;
            });
            foreach (var item in enumerable) {
              if (HasDeferredLoader(item)) {
                return true;
              }
            }
            return false;
          }
        }

        public override string ToString() => type.Name + ":" + GetState();

        private string GetState() {
          var state = this.state;
          if ((uint)state <= 1u || (uint)(state - 4) <= 1u) {
            return this.state.ToString();
          }
          if (IsModified) {
            return "Modified";
          }
          return "Unmodified";
        }

        internal StandardTrackedObject(StandardChangeTracker tracker, MetaType type, object current, object original) {
          if (current == null) {
            throw System.Data.Linq.Error.ArgumentNull("current");
          }
          this.tracker = tracker;
          this.type = type.GetInheritanceType(current.GetType());
          this.current = current;
          this.original = original;
          state = State.PossiblyModified;
          dirtyMemberCache = new BitArray(this.type.DataMembers.Count);
        }

        internal StandardTrackedObject(StandardChangeTracker tracker, MetaType type, object current, object original, bool isWeaklyTracked)
          : this(tracker, type, current, original) {
          this.isWeaklyTracked = isWeaklyTracked;
        }

        internal override bool CanInferDelete() {
          if (state == State.Modified || state == State.PossiblyModified) {
            foreach (var association in Type.Associations) {
              if (association.DeleteOnNull && association.IsForeignKey && !association.IsNullable && !association.IsMany && association.ThisMember.StorageAccessor.HasAssignedValue(Current) && association.ThisMember.StorageAccessor.GetBoxedValue(Current) == null) {
                return true;
              }
            }
          }
          return false;
        }

        internal override void ConvertToNew() {
          original = null;
          state = State.New;
        }

        internal override void ConvertToPossiblyModified() {
          state = State.PossiblyModified;
          isWeaklyTracked = false;
        }

        internal override void ConvertToModified() {
          state = State.Modified;
          isWeaklyTracked = false;
        }

        internal override void ConvertToPossiblyModified(object originalState) {
          state = State.PossiblyModified;
          original = CreateDataCopy(originalState);
          isWeaklyTracked = false;
        }

        internal override void ConvertToDeleted() {
          state = State.Deleted;
          isWeaklyTracked = false;
        }

        internal override void ConvertToDead() {
          state = State.Dead;
          isWeaklyTracked = false;
        }

        internal override void ConvertToRemoved() {
          state = State.Removed;
          isWeaklyTracked = false;
        }

        internal override void ConvertToUnmodified() {
          state = State.PossiblyModified;
          if (current is INotifyPropertyChanging) {
            original = current;
          } else {
            original = CreateDataCopy(current);
          }
          ResetDirtyMemberTracking();
          isWeaklyTracked = false;
        }

        internal override void AcceptChanges() {
          if (IsWeaklyTracked) {
            InitializeDeferredLoaders();
            isWeaklyTracked = false;
          }
          if (IsDeleted) {
            ConvertToDead();
          } else if (IsNew) {
            InitializeDeferredLoaders();
            ConvertToUnmodified();
          } else if (IsPossiblyModified) {
            ConvertToUnmodified();
          }
        }

        private void AssignMember(object instance, MetaDataMember mm, object value) {
          if (!(current is INotifyPropertyChanging)) {
            mm.StorageAccessor.SetBoxedValue(ref instance, value);
          } else {
            mm.MemberAccessor.SetBoxedValue(ref instance, value);
          }
        }

        private void ResetDirtyMemberTracking() => dirtyMemberCache.SetAll(false);

        internal override void Refresh(RefreshMode mode, object freshInstance) {
          SynchDependentData();
          UpdateDirtyMemberCache();
          var type = freshInstance.GetType();
          foreach (var persistentDataMember in this.type.PersistentDataMembers) {
            var refreshMode = persistentDataMember.IsDbGenerated ? RefreshMode.OverwriteCurrentValues : mode;
            if (refreshMode != 0 && !persistentDataMember.IsAssociation && (Type.Type == type || persistentDataMember.DeclaringType.Type.IsAssignableFrom(type))) {
              var boxedValue = persistentDataMember.StorageAccessor.GetBoxedValue(freshInstance);
              RefreshMember(persistentDataMember, refreshMode, boxedValue);
            }
          }
          original = CreateDataCopy(freshInstance);
          if (mode == RefreshMode.OverwriteCurrentValues) {
            ResetDirtyMemberTracking();
          }
        }

        private void UpdateDirtyMemberCache() {
          foreach (var persistentDataMember in type.PersistentDataMembers) {
            if ((!persistentDataMember.IsAssociation || !persistentDataMember.Association.IsMany) && !dirtyMemberCache.Get(persistentDataMember.Ordinal) && HasChangedValue(persistentDataMember)) {
              dirtyMemberCache.Set(persistentDataMember.Ordinal, true);
            }
          }
        }

        internal override void RefreshMember(MetaDataMember mm, RefreshMode mode, object freshValue) {
          if (mode != 0 && (!HasChangedValue(mm) || mode == RefreshMode.OverwriteCurrentValues)) {
            var boxedValue = mm.StorageAccessor.GetBoxedValue(current);
            if (!object.Equals(freshValue, boxedValue)) {
              mm.StorageAccessor.SetBoxedValue(ref current, freshValue);
              foreach (var item in GetAssociationsForKey(mm)) {
                if (!item.Association.IsMany) {
                  var source = tracker.services.GetDeferredSourceFactory(item).CreateDeferredSource(current);
                  if (item.StorageAccessor.HasValue(current)) {
                    AssignMember(current, item, source.Cast<object>().SingleOrDefault());
                  }
                }
              }
            }
          }
        }

        private IEnumerable<MetaDataMember> GetAssociationsForKey(MetaDataMember key) {
          foreach (var persistentDataMember in type.PersistentDataMembers) {
            if (persistentDataMember.IsAssociation && persistentDataMember.Association.ThisKey.Contains(key)) {
              yield return persistentDataMember;
            }
          }
        }

        internal override object CreateDataCopy(object instance) {
          var type = instance.GetType();
          var instance2 = Activator.CreateInstance(Type.Type);
          var inheritanceRoot = tracker.services.Model.GetTable(type).RowType.InheritanceRoot;
          foreach (var persistentDataMember in inheritanceRoot.GetInheritanceType(type).PersistentDataMembers) {
            if (!(Type.Type != type) || persistentDataMember.DeclaringType.Type.IsAssignableFrom(type)) {
              if (persistentDataMember.IsDeferred) {
                if (!persistentDataMember.IsAssociation) {
                  if (persistentDataMember.StorageAccessor.HasValue(instance)) {
                    var boxedValue = persistentDataMember.DeferredValueAccessor.GetBoxedValue(instance);
                    persistentDataMember.DeferredValueAccessor.SetBoxedValue(ref instance2, boxedValue);
                  } else {
                    var value = tracker.services.GetDeferredSourceFactory(persistentDataMember).CreateDeferredSource(instance2);
                    persistentDataMember.DeferredSourceAccessor.SetBoxedValue(ref instance2, value);
                  }
                }
              } else {
                var boxedValue2 = persistentDataMember.StorageAccessor.GetBoxedValue(instance);
                persistentDataMember.StorageAccessor.SetBoxedValue(ref instance2, boxedValue2);
              }
            }
          }
          return instance2;
        }

        internal void StartTracking() {
          if (original == current) {
            original = CreateDataCopy(current);
          }
        }

        internal override bool SynchDependentData() {
          var result = false;
          foreach (var association in Type.Associations) {
            var thisMember = association.ThisMember;
            if (association.IsForeignKey) {
              var flag = thisMember.StorageAccessor.HasAssignedValue(current);
              var flag2 = thisMember.StorageAccessor.HasLoadedValue(current);
              if (flag | flag2) {
                var boxedValue = thisMember.StorageAccessor.GetBoxedValue(current);
                if (boxedValue != null) {
                  var i = 0;
                  for (var count = association.ThisKey.Count; i < count; i++) {
                    var metaDataMember = association.ThisKey[i];
                    var metaDataMember2 = association.OtherKey[i];
                    var boxedValue2 = metaDataMember2.StorageAccessor.GetBoxedValue(boxedValue);
                    metaDataMember.StorageAccessor.SetBoxedValue(ref current, boxedValue2);
                    result = true;
                  }
                } else if (association.IsNullable) {
                  if (thisMember.IsDeferred || (original != null && thisMember.MemberAccessor.GetBoxedValue(original) != null)) {
                    var j = 0;
                    for (var count2 = association.ThisKey.Count; j < count2; j++) {
                      var metaDataMember3 = association.ThisKey[j];
                      if (metaDataMember3.CanBeNull) {
                        if (original != null && HasChangedValue(metaDataMember3)) {
                          if (metaDataMember3.StorageAccessor.GetBoxedValue(current) != null) {
                            throw System.Data.Linq.Error.InconsistentAssociationAndKeyChange(metaDataMember3.Member.Name, thisMember.Member.Name);
                          }
                        } else {
                          metaDataMember3.StorageAccessor.SetBoxedValue(ref current, null);
                          result = true;
                        }
                      }
                    }
                  }
                } else if (!flag2) {
                  var stringBuilder = new StringBuilder();
                  foreach (var item in association.ThisKey) {
                    if (stringBuilder.Length > 0) {
                      stringBuilder.Append(", ");
                    }
                    stringBuilder.AppendFormat("{0}.{1}", Type.Name.ToString(), item.Name);
                  }
                  throw System.Data.Linq.Error.CouldNotRemoveRelationshipBecauseOneSideCannotBeNull(association.OtherType.Name, Type.Name, stringBuilder);
                }
              }
            }
          }
          if (type.HasInheritance) {
            if (original != null) {
              var boxedValue3 = type.Discriminator.MemberAccessor.GetBoxedValue(current);
              var metaType = TypeFromDiscriminator(type, boxedValue3);
              var boxedValue4 = type.Discriminator.MemberAccessor.GetBoxedValue(original);
              var metaType2 = TypeFromDiscriminator(type, boxedValue4);
              if (metaType != metaType2) {
                throw System.Data.Linq.Error.CannotChangeInheritanceType(boxedValue4, boxedValue3, original.GetType().Name, metaType);
              }
            } else {
              var inheritanceType = type.GetInheritanceType(current.GetType());
              if (inheritanceType.HasInheritanceCode) {
                var inheritanceCode = inheritanceType.InheritanceCode;
                type.Discriminator.MemberAccessor.SetBoxedValue(ref current, inheritanceCode);
                result = true;
              }
            }
          }
          return result;
        }

        internal override bool HasChangedValue(MetaDataMember mm) {
          if (current == original) {
            return false;
          }
          if (mm.IsAssociation && mm.Association.IsMany) {
            return mm.StorageAccessor.HasAssignedValue(original);
          }
          if (mm.StorageAccessor.HasValue(current)) {
            if (original != null && mm.StorageAccessor.HasValue(original)) {
              if (dirtyMemberCache.Get(mm.Ordinal)) {
                return true;
              }
              var boxedValue = mm.MemberAccessor.GetBoxedValue(original);
              var boxedValue2 = mm.MemberAccessor.GetBoxedValue(current);
              if (!object.Equals(boxedValue2, boxedValue)) {
                return true;
              }
              return false;
            }
            if (mm.IsDeferred && mm.StorageAccessor.HasAssignedValue(current)) {
              return true;
            }
          }
          return false;
        }

        internal override bool HasChangedValues() {
          if (current == original) {
            return false;
          }
          if (IsNew) {
            return true;
          }
          foreach (var persistentDataMember in type.PersistentDataMembers) {
            if (!persistentDataMember.IsAssociation && HasChangedValue(persistentDataMember)) {
              return true;
            }
          }
          return false;
        }

        internal override IEnumerable<ModifiedMemberInfo> GetModifiedMembers() {
          foreach (var persistentDataMember in type.PersistentDataMembers) {
            if (IsModifiedMember(persistentDataMember)) {
              var currentValue = persistentDataMember.MemberAccessor.GetBoxedValue(current);
              if (original != null && persistentDataMember.StorageAccessor.HasValue(original)) {
                var boxedValue = persistentDataMember.MemberAccessor.GetBoxedValue(original);
                yield return new ModifiedMemberInfo(persistentDataMember.Member, currentValue, boxedValue);
              } else if (original == null || (persistentDataMember.IsDeferred && !persistentDataMember.StorageAccessor.HasLoadedValue(current))) {
                yield return new ModifiedMemberInfo(persistentDataMember.Member, currentValue, null);
              }
            }
          }
        }

        private bool IsModifiedMember(MetaDataMember member) {
          if (!member.IsAssociation && !member.IsPrimaryKey && !member.IsVersion && !member.IsDbGenerated && member.StorageAccessor.HasAssignedValue(current)) {
            if (state != State.Modified) {
              if (state == State.PossiblyModified) {
                return HasChangedValue(member);
              }
              return false;
            }
            return true;
          }
          return false;
        }

        private bool HasDeferredLoader(MetaDataMember deferredMember) {
          if (!deferredMember.IsDeferred) {
            return false;
          }
          var storageAccessor = deferredMember.StorageAccessor;
          if (storageAccessor.HasAssignedValue(current) || storageAccessor.HasLoadedValue(current)) {
            return false;
          }
          var deferredSourceAccessor = deferredMember.DeferredSourceAccessor;
          var enumerable = (IEnumerable)deferredSourceAccessor.GetBoxedValue(current);
          return enumerable != null;
        }

        internal override void InitializeDeferredLoaders() {
          if (tracker.services.Context.DeferredLoadingEnabled) {
            foreach (var association in Type.Associations) {
              if (!IsPendingGeneration(association.ThisKey)) {
                InitializeDeferredLoader(association.ThisMember);
              }
            }
            var enumerable = Type.PersistentDataMembers.Where(delegate (MetaDataMember p) {
              if (p.IsDeferred) {
                return !p.IsAssociation;
              }
              return false;
            });
            foreach (var item in enumerable) {
              if (!IsPendingGeneration(Type.IdentityMembers)) {
                InitializeDeferredLoader(item);
              }
            }
            haveInitializedDeferredLoaders = true;
          }
        }

        private void InitializeDeferredLoader(MetaDataMember deferredMember) {
          var storageAccessor = deferredMember.StorageAccessor;
          if (!storageAccessor.HasAssignedValue(current) && !storageAccessor.HasLoadedValue(current)) {
            var deferredSourceAccessor = deferredMember.DeferredSourceAccessor;
            var enumerable = (IEnumerable)deferredSourceAccessor.GetBoxedValue(current);
            if (enumerable == null) {
              var deferredSourceFactory = tracker.services.GetDeferredSourceFactory(deferredMember);
              enumerable = deferredSourceFactory.CreateDeferredSource(current);
              deferredSourceAccessor.SetBoxedValue(ref current, enumerable);
            } else if (enumerable != null && !haveInitializedDeferredLoaders) {
              throw System.Data.Linq.Error.CannotAttachAddNonNewEntities();
            }
          }
        }

        internal override bool IsPendingGeneration(IEnumerable<MetaDataMember> key) {
          if (IsNew) {
            foreach (var item in key) {
              if (IsMemberPendingGeneration(item)) {
                return true;
              }
            }
          }
          return false;
        }

        internal override bool IsMemberPendingGeneration(MetaDataMember keyMember) {
          if (IsNew && keyMember.IsDbGenerated) {
            return true;
          }
          foreach (var association in type.Associations) {
            if (association.IsForeignKey) {
              var num = association.ThisKey.IndexOf(keyMember);
              if (num > -1) {
                object obj = null;
                obj = ((!association.ThisMember.IsDeferred) ? association.ThisMember.StorageAccessor.GetBoxedValue(current) : association.ThisMember.DeferredValueAccessor.GetBoxedValue(current));
                if (obj != null && !association.IsMany) {
                  var standardTrackedObject = (StandardTrackedObject)tracker.GetTrackedObject(obj);
                  if (standardTrackedObject != null) {
                    var keyMember2 = association.OtherKey[num];
                    return standardTrackedObject.IsMemberPendingGeneration(keyMember2);
                  }
                }
              }
            }
          }
          return false;
        }
      }

      private Dictionary<object, StandardTrackedObject> items;

      private PropertyChangingEventHandler onPropertyChanging;

      private CommonDataServices services;

      internal StandardChangeTracker(CommonDataServices services) {
        this.services = services;
        items = new Dictionary<object, StandardTrackedObject>();
        onPropertyChanging = OnPropertyChanging;
      }

      private static MetaType TypeFromDiscriminator(MetaType root, object discriminator) {
        foreach (var inheritanceType in root.InheritanceTypes) {
          if (IsSameDiscriminator(discriminator, inheritanceType.InheritanceCode)) {
            return inheritanceType;
          }
        }
        return root.InheritanceDefault;
      }

      private static bool IsSameDiscriminator(object discriminator1, object discriminator2) {
        if (discriminator1 == discriminator2) {
          return true;
        }
        if (discriminator1 == null || discriminator2 == null) {
          return false;
        }
        return discriminator1.Equals(discriminator2);
      }

      internal override TrackedObject Track(object obj) => Track(obj, false);

      internal override TrackedObject Track(object obj, bool recurse) {
        var metaType = services.Model.GetMetaType(obj.GetType());
        var visited = new Dictionary<object, object>();
        return Track(metaType, obj, visited, recurse, 1);
      }

      private TrackedObject Track(MetaType mt, object obj, Dictionary<object, object> visited, bool recurse, int level) {
        var standardTrackedObject = (StandardTrackedObject)GetTrackedObject(obj);
        if (standardTrackedObject != null || visited.ContainsKey(obj)) {
          return standardTrackedObject;
        }
        var isWeaklyTracked = level > 1;
        standardTrackedObject = new StandardTrackedObject(this, mt, obj, obj, isWeaklyTracked);
        if (standardTrackedObject.HasDeferredLoaders) {
          throw System.Data.Linq.Error.CannotAttachAddNonNewEntities();
        }
        items.Add(obj, standardTrackedObject);
        Attach(obj);
        visited.Add(obj, obj);
        if (recurse) {
          foreach (var parent in services.GetParents(mt, obj)) {
            Track(parent.Type, parent.Item, visited, recurse, level + 1);
          }
          {
            foreach (var child in services.GetChildren(mt, obj)) {
              Track(child.Type, child.Item, visited, recurse, level + 1);
            }
            return standardTrackedObject;
          }
        }
        return standardTrackedObject;
      }

      internal override void FastTrack(object obj) => Attach(obj);

      internal override void StopTracking(object obj) {
        Detach(obj);
        items.Remove(obj);
      }

      internal override bool IsTracked(object obj) {
        if (!items.ContainsKey(obj)) {
          return IsFastTracked(obj);
        }
        return true;
      }

      private bool IsFastTracked(object obj) {
        var rowType = services.Model.GetTable(obj.GetType()).RowType;
        return services.IsCachedObject(rowType, obj);
      }

      internal override TrackedObject GetTrackedObject(object obj) {
        if (!items.TryGetValue(obj, out var value) && IsFastTracked(obj)) {
          return PromoteFastTrackedObject(obj);
        }
        return value;
      }

      private StandardTrackedObject PromoteFastTrackedObject(object obj) {
        var type = obj.GetType();
        var inheritanceType = services.Model.GetTable(type).RowType.GetInheritanceType(type);
        return PromoteFastTrackedObject(inheritanceType, obj);
      }

      private StandardTrackedObject PromoteFastTrackedObject(MetaType type, object obj) {
        var standardTrackedObject = new StandardTrackedObject(this, type, obj, obj);
        items.Add(obj, standardTrackedObject);
        return standardTrackedObject;
      }

      private void Attach(object obj) {
        var notifyPropertyChanging = obj as INotifyPropertyChanging;
        if (notifyPropertyChanging != null) {
          notifyPropertyChanging.PropertyChanging += onPropertyChanging;
        } else {
          OnPropertyChanging(obj, null);
        }
      }

      private void Detach(object obj) {
        var notifyPropertyChanging = obj as INotifyPropertyChanging;
        if (notifyPropertyChanging != null) {
          notifyPropertyChanging.PropertyChanging -= onPropertyChanging;
        }
      }

      private void OnPropertyChanging(object sender, PropertyChangingEventArgs args) {
        if (items.TryGetValue(sender, out var value)) {
          value.StartTracking();
        } else if (IsFastTracked(sender)) {
          value = PromoteFastTrackedObject(sender);
          value.StartTracking();
        }
      }

      internal override void AcceptChanges() {
        var list = new List<StandardTrackedObject>(items.Values);
        foreach (var item in list) {
          item.AcceptChanges();
        }
      }

      internal override IEnumerable<TrackedObject> GetInterestingObjects() {
        var enumerator = items.Values.GetEnumerator();
        try {
          while (enumerator.MoveNext()) {
            var current = enumerator.Current;
            if (current.IsInteresting) {
              yield return current;
            }
          }
        } finally {
          ((IDisposable)enumerator).Dispose();
        }
        enumerator = default(Dictionary<object, StandardTrackedObject>.ValueCollection.Enumerator);
      }
    }

    private class ReadOnlyChangeTracker : ChangeTracker {
      internal override TrackedObject Track(object obj) => null;

      internal override TrackedObject Track(object obj, bool recurse) => null;

      internal override void FastTrack(object obj) {
      }

      internal override bool IsTracked(object obj) => false;

      internal override TrackedObject GetTrackedObject(object obj) => null;

      internal override void StopTracking(object obj) {
      }

      internal override void AcceptChanges() {
      }

      internal override IEnumerable<TrackedObject> GetInterestingObjects() => new TrackedObject[0];
    }

    internal abstract TrackedObject Track(object obj);

    internal abstract TrackedObject Track(object obj, bool recurse);

    internal abstract void FastTrack(object obj);

    internal abstract bool IsTracked(object obj);

    internal abstract TrackedObject GetTrackedObject(object obj);

    internal abstract void StopTracking(object obj);

    internal abstract void AcceptChanges();

    internal abstract IEnumerable<TrackedObject> GetInterestingObjects();

    internal static ChangeTracker CreateChangeTracker(CommonDataServices dataServices, bool asReadOnly) {
      if (asReadOnly) {
        return new ReadOnlyChangeTracker();
      }
      return new StandardChangeTracker(dataServices);
    }
  }

}