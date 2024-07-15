using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq.Mapping;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class ProviderAttribute : Attribute {
  Type providerType;

  public ProviderAttribute() {
  }

  public ProviderAttribute(Type type) {
    this.providerType = type;
  }

  [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "The contexts in which this is available are fairly specific.")]
  public Type Type {
    get { return this.providerType; }
  }
}