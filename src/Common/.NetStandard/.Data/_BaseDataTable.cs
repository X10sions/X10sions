using System;
using System.Collections;
using System.Data;

namespace Common.Data {
  public interface IBaseDataTable { }
  public interface IBaseDataRow { }

  public abstract class _BaseDataTable<TRow>
    : DataTable, IBaseDataTable where TRow : DataRow, IBaseDataRow {
    //protected DbDataAdapter _adapter;
    //    protected string _connectionString = TypedDataSet.Properties.Settings.Default.ConnectionString;

    //protected DbDataAdapter Adapter(string sql) {
    //  _adapter = new OdbcDataAdapter(sql, _connectionString);
    //  var cb = new OdbcCommandBuilder((OdbcDataAdapter)_adapter);
    //  return _adapter;
    //}

    public TRow this[int index] => (TRow)Rows[index];
    public void Add(TRow row) => Rows.Add(row);
    public void Remove(TRow row) => Rows.Remove(row);

    //public void Save() {
    //  _adapter.Update(this);
    //  AcceptChanges();
    //}

    public TRow GetNewRow() => (TRow)NewRow();

    public IEnumerator GetEnumerator() => Rows.GetEnumerator();
    protected override Type GetRowType() => typeof(TRow);
    protected abstract override DataRow NewRowFromBuilder(DataRowBuilder builder);

  }
}
