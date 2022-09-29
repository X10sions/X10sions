namespace Microsoft.AspNetCore.Mvc.RazorPages;
public static class CompiledPageActionDescriptorExtensions {

  public static (string areaName, string pagePath, string handlerName) GetAreaPageHandler(this CompiledPageActionDescriptor compiledPageActionDescriptor) {
    var area = compiledPageActionDescriptor.AreaName;
    var vePath = compiledPageActionDescriptor.ViewEnginePath;
    var handler = compiledPageActionDescriptor.HandlerTypeInfo.Name;
    return (area, vePath, handler);
  }

}