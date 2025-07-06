using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.RegularExpressions;

namespace System.Reflection;
public static class MemberInfoExtensions {

  public static string GetHtmlAttributeName(this MemberInfo property) {
    var htmlAttributeNameAttribute = property.GetCustomAttribute<HtmlAttributeNameAttribute>();
    if (htmlAttributeNameAttribute != null) {
      return htmlAttributeNameAttribute.DictionaryAttributePrefix + htmlAttributeNameAttribute.Name;
    }
    return Regex.Replace(property.Name, "([A-Z])", "-$1").ToLower().Trim('-');
  }

  public static AuthorizeAttribute? GetAuthorizeAttribute(this MemberInfo memberInfo) => memberInfo.GetCustomAttribute<AuthorizeAttribute>();
  public static bool HasAuthorizeAttribute(this MemberInfo memberInfo) => memberInfo.GetAuthorizeAttribute() != null;

}