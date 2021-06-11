namespace System.Data.Linq.Mapping {
  [Obsolete]
  public abstract class DataAttribute : Attribute {
    public string Name { get; set; }
    public string Storage { get; set; }
  }
}