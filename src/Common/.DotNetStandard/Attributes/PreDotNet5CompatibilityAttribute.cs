namespace Common.Attributes;

[Obsolete("introduced in .NET 5", false)]

/// <summary>For backwards compatibility, and should not be used by new applications.Provides access to APIs that were not available only until .NET 6.</summary>
public class PreDotNet5CompatibilityAttribute : Attribute {
  public PreDotNet5CompatibilityAttribute() { }
  public PreDotNet5CompatibilityAttribute(string recommedation) : this() { }
}