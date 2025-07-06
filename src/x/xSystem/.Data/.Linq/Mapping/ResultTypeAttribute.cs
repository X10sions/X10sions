using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq.Mapping;

/// <summary>
/// This attribute is applied to functions returning multiple result types,
/// to declare the possible result types returned from the function.  For
/// inheritance types, only the root type of the inheritance hierarchy need
/// be specified.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class ResultTypeAttribute : Attribute {
  Type type;
  public ResultTypeAttribute(Type type) {
    this.type = type;
  }
  [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "The contexts in which this is available are fairly specific.")]
  public Type Type {
    get { return this.type; }
  }
}
