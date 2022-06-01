using System.Data;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped.DataRows;
public class RestrictionRow : BaseTypedDataRow {
  public RestrictionRow() { }
  public RestrictionRow(DataRow dataRow) : base(dataRow) { }

  public override Dictionary<string, Action<DataRow>> GetColumnSetValueDictionary() {
    var dic = new Dictionary<string, Action<DataRow>>();
    dic[DbMetaDataColumnNames.CollectionName] = dataRow => CollectionName = dataRow.Field<string>(DbMetaDataColumnNames.CollectionName);
    dic[nameof(MaximumVersion)] = dataRow => MaximumVersion = dataRow.Field<string?>(nameof(MaximumVersion));
    dic[nameof(MinimumVersion)] = dataRow => MinimumVersion = dataRow.Field<string?>(nameof(MinimumVersion));
    dic[nameof(ParameterName)] = dataRow => ParameterName = dataRow.Field<string?>(nameof(ParameterName));
    dic[nameof(RestrictionName)] = dataRow => RestrictionName = dataRow.Field<string>(nameof(RestrictionName));
    dic[nameof(RestrictionDefault)] = dataRow => RestrictionDefault = dataRow.Field<string>(nameof(RestrictionDefault));
    dic[nameof(RestrictionNumber)] = dataRow => RestrictionNumber = dataRow.Field<int>(nameof(RestrictionNumber));
    return dic;
  } 

  public string CollectionName { get; set; } = string.Empty;

  public string? MaximumVersion { get; set; }
  public string? MinimumVersion { get; set; }
  public string? ParameterName { get; set; }
  public string RestrictionName { get; set; } = string.Empty;
  public string RestrictionDefault { get; set; } = string.Empty;
  public int RestrictionNumber { get; set; }

}