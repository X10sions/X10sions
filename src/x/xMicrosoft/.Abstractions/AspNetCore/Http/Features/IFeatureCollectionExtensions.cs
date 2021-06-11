namespace Microsoft.AspNetCore.Http.Features {
  public static class IFeatureCollectionExtensions {
    // http://www.irmaktevfik.com/post/2016/09/05/a-dive-into-asp-net-core-with-an-example-project

    public static IEndpointFeature GetEndpointFeature(this IFeatureCollection features) => features.Get<IEndpointFeature>();
    public static IFormFeature GetFormFeatureFeature(this IFeatureCollection features) => features.Get<IFormFeature>();
    public static IHttpBodyControlFeature GetHttpBodyControlFeature(this IFeatureCollection features) => features.Get<IHttpBodyControlFeature>();
    public static IHttpBufferingFeature GetHttpBufferingFeature(this IFeatureCollection features) => features.Get<IHttpBufferingFeature>();
    public static IHttpConnectionFeature GetHttpConnectionFeature(this IFeatureCollection features) => features.Get<IHttpConnectionFeature>();
    public static IHttpMaxRequestBodySizeFeature GetHttpMaxRequestBodySizeFeature(this IFeatureCollection features) => features.Get<IHttpMaxRequestBodySizeFeature>();
    public static IHttpRequestFeature GetHttpRequestFeature(this IFeatureCollection features) => features.Get<IHttpRequestFeature>();
    public static IHttpRequestIdentifierFeature GetHttpRequestIdentifierFeature(this IFeatureCollection features) => features.Get<IHttpRequestIdentifierFeature>();
    public static IHttpResponseFeature GetHttpResponseFeature(this IFeatureCollection features) => features.Get<IHttpResponseFeature>();
    public static IHttpSendFileFeature GetHttpSendFileFeature(this IFeatureCollection features) => features.Get<IHttpSendFileFeature>();
    public static IHttpUpgradeFeature GetHttpUpgradeFeature(this IFeatureCollection features) => features.Get<IHttpUpgradeFeature>();
    public static IHttpWebSocketFeature GetHttpWebSocketFeature(this IFeatureCollection features) => features.Get<IHttpWebSocketFeature>();
    public static IRequestCookiesFeature GetRequestCookiesFeature(this IFeatureCollection features) => features.Get<IRequestCookiesFeature>();
    public static IRouteValuesFeature GetRouteValuesFeature(this IFeatureCollection features) => features.Get<IRouteValuesFeature>();
    public static ISessionFeature GetSessionFeature(this IFeatureCollection features) => features.Get<ISessionFeature>();

  }
}