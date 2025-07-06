namespace System.Data.Linq.Mapping;

public abstract class DataAttribute : Attribute {
  string name;
  string storage;
  protected DataAttribute() { }
  public string Name {
    get { return this.name; }
    set { name = value; }
  }
  public string Storage {
    get { return this.storage; }
    set { this.storage = value; }
  }
}
