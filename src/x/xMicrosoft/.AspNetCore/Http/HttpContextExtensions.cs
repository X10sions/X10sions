using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Routing;
using System.Text.Json;

namespace Microsoft.AspNetCore.Http {
  public static class HttpContextExtensions {

    public static AuthenticationProperties GetAuthenticationProperties(this HttpContext httpContext, bool rememberMe) => new AuthenticationProperties(httpContext.GetStateValuesDictionary()) {
      IsPersistent = rememberMe
    };

    public static string? GetEndpointNameMetadataName(this HttpContext httpContext) {
      if (httpContext == null)
        throw new ArgumentNullException(nameof(httpContext));
      var endpoint = httpContext.GetEndpoint();
      return endpoint?.Metadata.GetMetadata<EndpointNameMetadata>()?.EndpointName;
    }

    public static string GetRequestState(this HttpContext httpContext) => httpContext.Request.Values().State;

    public static string GetStateLoginProvider(this HttpContext httpContext) => httpContext.GetStateValuesDictionary()["LoginProvider"];

    public static IDictionary<string, string> GetStateValuesDictionary(this HttpContext httpContext) => JsonSerializer.Deserialize<IDictionary<string, string>>(httpContext.GetRequestState());

  }
}