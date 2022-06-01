using System.Data;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped.DataRows {
  public class MetaDataCollectionRow: BaseTypedDataRow {
    public MetaDataCollectionRow() { }

    public MetaDataCollectionRow(DataRow dataRow) :base(dataRow) { }

    public override Dictionary<string, Action<DataRow>> GetColumnSetValueDictionary() {
      var dic = new Dictionary<string, Action<DataRow>>();
      dic[DbMetaDataColumnNames.CollectionName] = dataRow => CollectionName = dataRow.Field<string>(DbMetaDataColumnNames.CollectionName);
      dic[DbMetaDataColumnNames.NumberOfRestrictions] = dataRow => NumberOfRestrictions = dataRow.Field<int>(DbMetaDataColumnNames.NumberOfRestrictions);
      dic[DbMetaDataColumnNames.NumberOfIdentifierParts] = dataRow => NumberOfIdentifierParts = dataRow.Field<int>(DbMetaDataColumnNames.NumberOfIdentifierParts);

      dic[nameof(PopulationMechanism)] = dataRow => PopulationMechanism = dataRow.Field<string?>(nameof(PopulationMechanism));
      dic[nameof(PopulationString)] = dataRow => PopulationString = dataRow.Field<string?>(nameof(PopulationString));
      dic[nameof(MinimumVersion)] = dataRow => MinimumVersion = dataRow.Field<string?>(nameof(MinimumVersion));
      dic[nameof(MaximumVersion)] = dataRow => MaximumVersion = dataRow.Field<string?>(nameof(MaximumVersion));
      return dic;
    }

    public string CollectionName { get; set; } = string.Empty;
    public int NumberOfRestrictions { get; set; }
    public int NumberOfIdentifierParts { get; set; }

    public string? PopulationMechanism { get; set; }
    public string? PopulationString { get; set; }
    public string? MinimumVersion { get; set; }
    public string? MaximumVersion { get; set; }

  }
}