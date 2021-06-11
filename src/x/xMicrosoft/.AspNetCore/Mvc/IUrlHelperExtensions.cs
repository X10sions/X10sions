#nullable enable
using Common.Constants;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.AspNetCore.Mvc {
  public static class IUrlHelperExtensions {

    public static IEnumerable<string> ContentPaths(this IUrlHelper urlHelper, IEnumerable<string> paths) => from p in paths select urlHelper.Content(p);
    public static IEnumerable<string> ContentPaths(this IUrlHelper urlHelper, Func<string, bool> predicate, IEnumerable<string> paths) => from path in paths.Where(predicate) select urlHelper.Content(path);
    public static IEnumerable<string> ContentPaths(this IUrlHelper urlHelper, string fileExtension, IEnumerable<string> paths) => from path in paths where path.PathHelper().HasFileExtension(fileExtension) select urlHelper.Content(path);

    public static string FileHtmlTagString(this IUrlHelper urlHelper, string path) {
      var applicationPath = urlHelper.Content(path);
      switch (Path.GetExtension(path)) {
        case ".css": return HtmlConstants.StylesheetHtmlTagString(applicationPath);
        case ".js": return HtmlConstants.ScriptHtmlTag(applicationPath);
        default: return string.Empty;
      }
    }

    public static HtmlString FileHtmlString(this IUrlHelper urlHelper, string virtualPath) => new HtmlString(urlHelper.FileHtmlTagString(virtualPath));
    public static HtmlString FileHtmlStrings(this IUrlHelper urlHelper, IEnumerable<string> virtualPaths) => new HtmlString(string.Join(Environment.NewLine, virtualPaths.Select(path => urlHelper.FileHtmlTagString(path))));
    public static HtmlString ScriptHtmlStrings(this IUrlHelper urlHelper, IEnumerable<string> virtualPaths) => urlHelper.FileHtmlStrings(virtualPaths.Where(x => x.PathHelper().HasFileExtension(".js")));
    public static HtmlString StylesheetHtmlStrings(this IUrlHelper urlHelper, IEnumerable<string> virtualPaths) => urlHelper.FileHtmlStrings(virtualPaths.Where(x => x.PathHelper().HasFileExtension(".css")));

    //public static string VirtualDirectory(this IUrlHelper url) => url.ActionContext.ActionDescriptor.DisplayName.PathHelper().VirtualDirectory;
    //public static string VirtualFilePath(this IUrlHelper url, bool includeExtension = true) => url.ActionContext.ActionDescriptor.DisplayName.PathHelper().VirtualFile(includeExtension);

  }
}