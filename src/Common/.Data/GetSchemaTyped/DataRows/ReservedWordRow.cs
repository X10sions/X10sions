using System.Data;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped.DataRows;
public class ReservedWordRow : ITypedDataRow {
  public ReservedWordRow() { }
  public ReservedWordRow(DataRow dataRow) {
    SetValues(dataRow);
  }

  public void SetValues(DataRow dataRow) {
    ReservedWord = dataRow.Field<string>(DbMetaDataColumnNames.ReservedWord);
    MaximumVersion = dataRow.Field<string?>(nameof(MaximumVersion));
    MinimumVersion = dataRow.Field<string?>(nameof(MinimumVersion));
  }

  public string ReservedWord { get; set; } = string.Empty;

  public string? MaximumVersion { get; set; }
  public string? MinimumVersion { get; set; }
}
