using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq.Mapping;

/// <summary>
/// Attribute placed on a method mapped to a User Defined Function.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class FunctionAttribute : Attribute {
  string name;
  bool isComposable;
  public FunctionAttribute() {
  }
  public string Name {
    get { return this.name; }
    set { this.name = value; }
  }
  [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Composable", Justification = "Spelling is correct.")]
  public bool IsComposable {
    get { return this.isComposable; }
    set { this.isComposable = value; }
  }
}
