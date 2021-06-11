using Common;
using System;

namespace Microsoft.AspNetCore.Http {
  public static class HttpResponseExtensions {

    // https://docs.microsoft.com/en-us/aspnet/core/migration/http-modules?view=aspnetcore-2.1

    [Obsolete] public static int Status(this HttpResponse httpResponse) => httpResponse.StatusCode;

    public static string StatusDescription(this HttpResponse httpResponse) => StatusCodesDictionary.Instance[httpResponse.StatusCode];

    #region "DebugObject"

    public static HttpResponseDebugObject GetDebugObject(this HttpResponse httpResponse) => new HttpResponseDebugObject(httpResponse);
    #endregion

  }

  public class HttpResponseDebugObject : IDebugObject<HttpResponse> {
    public HttpResponseDebugObject(HttpResponse httpResponse) {
      this.httpResponse = httpResponse;
    }
    HttpResponse httpResponse;

    public long? ContentLength => httpResponse.ContentLength;
    public string ContentType => httpResponse.ContentType;
    public int StatusCode => httpResponse.StatusCode;

  }

}
