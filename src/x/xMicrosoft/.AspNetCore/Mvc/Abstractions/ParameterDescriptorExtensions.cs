using Common;
using System;

namespace Microsoft.AspNetCore.Mvc.Abstractions {
  public static class ParameterDescriptorExtensions {

    #region "DebugObject"
    public static ParameterDescriptorDebugObject GetDebugObject(this ParameterDescriptor parameterDescriptor) => new ParameterDescriptorDebugObject(parameterDescriptor);
    #endregion

  }

  public class ParameterDescriptorDebugObject : IDebugObject<ParameterDescriptor> {
    public ParameterDescriptorDebugObject(ParameterDescriptor parameterDescriptor) {
      this.parameterDescriptor = parameterDescriptor;
    }
    ParameterDescriptor parameterDescriptor;

    public string Name => parameterDescriptor.Name;
    [Obsolete("ToDo: DebugObject for Type")] public string TypeName => parameterDescriptor.ParameterType.Name;
    [Obsolete("ToDo: DebugObject for BindingInfo")] public string BindingInfoBinderModelName => parameterDescriptor.BindingInfo.BinderModelName;


  }
}
