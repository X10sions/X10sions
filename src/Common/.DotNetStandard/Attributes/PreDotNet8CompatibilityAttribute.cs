namespace Common.Attributes;

[Obsolete("introduced in .NET 8", false)]
/// <summary>For backwards compatibility, and should not be used by new applications.Provides access to APIs that were not available only until .NET 8.</summary>
public class PreDotNet8CompatibilityAttribute : PreDotNet6CompatibilityAttribute {
  public PreDotNet8CompatibilityAttribute() : base() { }
  public PreDotNet8CompatibilityAttribute(string recommedation) : base(recommedation) { }
}