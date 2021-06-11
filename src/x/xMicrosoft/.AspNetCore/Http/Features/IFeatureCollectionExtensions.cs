using Microsoft.AspNetCore.Localization;

namespace Microsoft.AspNetCore.Http.Features {
  public static class IFeatureCollectionExtensions {

    public static IRequestCultureFeature GetRequestCultureFeature(this IFeatureCollection features) => features.Get<IRequestCultureFeature>();

  }
}