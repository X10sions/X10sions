using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace System.Data.Linq {
  public sealed class ChangeSet {
    private ReadOnlyCollection<object> inserts;
    private ReadOnlyCollection<object> deletes;
    private ReadOnlyCollection<object> updates;
    public IList<object> Inserts => inserts;
    public IList<object> Deletes => deletes;
    public IList<object> Updates => updates;

    internal ChangeSet(ReadOnlyCollection<object> inserts, ReadOnlyCollection<object> deletes, ReadOnlyCollection<object> updates) {
      this.inserts = inserts;
      this.deletes = deletes;
      this.updates = updates;
    }

    public override string ToString() {
      return "{" + string.Format(CultureInfo.InvariantCulture, "Inserts: {0}, Deletes: {1}, Updates: {2}", new object[3] {
      Inserts.Count,
      Deletes.Count,
      Updates.Count
      }) + "}";
    }
  }

}