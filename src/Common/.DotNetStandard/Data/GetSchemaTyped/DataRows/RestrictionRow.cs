namespace Common.Data.GetSchemaTyped.DataRows;
public class RestrictionRow : ITypedDataRow {
  public string CollectionName { get; set; } = string.Empty;
  public string? MaximumVersion { get; set; }
  public string? MinimumVersion { get; set; }
  public string? ParameterName { get; set; }
  public string RestrictionName { get; set; } = string.Empty;
  public string RestrictionDefault { get; set; } = string.Empty;
  public int RestrictionNumber { get; set; }

}