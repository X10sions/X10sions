using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Microsoft.AspNetCore.Http;
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

  public static IExceptionHandlerPathFeature? GetExceptionHandlerPathFeature(this HttpContext httpContext) => httpContext.Features.Get<IExceptionHandlerPathFeature>();
  public static Exception? GetExceptionHandlerPathFeatureException(this HttpContext httpContext) => httpContext.Features.Get<IExceptionHandlerPathFeature>()!.Error;
  public static IProblemDetailsService? GetProblemDetailsService(this HttpContext httpContext) => httpContext.RequestServices.GetService<IProblemDetailsService>();

  public static string GetRequestState(this HttpContext httpContext) => httpContext.Request.Values().State;

  public static string GetStateLoginProvider(this HttpContext httpContext) => httpContext.GetStateValuesDictionary()["LoginProvider"];

  public static IDictionary<string, string> GetStateValuesDictionary(this HttpContext httpContext) => JsonSerializer.Deserialize<IDictionary<string, string>>(httpContext.GetRequestState());

  public static async ValueTask WriteProblemDetailsServiceAsync(this HttpContext context, IProblemDetailsService problemDetailsService, string problemDetail, string problemType, string problemTitle = "Bad Input") {
    await problemDetailsService.WriteAsync(new ProblemDetailsContext {
      HttpContext = context,
      ProblemDetails = {
        Title = problemTitle,
        Detail = problemDetail,
        Type = problemType
      }
    });
  }

}