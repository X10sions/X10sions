using System.Data;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped.DataRows;
public class ReservedWordRow : BaseTypedDataRow {
  public ReservedWordRow() { }
  public ReservedWordRow(DataRow dataRow) : base(dataRow) { }

  public override Dictionary<string, Action<DataRow>> GetColumnSetValueDictionary() {
    var dic = new Dictionary<string, Action<DataRow>>();
    dic[DbMetaDataColumnNames.ReservedWord] = dataRow => ReservedWord = dataRow.Field<string>(DbMetaDataColumnNames.ReservedWord);
    dic[nameof(MaximumVersion)] = dataRow => MaximumVersion = dataRow.Field<string?>(nameof(MaximumVersion));
    dic[nameof(MinimumVersion)] = dataRow => MinimumVersion = dataRow.Field<string?>(nameof(MinimumVersion));
    return dic;
  }

  public string ReservedWord { get; set; } = string.Empty;

  public string? MaximumVersion { get; set; }
  public string? MinimumVersion { get; set; }
}
