using System;
using System.Security;
using System.Text;
using System.Windows.Navigation;

namespace Microsoft.Internal.PresentationCore {
  internal static class BindUriHelper {
    private const int MAX_PATH_LENGTH = 2048;

    private const int MAX_SCHEME_LENGTH = 32;

    public const int MAX_URL_LENGTH = 2083;

    internal static Uri BaseUri {
      get {
        return BaseUriHelper.BaseUri;
      }
      [SecurityCritical]
      set {
        BaseUriHelper.BaseUri = BaseUriHelper.FixFileUri(value);
      }
    }

    internal static string UriToString(Uri uri) {
      if (uri == null) {
        throw new ArgumentNullException("uri");
      }
      return new StringBuilder(uri.GetComponents(uri.IsAbsoluteUri ? UriComponents.AbsoluteUri : UriComponents.SerializationInfoString, UriFormat.SafeUnescaped), 2083).ToString();
    }

    internal static bool DoSchemeAndHostMatch(Uri first, Uri second) {
      if (SecurityHelper.AreStringTypesEqual(first.Scheme, second.Scheme)) {
        return first.Host.Equals(second.Host);
      }
      return false;
    }

    internal static Uri GetResolvedUri(Uri baseUri, Uri orgUri) {
      if (!(orgUri == null)) {
        if (orgUri.IsAbsoluteUri) {
          return BaseUriHelper.FixFileUri(orgUri);
        }
        Uri baseUri2 = (baseUri == null) ? BaseUri : baseUri;
        return new Uri(baseUri2, orgUri);
      }
      return null;
    }

    internal static string GetReferer(Uri destinationUri) {
      string result = null;
      Uri browserSource = SiteOfOriginContainer.BrowserSource;
      if (browserSource != null) {
        SecurityZone securityZone = CustomCredentialPolicy.MapUrlToZone(browserSource);
        SecurityZone securityZone2 = CustomCredentialPolicy.MapUrlToZone(destinationUri);
        if (securityZone == securityZone2 && SecurityHelper.AreStringTypesEqual(browserSource.Scheme, destinationUri.Scheme)) {
          result = browserSource.GetComponents(UriComponents.AbsoluteUri, UriFormat.UriEscaped);
        }
      }
      return result;
    }
  }
}
