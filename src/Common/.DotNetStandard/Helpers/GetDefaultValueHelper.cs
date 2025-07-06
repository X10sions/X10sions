using Common.Interfaces;

namespace Common.Helpers {
  public class GetDefaultValueHelper<T> : IGetDefaultValueHelper {
    public object GetDefaultValue() => default(T);
  }
}