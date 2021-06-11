using System.Collections.Generic;

namespace Common.AspNetCore.Mvc {
  public class xMvcRazorPageInfo {
    public string ActionDescriptorId { get; set; }
    public string Id => $"{AreaName}:{ViewEnginePath}";
    public string AreaName { get; set; }        // eg: /Areas/Identity/Pages
    public string DisplayName { get; set; }
    public string PageId { get; set; }
    public string RelativePath { get; set; }    // eg: /Areas/Identity/Pages/Manage/Accounts.cshtml  //Invocation
    public string ViewEnginePath { get; set; }  // eg:                      /Manage/Accounts         //Path
    public IEnumerable<xMvcRazorPageHandlerInfo> Handlers { get; set; }
    public string ClassName { get; set; }
    public string DeclaredModelClassName { get; set; }
    public string ModelClassName { get; set; }

  }
}
