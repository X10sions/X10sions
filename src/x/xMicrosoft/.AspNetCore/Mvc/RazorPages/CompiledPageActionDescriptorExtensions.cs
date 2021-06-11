using Common;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.AspNetCore.Mvc.RazorPages {
  public static class CompiledPageActionDescriptorExtensions {

    public static (string areaName, string pagePath, string handlerName) GetAreaPageHandler(this CompiledPageActionDescriptor compiledPageActionDescriptor) {
      var area = compiledPageActionDescriptor.AreaName;
      var vePath = compiledPageActionDescriptor.ViewEnginePath;
      var handler = compiledPageActionDescriptor.HandlerTypeInfo.Name;
      return (area, vePath, handler);
    }

    #region "DebugObject"

    public static CompiledPageActionDescriptorDebugObject GetDebugObject(this CompiledPageActionDescriptor compiledPageActionDescriptor) => new CompiledPageActionDescriptorDebugObject(compiledPageActionDescriptor);

    #endregion

  }

  public class CompiledPageActionDescriptorDebugObject : IDebugObject<CompiledPageActionDescriptor> {
    public CompiledPageActionDescriptorDebugObject(CompiledPageActionDescriptor compiledPageActionDescriptor) {
      this.compiledPageActionDescriptor = compiledPageActionDescriptor;
    }
    CompiledPageActionDescriptor compiledPageActionDescriptor;

    public TypeInfoDebugObject HandlerTypeInfo => compiledPageActionDescriptor.HandlerTypeInfo.GetDebugObject();
    public IEnumerable<HandlerMethodDescriptorDebugObject> HandlerMethods => compiledPageActionDescriptor.HandlerMethods.Select(x => x.GetDebugObject());

    public TypeInfoDebugObject DeclaredModelTypeInfo => compiledPageActionDescriptor.DeclaredModelTypeInfo.GetDebugObject();
    public TypeInfoDebugObject ModelTypeInfo => compiledPageActionDescriptor.ModelTypeInfo.GetDebugObject();
    public TypeInfoDebugObject PageTypeInfo => compiledPageActionDescriptor.PageTypeInfo.GetDebugObject();

  }
}