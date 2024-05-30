using Common;

namespace System.Reflection {
  public class MethodInfoDebugObject : IDebugObject<MethodInfo> {
    public MethodInfoDebugObject(MethodInfo methodInfo) {
      this.methodInfo = methodInfo;
    }
    MethodInfo methodInfo;
    public string Name => methodInfo.Name;
  }

  public static class MethodInfoExtensions {
    public static MethodInfoDebugObject GetDebugObject(this MethodInfo methodInfo) => new MethodInfoDebugObject(methodInfo);
  }

}