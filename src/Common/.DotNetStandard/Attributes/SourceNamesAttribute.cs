namespace Common.Attributes;


[AttributeUsage(AttributeTargets.Property)]
public class SourceNamesAttribute : Attribute {
  public SourceNamesAttribute() { }
  public SourceNamesAttribute(params string[] columnNames) {
    ColumnNames = columnNames.ToList();
  }
  public List<string> ColumnNames { get; set; } = new List<string>();
}
