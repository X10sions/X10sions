namespace System.Data.Linq.Mapping {
  [Obsolete]
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
  public sealed class ColumnAttribute : DataAttribute {
    public AutoSync AutoSync { get; set; }
    public bool CanBeNull { get; set; } = true;
    public string DbType { get; set; }
    public string Expression { get; set; }
    public bool IsDbGenerated { get; set; }
    public bool IsDiscriminator { get; set; }
    public bool IsPrimaryKey { get; set; }
    public bool IsVersion { get; set; }
    public UpdateCheck UpdateCheck { get; set; }
  }
}