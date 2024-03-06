using System.Web;

namespace System {
  public static class UriExtensions {
    public static string FileExtensionOnly(this Uri exn) => VirtualPathUtility.GetExtension(exn.LocalPath).Replace(".", "");

    public static string FileNameAndExtension(this Uri exn) => VirtualPathUtility.GetFileName(exn.LocalPath);

    public static string FileNameOnly(this Uri exn) => VirtualPathUtility.GetFileName(exn.LocalPath).Replace(VirtualPathUtility.GetExtension(exn.LocalPath), "");

    public static string VirtualPathOnly(this Uri exn) => VirtualPathUtility.GetDirectory(exn.LocalPath);

    public static string ToRelativeUrl(this Uri uri, string relativeUrl) {
      if (string.IsNullOrEmpty(relativeUrl)) {
        return relativeUrl;
      }
      if (HttpContext.Current == null) {
        return relativeUrl;
      }
      if (relativeUrl.StartsWith("/")) {
        relativeUrl = relativeUrl.Insert(0, "~");
      }
      if (!relativeUrl.StartsWith("~/")) {
        relativeUrl = relativeUrl.Insert(0, "~/");
      }
      var url = HttpContext.Current.Request.Url;
      var port = (url.Port != 80) ? (":" + url.Port) : string.Empty;
      return $"{url.Scheme}://{url.Host}{port}{VirtualPathUtility.ToAbsolute(relativeUrl)}";
    }
  }
}