namespace Common.Data.GetSchemaTyped.DataRows;
public class ReservedWordRow : ITypedDataRow {
  public string ReservedWord { get; set; } = string.Empty;
  public string? MaximumVersion { get; set; }
  public string? MinimumVersion { get; set; }
}
