using System.Data;

namespace Common.Data;

public interface ITypedDataRow {
  public void SetValues(DataRow dataRow);
}

public abstract class BaseTypedDataRow : ITypedDataRow {
  public BaseTypedDataRow() { }

  public BaseTypedDataRow(DataRow dataRow) {
    SetValues(dataRow);
  }

  public abstract Dictionary<string, Action<DataRow>> GetColumnSetValueDictionary();
  //public virtual Dictionary<string, string> GetColumnNameAliasDictionary() => new Dictionary<string, string>();

  //public Dictionary<string, object?> OtherColumns { get; } = new Dictionary<string, object?>();

  public void SetValues(DataRow dataRow) {
    //var aliasDic = GetColumnNameAliasDictionary();
    var dic = GetColumnSetValueDictionary();
    foreach (DataColumn c in dataRow.Table.Columns) {
      var columnName = c.ColumnName;
      //if (aliasDic.ContainsKey(columnName)) columnName = aliasDic[columnName];
      var columnExists = dic.TryGetValue(columnName, out var setColumnValue);
      if (columnExists) {
        setColumnValue(dataRow);
        dic.Remove(columnName);
      } else {
        throw new Exception($"Unknown Column: {c.ColumnName} ({c.DataType.Name}{(c.AllowDBNull ? "?" : string.Empty)})");
        //OtherColumns.Add(columnName, dataRow.Field<object?>(columnName));
      }
    }
  }

}