namespace Microsoft.AspNetCore.Mvc.Rendering;
public static class SelectListGroupExtensions {

  public static SelectListGroup GetSelectListGroup(this List<SelectListGroup> groupList, string groupName, bool isDisabled) {
    var g = groupList.FirstOrDefault(x => x.Name == groupName);
    if (g == null) {
      g = new SelectListGroup { Name = groupName };
      groupList.Add(g);
    }
    g.Disabled = isDisabled;
    return g;
  }

}
