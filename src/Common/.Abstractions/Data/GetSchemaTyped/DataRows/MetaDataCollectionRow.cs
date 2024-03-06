namespace Common.Data.GetSchemaTyped.DataRows {
  public class MetaDataCollectionRow : ITypedDataRow {
    public string CollectionName { get; set; } = string.Empty;
    public int NumberOfRestrictions { get; set; }
    public int NumberOfIdentifierParts { get; set; }
    public string? PopulationMechanism { get; set; }
    public string? PopulationString { get; set; }
    public string? MinimumVersion { get; set; }
    public string? MaximumVersion { get; set; }
  }
}