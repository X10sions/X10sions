using System;
using System.Collections;
using System.Data;

namespace Common.Data {
  //public interface IDataTable { }
  public interface ITypedDataTable<TRow> where TRow : DataRow{
    // protected TRow NewRowFromBuilderTyped(DataRowBuilder builder);
    public TRow NewRowTyped();
  }

  public static class BaseTypedDataTableExtensions {
    public static TRow NewRowTyped<TRow>(this DataTable dataTable) where TRow : DataRow => (TRow)dataTable.NewRow() ;
  }

  public abstract class BaseTypedDataTable<TRow> : DataTable, ITypedDataTable<TRow> where TRow : DataRow{
    // https://www.codeproject.com/Articles/30490/How-to-Manually-Create-a-Typed-DataTable
    protected override DataRow NewRowFromBuilder(DataRowBuilder builder) => NewRowFromBuilderTyped(builder);
    protected abstract TRow NewRowFromBuilderTyped(DataRowBuilder builder);

    protected override Type GetRowType() => typeof(TRow);
    
    //public DataRowCollection TypedRows = new DataRowCollection();
    public IEnumerator GetEnumerator() => Rows.GetEnumerator();

    public TRow this[int index] => index >= 0 & index <= Rows.Count - 1
          ? (TRow)Rows[index]
          : throw new ArgumentOutOfRangeException("index", index, "is't in your " + TableName + " table");

    public TRow NewRowTyped() => this.NewRowTyped<TRow>();

    protected override void OnRowChanged(DataRowChangeEventArgs e) {
      base.OnRowChanged(e);
      TypedRowChangedEventArgs args = new TypedRowChangedEventArgs(e.Action, (TRow)e.Row);
      OnTypedRowChanged(args);
    }

    protected virtual void OnTypedRowChanged(TypedRowChangedEventArgs args) {
      if (TypedRowChanged != null) {
        TypedRowChanged(this, args);
      }
    }

    public delegate void TypedRowChangedDelegate(BaseTypedDataTable<TRow> sender, TypedRowChangedEventArgs args);
    public event TypedRowChangedDelegate TypedRowChanged;

    public class TypedRowChangedEventArgs {
      public TypedRowChangedEventArgs(DataRowAction action, TRow row) {
        Action = action;
        Row = row;
      }
      public DataRowAction Action { get; }
      public TRow Row { get; }

    }

    //protected DbDataAdapter _adapter;
    //    protected string _connectionString = TypedDataSet.Properties.Settings.Default.ConnectionString;

    //protected DbDataAdapter Adapter(string sql) {
    //  _adapter = new OdbcDataAdapter(sql, _connectionString);
    //  var cb = new OdbcCommandBuilder((OdbcDataAdapter)_adapter);
    //  return _adapter;
    //}

    //public void Save() {
    //  _adapter.Update(this);
    //  AcceptChanges();
    //}

  }
 }
