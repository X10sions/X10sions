namespace Common.AspNetCore.Mvc {
  public class xMvcRazorPageHandlerInfo : xMvcActionDescriptorInfo {
    public string Id => $"{RazorPageId}:{Name}";
    public string RazorPageId => RazorPageInfo.Id;
    public xMvcRazorPageInfo RazorPageInfo { get; set; }
    public string ClassName { get; set; }
  }
}