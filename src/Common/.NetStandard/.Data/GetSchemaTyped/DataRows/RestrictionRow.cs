using System.Data;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped.DataRows;
public class RestrictionRow : BaseTypedDataRow {
  public RestrictionRow() { }
  public RestrictionRow(DataRow dataRow):base(dataRow) { }

  public override void SetValues(DataRow dataRow) {
    CollectionName = dataRow.Field<string>(DbMetaDataColumnNames.CollectionName);
    RestrictionName = dataRow.Field<string>(nameof(RestrictionName));
    RestrictionDefault = dataRow.Field<string>(nameof(RestrictionDefault));
    RestrictionNumber = dataRow.Field<int>(nameof(RestrictionNumber));
  }

  public string CollectionName { get; set; } = string.Empty;
  public string RestrictionName { get; set; } = string.Empty;
  public string RestrictionDefault { get; set; } = string.Empty;
  public int RestrictionNumber { get; set; }

}