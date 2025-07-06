namespace System.Data.Linq.Mapping;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class TableAttribute : Attribute {
  string name;
  public TableAttribute() {
  }
  public string Name {
    get { return this.name; }
    set { this.name = value; }
  }
}
