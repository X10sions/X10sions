using Common;

namespace Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure {
  public static class HandlerMethodDescriptorExtensions {

    #region "DebugObject"
    public static HandlerMethodDescriptorDebugObject GetDebugObject(this HandlerMethodDescriptor handlerMethodDescriptor) => new HandlerMethodDescriptorDebugObject(handlerMethodDescriptor);
    #endregion
  }

  public class HandlerMethodDescriptorDebugObject : IDebugObject<HandlerMethodDescriptor> {
    public HandlerMethodDescriptorDebugObject(HandlerMethodDescriptor handlerMethodDescriptor) {
      this.handlerMethodDescriptor = handlerMethodDescriptor;
    }
    HandlerMethodDescriptor handlerMethodDescriptor;

    public string Name => handlerMethodDescriptor.Name;
    public string HttpMethod => handlerMethodDescriptor.HttpMethod;
  }
}
