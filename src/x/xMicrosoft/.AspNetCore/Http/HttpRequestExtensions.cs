using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.AspNetCore.Http;
public static class HttpRequestExtensions {
  public static StringValues Item(this HttpRequest httpRequest, string key, StringValues defaultValue = default) => httpRequest.Item(key, new HttpRequestItemOptions { DefaultValue = defaultValue });

  public static StringValues Item(this HttpRequest httpRequest, string key, HttpRequestItemOptions options)
    => (options.SearchQuery ? httpRequest.Query?[key] : null)
    ?? (options.SearchForm ? httpRequest.Form?[key] : null)
    ?? (options.SearchCookies ? httpRequest.Cookies?[key] : null)
    ?? (options.SearchHeaders ? httpRequest.Headers?[key] : null)
    .As(options.DefaultValue);

  public static IEnumerable<KeyValuePair<string, StringValues>> Items(this HttpRequest httpRequest)
    => httpRequest.Query
    .Concat(httpRequest.Form)
    .Concat(httpRequest.Cookies.Select(x => new KeyValuePair<string, StringValues>(x.Key, x.Value)))
    .Concat(httpRequest.Headers);

  public class HttpRequestItemOptions {
    public StringValues DefaultValue { get; set; }
    public bool SearchQuery { get; set; } = true;
    public bool SearchForm { get; set; } = true;
    public bool SearchCookies { get; set; }
    public bool SearchHeaders { get; set; }
  }

