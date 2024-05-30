using System.Data;

namespace Common.Data;

public abstract class BaseTypedDataRow : ITypedDataRow {
  public BaseTypedDataRow() { }

  public BaseTypedDataRow(DataRow dataRow) {
    SetValues(dataRow);
  }

  public abstract Dictionary<string, Action<DataRow>> GetColumnSetValueDictionary();

  public void SetValues(DataRow dataRow) {
    var dic = GetColumnSetValueDictionary();
    foreach (DataColumn c in dataRow.Table.Columns) {
      var columnName = c.ColumnName;
      var columnExists = dic.TryGetValue(columnName, out var setColumnValue);
      if (columnExists) {
        setColumnValue(dataRow);
        dic.Remove(columnName);
      } else {
        throw new Exception($"Unknown Column: {c.ColumnName} ({c.DataType.Name}{(c.AllowDBNull ? "?" : string.Empty)})");
      }
    }
  }

}