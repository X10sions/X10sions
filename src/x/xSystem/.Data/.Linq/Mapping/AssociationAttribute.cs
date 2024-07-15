namespace System.Data.Linq.Mapping;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public sealed class AssociationAttribute : DataAttribute {
  string thisKey;
  string otherKey;
  bool isUnique;
  bool isForeignKey;
  bool deleteOnNull;
  string deleteRule;

  public AssociationAttribute() { }
  public string ThisKey {
    get { return this.thisKey; }
    set { this.thisKey = value; }
  }
  public string OtherKey {
    get { return this.otherKey; }
    set { this.otherKey = value; }
  }
  public bool IsUnique {
    get { return this.isUnique; }
    set { this.isUnique = value; }
  }
  public bool IsForeignKey {
    get { return this.isForeignKey; }
    set { this.isForeignKey = value; }
  }
  public string DeleteRule {
    get { return this.deleteRule; }
    set { this.deleteRule = value; }
  }
  public bool DeleteOnNull {
    get { return this.deleteOnNull; }
    set { this.deleteOnNull = value; }
  }
}
