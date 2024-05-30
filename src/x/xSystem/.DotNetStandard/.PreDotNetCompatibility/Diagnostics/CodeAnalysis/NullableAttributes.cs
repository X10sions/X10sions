namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
[PreDotNetCompatibility("https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Diagnostics/CodeAnalysis/NullableAttributes.cs")]
internal sealed class AllowNullAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
[PreDotNetCompatibility("https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Diagnostics/CodeAnalysis/NullableAttributes.cs")]
internal sealed class DisallowNullAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false)]
[PreDotNetCompatibility("https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Diagnostics/CodeAnalysis/NullableAttributes.cs")]
internal sealed class MaybeNullAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false)]
[PreDotNetCompatibility("https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Diagnostics/CodeAnalysis/NullableAttributes.cs")]
internal sealed class NotNullAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
[PreDotNetCompatibility("https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Diagnostics/CodeAnalysis/NullableAttributes.cs")]
internal sealed class MaybeNullWhenAttribute(bool returnValue) : Attribute {
  public bool ReturnValue { get; } = returnValue;
}

[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
[PreDotNetCompatibility("https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Diagnostics/CodeAnalysis/NullableAttributes.cs")]
internal sealed class NotNullWhenAttribute : Attribute {
  public bool ReturnValue { get; }
}

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
[PreDotNetCompatibility("https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Diagnostics/CodeAnalysis/NullableAttributes.cs")]
internal sealed class NotNullIfNotNullAttribute(string parameterName) : Attribute {
  public string ParameterName { get; } = parameterName;
}

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
[PreDotNetCompatibility("https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Diagnostics/CodeAnalysis/NullableAttributes.cs")]
internal sealed class DoesNotReturnAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
[PreDotNetCompatibility("https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Diagnostics/CodeAnalysis/NullableAttributes.cs")]
internal sealed class DoesNotReturnIfAttribute(bool parameterValue) : Attribute {
  public bool ParameterValue { get; } = parameterValue;
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
[PreDotNetCompatibility("https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Diagnostics/CodeAnalysis/NullableAttributes.cs")]
internal sealed class MemberNotNullAttribute : Attribute {
  public MemberNotNullAttribute(string member) => Members = [member];
  public MemberNotNullAttribute(params string[] members) => Members = members;
  public string[] Members { get; }
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
[PreDotNetCompatibility("https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Diagnostics/CodeAnalysis/NullableAttributes.cs")]
internal sealed class MemberNotNullWhenAttribute : Attribute {
  public MemberNotNullWhenAttribute(bool returnValue, string member) {
    ReturnValue = returnValue;
    Members = [member];
  }
  public MemberNotNullWhenAttribute(bool returnValue, params string[] members) {
    ReturnValue = returnValue;
    Members = members;
  }
  public bool ReturnValue { get; }
  public string[] Members { get; }
}