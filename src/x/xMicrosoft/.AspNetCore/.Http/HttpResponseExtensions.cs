namespace Microsoft.AspNetCore.Http;
public static class HttpResponseExtensions {

  // https://docs.microsoft.com/en-us/aspnet/core/migration/http-modules?view=aspnetcore-2.1

  public static string StatusDescription(this HttpResponse httpResponse) => StatusCodesDictionary.Instance[httpResponse.StatusCode];

}