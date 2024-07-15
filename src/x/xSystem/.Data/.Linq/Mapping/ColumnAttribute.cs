using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq.Mapping;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public sealed class ColumnAttribute : DataAttribute {
  string dbtype;
  string expression;
  bool isPrimaryKey;
  bool isDBGenerated;
  bool isVersion;
  bool isDiscriminator;
  bool canBeNull = true;
  UpdateCheck check;
  AutoSync autoSync = AutoSync.Default;
  bool canBeNullSet = false;

  public ColumnAttribute() {
    check = UpdateCheck.Always;
  }
  [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Db", Justification = "Conforms to legacy spelling.")]
  public string DbType {
    get { return this.dbtype; }
    set { this.dbtype = value; }
  }
  public string Expression {
    get { return this.expression; }
    set { this.expression = value; }
  }
  public bool IsPrimaryKey {
    get { return this.isPrimaryKey; }
    set { this.isPrimaryKey = value; }
  }
  [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Db", Justification = "Conforms to legacy spelling.")]
  public bool IsDbGenerated {
    get { return this.isDBGenerated; }
    set { this.isDBGenerated = value; }
  }
  public bool IsVersion {
    get { return this.isVersion; }
    set { this.isVersion = value; }
  }
  public UpdateCheck UpdateCheck {
    get { return this.check; }
    set { this.check = value; }
  }
  public AutoSync AutoSync {
    get { return this.autoSync; }
    set { this.autoSync = value; }
  }
  public bool IsDiscriminator {
    get { return this.isDiscriminator; }
    set { isDiscriminator = value; }
  }
  public bool CanBeNull {
    get { return this.canBeNull; }
    set {
      this.canBeNullSet = true;
      this.canBeNull = value;
    }
  }
  internal bool CanBeNullSet {
    get { return this.canBeNullSet; }
  }
}
