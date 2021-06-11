using System.Collections;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.Data.Linq {

  internal class ChangeProcessor {
    private enum VisitState {
      Before,
      After
    }

    private class EdgeMap {
      private Dictionary<MetaAssociation, Dictionary<TrackedObject, TrackedObject>> associations;

      internal TrackedObject this[MetaAssociation assoc, TrackedObject from] {
        get {
          if (associations.TryGetValue(assoc, out var value) && value.TryGetValue(from, out var value2)) {
            return value2;
          }
          return null;
        }
      }

      internal EdgeMap() {
        associations = new Dictionary<MetaAssociation, Dictionary<TrackedObject, TrackedObject>>();
      }

      internal void Add(MetaAssociation assoc, TrackedObject from, TrackedObject to) {
        if (!associations.TryGetValue(assoc, out var value)) {
          value = new Dictionary<TrackedObject, TrackedObject>();
          associations.Add(assoc, value);
        }
        value.Add(from, to);
      }

      internal void Clear() => associations.Clear();
    }

    private class ReferenceMap {
      private Dictionary<TrackedObject, List<TrackedObject>> references;

      private static TrackedObject[] Empty = new TrackedObject[0];

      internal IEnumerable<TrackedObject> this[TrackedObject from] {
        get {
          if (references.TryGetValue(from, out var value)) {
            return value;
          }
          return Empty;
        }
      }

      internal ReferenceMap() {
        references = new Dictionary<TrackedObject, List<TrackedObject>>();
      }

      internal void Add(TrackedObject from, TrackedObject to) {
        if (!references.TryGetValue(from, out var value)) {
          value = new List<TrackedObject>();
          references.Add(from, value);
        }
        if (!value.Contains(to)) {
          value.Add(to);
        }
      }

      internal void Clear() => references.Clear();
    }

    private CommonDataServices services;

    private DataContext context;

    private ChangeTracker tracker;

    private ChangeDirector changeDirector;

    private EdgeMap currentParentEdges;

    private EdgeMap originalChildEdges;

    private ReferenceMap originalChildReferences;

    internal ChangeProcessor(CommonDataServices services, DataContext context) {
      this.services = services;
      this.context = context;
      tracker = services.ChangeTracker;
      changeDirector = services.ChangeDirector;
      currentParentEdges = new EdgeMap();
      originalChildEdges = new EdgeMap();
      originalChildReferences = new ReferenceMap();
    }

    internal void SubmitChanges(ConflictMode failureMode) {
      TrackUntrackedObjects();
      ApplyInferredDeletions();
      BuildEdgeMaps();
      var orderedList = GetOrderedList();
      ValidateAll(orderedList);
      var num = 0;
      var session = new ChangeConflictSession(context);
      var list = new List<ObjectChangeConflict>();
      var list2 = new List<TrackedObject>();
      var list3 = new List<TrackedObject>();
      var list4 = new List<TrackedObject>();
      foreach (var item in orderedList) {
        try {
          if (item.IsNew) {
            if (item.SynchDependentData()) {
              list4.Add(item);
            }
            changeDirector.Insert(item);
            list3.Add(item);
          } else if (item.IsDeleted) {
            num++;
            if (changeDirector.Delete(item) == 0) {
              list.Add(new ObjectChangeConflict(session, item, false));
            } else {
              list2.Add(item);
            }
          } else if (item.IsPossiblyModified) {
            if (item.SynchDependentData()) {
              list4.Add(item);
            }
            if (item.IsModified) {
              CheckForInvalidChanges(item);
              num++;
              if (changeDirector.Update(item) <= 0) {
                list.Add(new ObjectChangeConflict(session, item));
              }
            }
          }
        } catch (ChangeConflictException) {
          list.Add(new ObjectChangeConflict(session, item));
        }
        if (list.Count > 0 && failureMode == ConflictMode.FailOnFirstConflict) {
          break;
        }
      }
      if (list.Count > 0) {
        changeDirector.RollbackAutoSync();
        foreach (var item2 in list4) {
          item2.SynchDependentData();
        }
        context.ChangeConflicts.Fill(list);
        throw CreateChangeConflictException(num, list.Count);
      }
      changeDirector.ClearAutoSyncRollback();
      PostProcessUpdates(list3, list2);
    }

    private void PostProcessUpdates(List<TrackedObject> insertedItems, List<TrackedObject> deletedItems) {
      foreach (var deletedItem in deletedItems) {
        services.RemoveCachedObjectLike(deletedItem.Type, deletedItem.Original);
        ClearForeignKeyReferences(deletedItem);
      }
      foreach (var insertedItem in insertedItems) {
        var obj = services.InsertLookupCachedObject(insertedItem.Type, insertedItem.Current);
        if (obj != insertedItem.Current) {
          throw new DuplicateKeyException(insertedItem.Current, System.Data.Linq.Strings.DatabaseGeneratedAlreadyExistingKey);
        }
        insertedItem.InitializeDeferredLoaders();
      }
    }

    private void ClearForeignKeyReferences(TrackedObject to) {
      foreach (var association in to.Type.Associations) {
        if (association.IsForeignKey) {
          if (association.OtherMember != null && association.OtherKeyIsPrimaryKey) {
            var foreignKeyValues = CommonDataServices.GetForeignKeyValues(association, to.Current);
            var instance = services.IdentityManager.Find(association.OtherType, foreignKeyValues);
            if (instance != null) {
              if (association.OtherMember.Association.IsMany) {
                var list = association.OtherMember.MemberAccessor.GetBoxedValue(instance) as IList;
                if (list != null && !list.IsFixedSize) {
                  list.Remove(to.Current);
                  ClearForeignKeysHelper(association, to.Current);
                }
              } else {
                association.OtherMember.MemberAccessor.SetBoxedValue(ref instance, null);
                ClearForeignKeysHelper(association, to.Current);
              }
            }
          } else {
            ClearForeignKeysHelper(association, to.Current);
          }
        }
      }
    }

    private static void ClearForeignKeysHelper(MetaAssociation assoc, object trackedInstance) {
      var thisMember = assoc.ThisMember;
      if (thisMember.IsDeferred && !thisMember.StorageAccessor.HasAssignedValue(trackedInstance) && !thisMember.StorageAccessor.HasLoadedValue(trackedInstance)) {
        thisMember.DeferredSourceAccessor.SetBoxedValue(ref trackedInstance, null);
      }
      thisMember.MemberAccessor.SetBoxedValue(ref trackedInstance, null);
      var i = 0;
      for (var count = assoc.ThisKey.Count; i < count; i++) {
        var metaDataMember = assoc.ThisKey[i];
        if (metaDataMember.CanBeNull) {
          metaDataMember.StorageAccessor.SetBoxedValue(ref trackedInstance, null);
        }
      }
    }

    private static void ValidateAll(IEnumerable<TrackedObject> list) {
      foreach (var item in list) {
        if (item.IsNew) {
          item.SynchDependentData();
          if (item.Type.HasAnyValidateMethod) {
            SendOnValidate(item.Type, item, ChangeAction.Insert);
          }
        } else if (item.IsDeleted) {
          if (item.Type.HasAnyValidateMethod) {
            SendOnValidate(item.Type, item, ChangeAction.Delete);
          }
        } else if (item.IsPossiblyModified) {
          item.SynchDependentData();
          if (item.IsModified && item.Type.HasAnyValidateMethod) {
            SendOnValidate(item.Type, item, ChangeAction.Update);
          }
        }
      }
    }

    private static void SendOnValidate(MetaType type, TrackedObject item, ChangeAction changeAction) {
      if (type != null) {
        SendOnValidate(type.InheritanceBase, item, changeAction);
        if (type.OnValidateMethod != null) {
          try {
            type.OnValidateMethod.Invoke(item.Current, new object[1]
            {
            changeAction
            });
          } catch (TargetInvocationException ex) {
            if (ex.InnerException != null) {
              throw ex.InnerException;
            }
            throw;
          }
        }
      }
    }

    internal string GetChangeText() {
      ObserveUntrackedObjects();
      ApplyInferredDeletions();
      BuildEdgeMaps();
      var stringBuilder = new StringBuilder();
      foreach (var ordered in GetOrderedList()) {
        if (ordered.IsNew) {
          ordered.SynchDependentData();
          changeDirector.AppendInsertText(ordered, stringBuilder);
        } else if (ordered.IsDeleted) {
          changeDirector.AppendDeleteText(ordered, stringBuilder);
        } else if (ordered.IsPossiblyModified) {
          ordered.SynchDependentData();
          if (ordered.IsModified) {
            changeDirector.AppendUpdateText(ordered, stringBuilder);
          }
        }
      }
      return stringBuilder.ToString();
    }

    internal ChangeSet GetChangeSet() {
      var list = new List<object>();
      var list2 = new List<object>();
      var list3 = new List<object>();
      ObserveUntrackedObjects();
      ApplyInferredDeletions();
      foreach (var interestingObject in tracker.GetInterestingObjects()) {
        if (interestingObject.IsNew) {
          interestingObject.SynchDependentData();
          list.Add(interestingObject.Current);
        } else if (interestingObject.IsDeleted) {
          list2.Add(interestingObject.Current);
        } else if (interestingObject.IsPossiblyModified) {
          interestingObject.SynchDependentData();
          if (interestingObject.IsModified) {
            list3.Add(interestingObject.Current);
          }
        }
      }
      return new ChangeSet(list.AsReadOnly(), list2.AsReadOnly(), list3.AsReadOnly());
    }

    private static void CheckForInvalidChanges(TrackedObject tracked) {
      foreach (var persistentDataMember in tracked.Type.PersistentDataMembers) {
        if ((persistentDataMember.IsPrimaryKey || persistentDataMember.IsDbGenerated || persistentDataMember.IsVersion) && tracked.HasChangedValue(persistentDataMember)) {
          if (persistentDataMember.IsPrimaryKey) {
            throw System.Data.Linq.Error.IdentityChangeNotAllowed(persistentDataMember.Name, tracked.Type.Name);
          }
          throw System.Data.Linq.Error.DbGeneratedChangeNotAllowed(persistentDataMember.Name, tracked.Type.Name);
        }
      }
    }

    private static ChangeConflictException CreateChangeConflictException(int totalUpdatesAttempted, int failedUpdates) {
      var message = System.Data.Linq.Strings.RowNotFoundOrChanged;
      if (totalUpdatesAttempted > 1) {
        message = System.Data.Linq.Strings.UpdatesFailedMessage(failedUpdates, totalUpdatesAttempted);
      }
      return new ChangeConflictException(message);
    }

    internal void TrackUntrackedObjects() {
      var visited = new Dictionary<object, object>();
      var list = new List<TrackedObject>(tracker.GetInterestingObjects());
      foreach (var item in list) {
        TrackUntrackedObjects(item.Type, item.Current, visited);
      }
    }

    internal void ApplyInferredDeletions() {
      foreach (var interestingObject in tracker.GetInterestingObjects()) {
        if (interestingObject.CanInferDelete()) {
          if (interestingObject.IsNew) {
            interestingObject.ConvertToRemoved();
          } else if (interestingObject.IsPossiblyModified || interestingObject.IsModified) {
            interestingObject.ConvertToDeleted();
          }
        }
      }
    }

    private void TrackUntrackedObjects(MetaType type, object item, Dictionary<object, object> visited) {
      if (!visited.ContainsKey(item)) {
        visited.Add(item, item);
        var trackedObject = tracker.GetTrackedObject(item);
        if (trackedObject == null) {
          trackedObject = tracker.Track(item);
          trackedObject.ConvertToNew();
        } else if (trackedObject.IsDead || trackedObject.IsRemoved) {
          return;
        }
        foreach (var parent in services.GetParents(type, item)) {
          TrackUntrackedObjects(parent.Type, parent.Item, visited);
        }
        if (trackedObject.IsNew) {
          trackedObject.InitializeDeferredLoaders();
          if (!trackedObject.IsPendingGeneration(trackedObject.Type.IdentityMembers)) {
            trackedObject.SynchDependentData();
            var obj = services.InsertLookupCachedObject(trackedObject.Type, item);
            if (obj != item) {
              var trackedObject2 = tracker.GetTrackedObject(obj);
              if (trackedObject2.IsDeleted || trackedObject2.CanInferDelete()) {
                trackedObject.ConvertToPossiblyModified(trackedObject2.Original);
                trackedObject2.ConvertToDead();
                services.RemoveCachedObjectLike(trackedObject.Type, item);
                services.InsertLookupCachedObject(trackedObject.Type, item);
              } else if (!trackedObject2.IsDead) {
                throw new DuplicateKeyException(item, System.Data.Linq.Strings.CantAddAlreadyExistingKey);
              }
            }
          } else {
            var cachedObjectLike = services.GetCachedObjectLike(trackedObject.Type, item);
            if (cachedObjectLike != null) {
              var trackedObject3 = tracker.GetTrackedObject(cachedObjectLike);
              if (trackedObject3.IsDeleted || trackedObject3.CanInferDelete()) {
                trackedObject.ConvertToPossiblyModified(trackedObject3.Original);
                trackedObject3.ConvertToDead();
                services.RemoveCachedObjectLike(trackedObject.Type, item);
                services.InsertLookupCachedObject(trackedObject.Type, item);
              }
            }
          }
        }
        foreach (var child in services.GetChildren(type, item)) {
          TrackUntrackedObjects(child.Type, child.Item, visited);
        }
      }
    }

    internal void ObserveUntrackedObjects() {
      var visited = new Dictionary<object, object>();
      var list = new List<TrackedObject>(tracker.GetInterestingObjects());
      foreach (var item in list) {
        ObserveUntrackedObjects(item.Type, item.Current, visited);
      }
    }

    private void ObserveUntrackedObjects(MetaType type, object item, Dictionary<object, object> visited) {
      if (!visited.ContainsKey(item)) {
        visited.Add(item, item);
        var trackedObject = tracker.GetTrackedObject(item);
        if (trackedObject == null) {
          trackedObject = tracker.Track(item);
          trackedObject.ConvertToNew();
        } else if (trackedObject.IsDead || trackedObject.IsRemoved) {
          return;
        }
        foreach (var parent in services.GetParents(type, item)) {
          ObserveUntrackedObjects(parent.Type, parent.Item, visited);
        }
        if (trackedObject.IsNew && !trackedObject.IsPendingGeneration(trackedObject.Type.IdentityMembers)) {
          trackedObject.SynchDependentData();
        }
        foreach (var child in services.GetChildren(type, item)) {
          ObserveUntrackedObjects(child.Type, child.Item, visited);
        }
      }
    }

    private TrackedObject GetOtherItem(MetaAssociation assoc, object instance) {
      if (instance == null) {
        return null;
      }
      object obj = null;
      if (assoc.ThisMember.StorageAccessor.HasAssignedValue(instance) || assoc.ThisMember.StorageAccessor.HasLoadedValue(instance)) {
        obj = assoc.ThisMember.MemberAccessor.GetBoxedValue(instance);
      } else if (assoc.OtherKeyIsPrimaryKey) {
        var foreignKeyValues = CommonDataServices.GetForeignKeyValues(assoc, instance);
        obj = services.GetCachedObject(assoc.OtherType, foreignKeyValues);
      }
      if (obj == null) {
        return null;
      }
      return tracker.GetTrackedObject(obj);
    }

    private bool HasAssociationChanged(MetaAssociation assoc, TrackedObject item) {
      if (item.Original != null && item.Current != null) {
        if (assoc.ThisMember.StorageAccessor.HasAssignedValue(item.Current) || assoc.ThisMember.StorageAccessor.HasLoadedValue(item.Current)) {
          return GetOtherItem(assoc, item.Current) != GetOtherItem(assoc, item.Original);
        }
        var foreignKeyValues = CommonDataServices.GetForeignKeyValues(assoc, item.Current);
        var foreignKeyValues2 = CommonDataServices.GetForeignKeyValues(assoc, item.Original);
        var i = 0;
        for (var num = foreignKeyValues.Length; i < num; i++) {
          if (!object.Equals(foreignKeyValues[i], foreignKeyValues2[i])) {
            return true;
          }
        }
      }
      return false;
    }

    private void BuildEdgeMaps() {
      currentParentEdges.Clear();
      originalChildEdges.Clear();
      originalChildReferences.Clear();
      var list = new List<TrackedObject>(tracker.GetInterestingObjects());
      foreach (var item in list) {
        var isNew = item.IsNew;
        var type = item.Type;
        foreach (var association in type.Associations) {
          if (association.IsForeignKey) {
            var otherItem = GetOtherItem(association, item.Current);
            var otherItem2 = GetOtherItem(association, item.Original);
            var flag = (otherItem != null && otherItem.IsDeleted) || (otherItem2?.IsDeleted ?? false);
            var flag2 = otherItem?.IsNew ?? false;
            if ((isNew | flag | flag2) || HasAssociationChanged(association, item)) {
              if (otherItem != null) {
                currentParentEdges.Add(association, item, otherItem);
              }
              if (otherItem2 != null) {
                if (association.IsUnique) {
                  originalChildEdges.Add(association, otherItem2, item);
                }
                originalChildReferences.Add(otherItem2, item);
              }
            }
          }
        }
      }
    }

    private List<TrackedObject> GetOrderedList() {
      var objects = tracker.GetInterestingObjects().ToList();
      var list = Enumerable.Range(0, objects.Count).ToList();
      list.Sort((int x, int y) => Compare(objects[x], x, objects[y], y));
      var list2 = (from i in list
                   select objects[i]).ToList();
      var visited = new Dictionary<TrackedObject, VisitState>();
      var list3 = new List<TrackedObject>();
      foreach (var item in list2) {
        BuildDependencyOrderedList(item, list3, visited);
      }
      return list3;
    }

    private static int Compare(TrackedObject x, int xOrdinal, TrackedObject y, int yOrdinal) {
      if (x == y) {
        return 0;
      }
      if (x == null) {
        return -1;
      }
      if (y == null) {
        return 1;
      }
      var num = (!x.IsNew) ? ((!x.IsDeleted) ? 1 : 2) : 0;
      var num2 = (!y.IsNew) ? ((!y.IsDeleted) ? 1 : 2) : 0;
      if (num < num2) {
        return -1;
      }
      if (num > num2) {
        return 1;
      }
      if (x.IsNew) {
        return xOrdinal.CompareTo(yOrdinal);
      }
      if (x.Type != y.Type) {
        return string.CompareOrdinal(x.Type.Type.FullName, y.Type.Type.FullName);
      }
      var num3 = 0;
      foreach (var identityMember in x.Type.IdentityMembers) {
        var boxedValue = identityMember.StorageAccessor.GetBoxedValue(x.Current);
        var boxedValue2 = identityMember.StorageAccessor.GetBoxedValue(y.Current);
        if (boxedValue == null) {
          if (boxedValue2 != null) {
            return -1;
          }
        } else {
          var comparable = boxedValue as IComparable;
          if (comparable != null) {
            num3 = comparable.CompareTo(boxedValue2);
            if (num3 != 0) {
              return num3;
            }
          }
        }
      }
      return xOrdinal.CompareTo(yOrdinal);
    }

    private void BuildDependencyOrderedList(TrackedObject item, List<TrackedObject> list, Dictionary<TrackedObject, VisitState> visited) {
      if (visited.TryGetValue(item, out var value)) {
        if (value == VisitState.Before) {
          throw System.Data.Linq.Error.CycleDetected();
        }
      } else {
        visited[item] = VisitState.Before;
        if (item.IsInteresting) {
          if (item.IsDeleted) {
            foreach (var item2 in originalChildReferences[item]) {
              if (item2 != item) {
                BuildDependencyOrderedList(item2, list, visited);
              }
            }
          } else {
            foreach (var association in item.Type.Associations) {
              if (association.IsForeignKey) {
                var trackedObject = currentParentEdges[association, item];
                if (trackedObject != null) {
                  if (trackedObject.IsNew) {
                    if (trackedObject != item || item.Type.DBGeneratedIdentityMember != null) {
                      BuildDependencyOrderedList(trackedObject, list, visited);
                    }
                  } else if (association.IsUnique || association.ThisKeyIsPrimaryKey) {
                    var trackedObject2 = originalChildEdges[association, trackedObject];
                    if (trackedObject2 != null && trackedObject != item) {
                      BuildDependencyOrderedList(trackedObject2, list, visited);
                    }
                  }
                }
              }
            }
          }
          list.Add(item);
        }
        visited[item] = VisitState.After;
      }
    }
  }

}