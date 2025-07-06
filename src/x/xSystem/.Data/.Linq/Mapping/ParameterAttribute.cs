using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq.Mapping;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = false)]
public sealed class ParameterAttribute : Attribute {
  string name;
  string dbType;
  public ParameterAttribute() {
  }
  public string Name {
    get { return this.name; }
    set { this.name = value; }
  }
  [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Db", Justification = "Conforms to legacy spelling.")]
  public string DbType {
    get { return this.dbType; }
    set { this.dbType = value; }
  }
}