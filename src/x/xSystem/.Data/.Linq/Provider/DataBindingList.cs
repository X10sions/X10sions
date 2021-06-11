using System.Collections.Generic;

namespace System.Data.Linq.Provider {
  internal class DataBindingList<TEntity> : SortableBindingList<TEntity> where TEntity : class {
    private readonly Table<TEntity> data;
    private TEntity addNewInstance;
    private TEntity cancelNewInstance;
    private bool addingNewInstance;

    internal DataBindingList(IList<TEntity> sequence, Table<TEntity> data)
      : base(sequence ?? new List<TEntity>()) {
      if (sequence == null) {
        throw Error.ArgumentNull(nameof(sequence));
      }
      this.data = data ?? throw Error.ArgumentNull(nameof(data));
    }

    protected override object AddNewCore() {
      addingNewInstance = true;
      addNewInstance = (TEntity)base.AddNewCore();
      return addNewInstance;
    }

    protected override void InsertItem(int index, TEntity item) {
      base.InsertItem(index, item);
      if (!addingNewInstance && index >= 0 && index <= Count) {
        data.InsertOnSubmit(item);
      }
    }

    protected override void RemoveItem(int index) {
      if (index >= 0 && index < Count && base[index] == cancelNewInstance) {
        cancelNewInstance = null;
      } else {
        data.DeleteOnSubmit(base[index]);
      }
      base.RemoveItem(index);
    }

    protected override void SetItem(int index, TEntity item) {
      var val = base[index];
      base.SetItem(index, item);
      if (index >= 0 && index < Count) {
        if (val == addNewInstance) {
          addNewInstance = null;
          addingNewInstance = false;
        } else {
          data.DeleteOnSubmit(val);
        }
        data.InsertOnSubmit(item);
      }
    }

    protected override void ClearItems() {
      data.DeleteAllOnSubmit(data.ToList());
      base.ClearItems();
    }

    public override void EndNew(int itemIndex) {
      if (itemIndex >= 0 && itemIndex < Count && base[itemIndex] == addNewInstance) {
        data.InsertOnSubmit(addNewInstance);
        addNewInstance = null;
        addingNewInstance = false;
      }
      base.EndNew(itemIndex);
    }

    public override void CancelNew(int itemIndex) {
      if (itemIndex >= 0 && itemIndex < Count && base[itemIndex] == addNewInstance) {
        cancelNewInstance = addNewInstance;
        addNewInstance = null;
        addingNewInstance = false;
      }
      base.CancelNew(itemIndex);
    }
  }
}