namespace System.Data.Linq {
  public interface IExecuteResult : IDisposable {
    object ReturnValue { get; }
    object GetParameterValue(int parameterIndex);
  }
}