using Common;

namespace Microsoft.AspNetCore.Mvc.RazorPages {
  public static class PageActionDescriptorExtensions {

    #region "DebugObject"
    public static PageActionDescriptorDebugObject GetDebugObject(this PageActionDescriptor pageActionDescriptor) => new PageActionDescriptorDebugObject(pageActionDescriptor);
    #endregion
  }

  public class PageActionDescriptorDebugObject : IDebugObject<PageActionDescriptor> {
    public PageActionDescriptorDebugObject(PageActionDescriptor pageActionDescriptor) {
      this.pageActionDescriptor = pageActionDescriptor;
    }
    PageActionDescriptor pageActionDescriptor;

    public string AreaName => pageActionDescriptor.AreaName;
    public string RelativePath => pageActionDescriptor.RelativePath;
    public string ViewEnginePath => pageActionDescriptor.ViewEnginePath;

  }
}