using System.Data;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped.DataRows {
  public class MetaDataCollectionRow : ITypedDataRow {
    public MetaDataCollectionRow() { }

    public MetaDataCollectionRow(DataRow dataRow) {
      SetValues(dataRow);
    }

    public void SetValues(DataRow dataRow) {
      CollectionName = dataRow.Field<string>(DbMetaDataColumnNames.CollectionName);
      NumberOfRestrictions = dataRow.Field<int>(DbMetaDataColumnNames.NumberOfRestrictions);
      NumberOfIdentifierParts = dataRow.Field<int>(DbMetaDataColumnNames.NumberOfIdentifierParts);

      PopulationMechanism = dataRow.Field<string?>(nameof(PopulationMechanism));
      PopulationString = dataRow.Field<string?>(nameof(PopulationString));
      MinimumVersion = dataRow.Field<string?>(nameof(MinimumVersion));
      MaximumVersion = dataRow.Field<string?>(nameof(MaximumVersion));
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