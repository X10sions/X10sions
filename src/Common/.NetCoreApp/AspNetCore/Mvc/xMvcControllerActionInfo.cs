namespace Common.AspNetCore.Mvc {
  public class xMvcControllerActionInfo : xMvcActionDescriptorInfo {
    public string Id => $"{ControllerId}:{Name}";
    public string ControllerId => ControllerInfo.Id;
    public xMvcControllerInfo ControllerInfo { get; set; }

    public string MethodName { get; set; }
  }
}
