using System.Data;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped.DataRows;
public class RestrictionRow : ITypedDataRow {
  public RestrictionRow() { }
  public RestrictionRow(DataRow dataRow) {
    SetValues(dataRow);
  }

  public void SetValues(DataRow dataRow) {
    CollectionName = dataRow.Field<string>(DbMetaDataColumnNames.CollectionName);
    MaximumVersion = dataRow.Field<string?>(nameof(MaximumVersion));
    MinimumVersion = dataRow.Field<string?>(nameof(MinimumVersion));
    ParameterName = dataRow.Field<string?>(nameof(ParameterName));
    RestrictionName = dataRow.Field<string>(nameof(RestrictionName));
    RestrictionDefault = dataRow.Field<string>(nameof(RestrictionDefault));
    RestrictionNumber = dataRow.Field<int>(nameof(RestrictionNumber));
  }

  public string CollectionName { get; set; } = string.Empty;

  public string? MaximumVersion { get; set; }
  public string? MinimumVersion { get; set; }
  public string? ParameterName { get; set; }
  public string RestrictionName { get; set; } = string.Empty;
  public string RestrictionDefault { get; set; } = string.Empty;
  public int RestrictionNumber { get; set; }

}