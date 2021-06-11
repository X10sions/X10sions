using System.Collections.Generic;

namespace Common.AspNetCore.Mvc {
  public class xMvcControllerInfo {

    public string Id => $"{AreaName}:{Name}";
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string AreaName { get; set; }
    public IEnumerable<xMvcControllerActionInfo> Actions { get; set; }

    public string ClassName { get; set; }
  }
}
