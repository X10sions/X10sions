using System.Linq;

namespace Microsoft.AspNetCore.Razor.TagHelpers {
  public static class TagHelperAttributeListExtensions {

    public static bool RemoveAll(this TagHelperAttributeList attributeList, params string[] attributeNames) => attributeNames.Aggregate(false, (current, name) => attributeList.RemoveAll(name) || current);

    public static TagHelperAttributeList AddAriaAttribute(this TagHelperAttributeList attributeList, string attributeName, object value) {
      attributeList.Add("aria-" + attributeName, value);
      return attributeList;
    }

    public static TagHelperAttributeList AddDataAttribute(this TagHelperAttributeList attributeList, string attributeName, object value) {
      attributeList.Add("data-" + attributeName, value);
      return attributeList;
    }

  }
}