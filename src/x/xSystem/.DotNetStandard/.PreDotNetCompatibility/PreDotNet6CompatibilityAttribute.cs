#if !NET

namespace System;

[Obsolete("introduced in .NET 5, 6, 7, 8")]
/// <summary>For backwards compatibility, and should not be used by new applications.Provides access to APIs that were not available only until .NET 5, 6, 7, or 8.</summary>
public class PreDotNetCompatibilityAttribute(string SourceUrl) : Attribute {
  public string SourceUrl { get; } = SourceUrl;
}

//[Obsolete("introduced in .NET 6")] 
///// <summary>For backwards compatibility, and should not be used by new applications.Provides access to APIs that were not available only until .NET 6.</summary>
//public class PreDotNet6CompatibilityAttribute : Attribute { }


//[Obsolete("introduced in .NET 7")]
///// <summary>For backwards compatibility, and should not be used by new applications.Provides access to APIs that were not available only until .NET 7.</summary>
//public class PreDotNet7CompatibilityAttribute : Attribute { }


//[Obsolete("introduced in .NET 8")]
///// <summary>For backwards compatibility, and should not be used by new applications.Provides access to APIs that were not available only until .NET 8.</summary>
//public class PreDotNet8CompatibilityAttribute : Attribute { }

#endif