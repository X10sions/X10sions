namespace System.Data.Linq.Mapping;
/// <summary>
/// Column Attribute
/// </summary>
/// <seealso cref="System.Attribute" />
[Obsolete]
public sealed class ColumnAttribute : Attribute {
  public string Name { get; set; }
  public string Storage { get; set; }
  public bool CanBeNull { get; set; }
  public string DbType { get; set; }
  public string Expression { get; set; }
  public bool IsDbGenerated { get; set; }
  public bool IsDiscriminator { get; set; }
  public bool IsPrimaryKey { get; set; }
  public bool IsVersion { get; set; }
}