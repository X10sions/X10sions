namespace Common.Attributes;

[Obsolete("introduced in .NET 6", false)]

/// <summary>For backwards compatibility, and should not be used by new applications.Provides access to APIs that were not available only until .NET 6.</summary>
public class PreDotNet6CompatibilityAttribute : PreDotNet5CompatibilityAttribute {
  public PreDotNet6CompatibilityAttribute() : base() { }
  public PreDotNet6CompatibilityAttribute(string recommedation) : base(recommedation) { }
}
