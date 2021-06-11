using System.Collections;
using System.Collections.Generic;

namespace System.Data.Linq {
  public sealed class ChangeConflictCollection : ICollection<ObjectChangeConflict>, IEnumerable<ObjectChangeConflict>, IEnumerable, ICollection {
    private List<ObjectChangeConflict> conflicts;

    public int Count => conflicts.Count;

    public ObjectChangeConflict this[int index] => conflicts[index];

    bool ICollection<ObjectChangeConflict>.IsReadOnly => true;

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot => null;

    internal ChangeConflictCollection() {
      conflicts = new List<ObjectChangeConflict>();
    }

    void ICollection<ObjectChangeConflict>.Add(ObjectChangeConflict item) => throw Error.CannotAddChangeConflicts();

    public bool Remove(ObjectChangeConflict item) => conflicts.Remove(item);

    public void Clear() => conflicts.Clear();

    public bool Contains(ObjectChangeConflict item) => conflicts.Contains(item);

    public void CopyTo(ObjectChangeConflict[] array, int arrayIndex) => conflicts.CopyTo(array, arrayIndex);

    public IEnumerator<ObjectChangeConflict> GetEnumerator() => conflicts.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => conflicts.GetEnumerator();

    void ICollection.CopyTo(Array array, int index) => ((ICollection)conflicts).CopyTo(array, index);

    public void ResolveAll(RefreshMode mode) => ResolveAll(mode, true);

    public void ResolveAll(RefreshMode mode, bool autoResolveDeletes) {
      foreach (var conflict in conflicts) {
        if (!conflict.IsResolved) {
          conflict.Resolve(mode, autoResolveDeletes);
        }
      }
    }

    internal void Fill(List<ObjectChangeConflict> conflictList) => conflicts = conflictList;
  }

}