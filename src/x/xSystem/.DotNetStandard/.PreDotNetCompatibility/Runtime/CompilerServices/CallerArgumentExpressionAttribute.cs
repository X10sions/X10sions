namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
[PreDotNetCompatibility("https://github.com/dotnet/runtime/blob/main/src/libraries/Common/src/System/ThrowHelper.cs")]
internal sealed class CallerArgumentExpressionAttribute : Attribute {
  public CallerArgumentExpressionAttribute(string parameterName) {
    ParameterName = parameterName;
  }
  public string ParameterName { get; }
}