using Common;

namespace Microsoft.AspNetCore.Mvc.Filters {
  public static class FilterDescriptorExtensions {
    #region "DebugObject"
    public static FilterDescriptorDebugObject GetDebugObject(this FilterDescriptor filterDescriptor) => new FilterDescriptorDebugObject(filterDescriptor);
    #endregion
  }

  public class FilterDescriptorDebugObject : IDebugObject<FilterDescriptor> {
    public FilterDescriptorDebugObject(FilterDescriptor filterDescriptor) {
      this.filterDescriptor = filterDescriptor;
    }
    FilterDescriptor filterDescriptor;
    public int Order => filterDescriptor.Order;
    public int Scope => filterDescriptor.Scope;
  }

}