  //regex from http://detectmobilebrowsers.com/
  static readonly Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
  static readonly Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);

  public static bool IsMobileBrowser(this HttpRequest request) {
    var userAgent = request.Headers["User-Agent"].ToString();
    if ((b.IsMatch(userAgent) || v.IsMatch(userAgent.Substring(0, 4)))) {
      return true;
    }
    return false;
  }

  public static string PathAndQuery(this HttpRequest request) => request.Path + request.QueryString;

  public static Uri GetBaseUri(this HttpRequest request) {
    var hostComponents = request.Host.ToUriComponent().Split(':');
    var builder = new UriBuilder { Scheme = request.Scheme, Host = hostComponents[0] };
    if (hostComponents.Length == 2)
      builder.Port = Convert.ToInt32(hostComponents[1]);
    return builder.Uri;
  }

  public static string GetCurrentBaseUrl(this HttpRequest request) => request.Scheme + "://" + request.Host.ToUriComponent();

  public static string GetCurrentFullUrl(this HttpRequest request) => request.Scheme + "://" + request.Host.ToUriComponent() + request.Path.Value;

  public static string GetCurrentFullUrlWithQueryString(this HttpRequest request) => request.Scheme + "://"
        + request.Host.ToUriComponent()
        + request.Path.Value
        + request.QueryString
        ;

  //public static IPAddress ConnectionRemoteIpAddress(this HttpContext context) => context.Connection.RemoteIpAddress;
  //public static IPAddress ConnectionRemoteIpAddressV4(this HttpContext context) => context.ConnectionRemoteIpAddress().ToV4();
  //public static IPAddress ConnectionRemoteIpAddressV6(this HttpContext context) => context.ConnectionRemoteIpAddress().ToV6();

  public static bool HasFormCount(this HttpRequest httpRequest) => httpRequest.HasFormContentType && httpRequest.Form != null && httpRequest.Form.Count > 0;

  public static string FormString(this HttpRequest httpRequest) {
    var sb = new StringBuilder();
    if (httpRequest.HasFormCount()) {
      foreach (var kv in httpRequest.Form) {
        sb.Append($"{kv.Key}={kv.Value};");
      }
    }
    return sb.ToString();
  }

  public static async Task<IFormCollection> FormAsync(this HttpRequest httpRequest) => httpRequest.HasFormContentType ? await httpRequest.ReadFormAsync() : default;

  public static string FormKeysString(this HttpRequest httpRequest) {
    var sb = new StringBuilder();
    if (httpRequest.HasFormCount()) {
      foreach (var key in httpRequest.Form.Keys) {
        sb.Append($"{key}={httpRequest.Form[key]};");
      }
    }
    return sb.ToString();
  }

  public static string VirtualDirectory(this HttpRequest req) => req.Path.Value.PathHelper().VirtualDirectory;
  public static string VirtualFilePath(this HttpRequest req, bool includeExtension = true) => req.Path.Value.PathHelper().VirtualFile(includeExtension);
  //public static string AppRelativeVirtualPath(this HttpRequest req) => req.PhysicalPath.Replace(this.PhysicalApplicationPath.TrimEnd("\"), String.Empty).Replace("\", "/")

  // https://docs.microsoft.com/en-us/aspnet/core/migration/http-modules?view=aspnetcore-2.1
  //[Obsolete("Use HttpRequest.Method")] public static string HttpMethod(this HttpRequest httpRequest) => httpRequest.Method;
  //[Obsolete] public static string Url(this HttpRequest httpRequest) => httpRequest.GetDisplayUrl();
  //[Obsolete] public static string RawUrl(this HttpRequest httpRequest) => httpRequest.GetDisplayUrl();


  //[Obsolete] public static bool IsSecureConnection(this HttpRequest httpRequest) => httpRequest.IsHttps;
  //[Obsolete] public static string UserHostAddress(this HttpRequest httpRequest) => httpRequest.HttpContext.Connection.RemoteIpAddress?.ToString();
  //[Obsolete] public static string UrlReferrer(this HttpRequest httpRequest) => httpRequest.Headers.Referer();
  //[Obsolete] public static string UserAgent(this HttpRequest httpRequest) => httpRequest.Headers.UserAgent();

  //[Obsolete] public static string GetFullPath(this HttpRequest httpRequest) => Path.GetFullPath(httpRequest.Path).Replace("~\\", string.Empty);

  public static MediaTypeHeaderValue MediaTypeHeaderValue(this HttpRequest httpRequest) => httpRequest.GetTypedHeaders().ContentType;

  public static string Url(this HttpRequest request, string? scheme = null, string? host = null, int? port = null, string? pathBase = null, string? path = null, string? queryString = null, string? fragment = null)
    => $"{scheme ?? request.Scheme + "://"}{host ?? request.Host.Host}{port ?? request.Host.Port}{pathBase ?? request.PathBase}{path ?? request.Path}{queryString ?? request.QueryString.Value}{fragment ?? string.Empty}";

  [Obsolete] public static string? ContentType(this HttpRequest httpRequest) => httpRequest.MediaTypeHeaderValue()?.MediaType.ToString();
  [Obsolete] public static string? ContentMainType(this HttpRequest httpRequest) => httpRequest.MediaTypeHeaderValue()?.Type.ToString();
  [Obsolete] public static string? ContentSubType(this HttpRequest httpRequest) => httpRequest.MediaTypeHeaderValue()?.SubType.ToString();
  [Obsolete] public static Encoding? Encoding(this HttpRequest httpRequest) => httpRequest.MediaTypeHeaderValue()?.Encoding;

  public static string FirstFormThenQueryValue(this HttpRequest request, string key) => request.Form[key].FirstOrDefault() ?? request.Query[key].FirstOrDefault() ?? string.Empty;
  public static string FirstQueryThenFormValue(this HttpRequest request, string key) => request.Query[key].FirstOrDefault() ?? request.Form[key].FirstOrDefault() ?? string.Empty;

  const string X_Requested_With = "X-Requested-With";
  const string XmlHttpRequest = "XMLHttpRequest";

  public static bool IsAjaxRequest(this HttpRequest request) => request.Headers != null ? request.Headers[X_Requested_With] == XmlHttpRequest : false;

  public static string SchemeHostUrl(this HttpRequest request, string suffix = "") => $"{request.Scheme}://{request.Host}{suffix}";
  public static string SchemeHostPathUrl(this HttpRequest request, string suffix = "") => $"{request.SchemeHostUrl()}{request.PathBase}{request.Path}{suffix}";

  public static string GetBaseUrl(this HttpRequest request, string? pathBase = null) => $"{request.Scheme}://{request.Host.ToUriComponent()}{pathBase ?? request.PathBase.ToUriComponent()}";

  public static string GetRawTarget(this HttpRequest request) => request.HttpContext.Features.GetHttpRequestFeature().RawTarget;

  public static UriBuilder UriBuilder(HttpRequest request, string userName = "", string password = "", string fragment = "") =>
    new UriBuilder(request.Scheme, request.Host.Value) {
      //      Scheme = request.Scheme,
      //      Host = request.Host.Host,
      //      Port = request.Host.Port.Value,
      UserName = userName,
      Password = password,
      Path = request.Path,
      Query = request.QueryString.Value,
      Fragment = fragment
    };

}
