using Microsoft.Internal;
using Microsoft.Internal.WindowsBase;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Text;

namespace System.Windows.Navigation {
  /// <summary>Provides a method to resolve relative uniform resource identifiers (URIs) with respect to the base URI of a container, such as a <see cref="T:System.Windows.Controls.Frame" />.</summary>
  public static class BaseUriHelper {
    private const string SOOBASE = "SiteOfOrigin://";
    private static readonly Uri _siteOfOriginBaseUri;
    private const string APPBASE = "application://";
    private static readonly Uri _packAppBaseUri;
    private static MS.Internal.SecurityCriticalDataForSet<Uri> _baseUri;
    private const string _packageApplicationBaseUriEscaped = "application:///";
    private const string _packageSiteOfOriginBaseUriEscaped = "siteoforigin:///";
    /// <summary>Identifies the BaseUri attached property.</summary>
    /// <returns>The identifier for the BaseUri attached property.</returns>
    public static readonly DependencyProperty BaseUriProperty;
    private const string COMPONENT = ";component";
    private const string VERSION = "v";
    private const char COMPONENT_DELIMITER = ';';
    private static Assembly _resourceAssembly;

    internal static Uri SiteOfOriginBaseUri {
      [FriendAccessAllowed]
      get {
        return _siteOfOriginBaseUri;
      }
    }

    internal static Uri PackAppBaseUri {
      [FriendAccessAllowed]
      get {
        return _packAppBaseUri;
      }
    }

    internal static Uri BaseUri {
      [FriendAccessAllowed]
      get {
        return _baseUri.Value;
      }
      [SecurityCritical]
      [FriendAccessAllowed]
      set {
        _baseUri.Value = value;
      }
    }

    internal static Assembly ResourceAssembly {
      get {
        if (_resourceAssembly == null) {
          _resourceAssembly = Assembly.GetEntryAssembly();
        }
        return _resourceAssembly;
      }
      [FriendAccessAllowed]
      set {
        _resourceAssembly = value;
      }
    }

    [SecurityCritical]
    [SecurityTreatAsSafe]
    static BaseUriHelper() {
      _siteOfOriginBaseUri = PackUriHelper.Create(new Uri("SiteOfOrigin://"));
      _packAppBaseUri = PackUriHelper.Create(new Uri("application://"));
      BaseUriProperty = DependencyProperty.RegisterAttached("BaseUri", typeof(Uri), typeof(BaseUriHelper), new PropertyMetadata((object)null));
      _baseUri = new MS.Internal.SecurityCriticalDataForSet<Uri>(_packAppBaseUri);
      PreloadedPackages.AddPackage(PackUriHelper.GetPackageUri(SiteOfOriginBaseUri), new SiteOfOriginContainer(), true);
    }

    /// <summary>Gets the value of the BaseUri attached property for a specified <see cref="T:System.Windows.UIElement" />.</summary>
    /// <returns>The base URI of a given element.</returns>
    /// <param name="element">The element from which the property value is read. </param>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="element" /> is null.</exception>
    [SecurityCritical]
    public static Uri GetBaseUri(DependencyObject element) {
      Uri uri = GetBaseUriCore(element);
      if (uri == null) {
        uri = BaseUri;
      } else if (!uri.IsAbsoluteUri) {
        uri = new Uri(BaseUri, uri);
      }
      return uri;
    }

    internal static bool IsPackApplicationUri(Uri uri) {
      if (uri.IsAbsoluteUri && SecurityHelper.AreStringTypesEqual(uri.Scheme, PackUriHelper.UriSchemePack)) {
        return SecurityHelper.AreStringTypesEqual(PackUriHelper.GetPackageUri(uri).GetComponents(UriComponents.AbsoluteUri, UriFormat.UriEscaped), "application:///");
      }
      return false;
    }

    [FriendAccessAllowed]
    internal static void GetAssemblyAndPartNameFromPackAppUri(Uri uri, out Assembly assembly, out string partName) {
      Uri uri2 = new Uri(uri.AbsolutePath, UriKind.Relative);
      GetAssemblyNameAndPart(uri2, out partName, out string assemblyName, out string assemblyVersion, out string assemblyKey);
      if (string.IsNullOrEmpty(assemblyName)) {
        assembly = ResourceAssembly;
      } else {
        assembly = GetLoadedAssembly(assemblyName, assemblyVersion, assemblyKey);
      }
    }

    [FriendAccessAllowed]
    internal static Assembly GetLoadedAssembly(string assemblyName, string assemblyVersion, string assemblyKey) {
      AssemblyName assemblyName2 = new AssemblyName(assemblyName) {
        CultureInfo = new CultureInfo(string.Empty)
      };
      if (!string.IsNullOrEmpty(assemblyVersion)) {
        assemblyName2.Version = new Version(assemblyVersion);
      }
      byte[] array = ParseAssemblyKey(assemblyKey);
      if (array != null) {
        assemblyName2.SetPublicKeyToken(array);
      }
      Assembly assembly = SafeSecurityHelper.GetLoadedAssembly(assemblyName2);
      if (assembly == null) {
        assembly = Assembly.Load(assemblyName2);
      }
      return assembly;
    }

    [FriendAccessAllowed]
    internal static void GetAssemblyNameAndPart(Uri uri, out string partName, out string assemblyName, out string assemblyVersion, out string assemblyKey) {
      Invariant.Assert(uri != null && !uri.IsAbsoluteUri, "This method accepts relative uri only.");
      string text = uri.ToString();
      int num = 0;
      if (text[0] == '/') {
        num = 1;
      }
      partName = text.Substring(num);
      assemblyName = string.Empty;
      assemblyVersion = string.Empty;
      assemblyKey = string.Empty;
      int num2 = text.IndexOf('/', num);
      string text2 = string.Empty;
      bool flag = false;
      if (num2 > 0) {
        text2 = text.Substring(num, num2 - num);
        if (text2.EndsWith(";component", StringComparison.OrdinalIgnoreCase)) {
          partName = text.Substring(num2 + 1);
          flag = true;
        }
      }
      if (flag) {
        string[] array = text2.Split(';');
        int num3 = array.Length;
        if (num3 > 4 || num3 < 2) {
          throw new UriFormatException(SR.Get("WrongFirstSegment"));
        }
        assemblyName = Uri.UnescapeDataString(array[0]);
        int num4 = 1;
        while (true) {
          if (num4 >= num3 - 1) {
            return;
          }
          if (array[num4].StartsWith("v", StringComparison.OrdinalIgnoreCase)) {
            if (!string.IsNullOrEmpty(assemblyVersion)) {
              throw new UriFormatException(SR.Get("WrongFirstSegment"));
            }
            assemblyVersion = array[num4].Substring(1);
          } else {
            if (!string.IsNullOrEmpty(assemblyKey)) {
              break;
            }
            assemblyKey = array[num4];
          }
          num4++;
        }
        throw new UriFormatException(SR.Get("WrongFirstSegment"));
      }
    }

    [FriendAccessAllowed]
    internal static bool IsComponentEntryAssembly(string component) {
      if (component.EndsWith(";component", StringComparison.OrdinalIgnoreCase)) {
        string[] array = component.Split(';');
        int num = array.Length;
        if (num >= 2 && num <= 4) {
          string strB = Uri.UnescapeDataString(array[0]);
          Assembly resourceAssembly = ResourceAssembly;
          if (resourceAssembly != null) {
            return string.Compare(SafeSecurityHelper.GetAssemblyPartialName(resourceAssembly), strB, StringComparison.OrdinalIgnoreCase) == 0;
          }
          return false;
        }
      }
      return false;
    }

    [FriendAccessAllowed]
    internal static Uri GetResolvedUri(Uri baseUri, Uri orgUri) {
      return new Uri(baseUri, orgUri);
    }

    [FriendAccessAllowed]
    internal static Uri MakeRelativeToSiteOfOriginIfPossible(Uri sUri) {
      if (Uri.Compare(sUri, SiteOfOriginBaseUri, UriComponents.Scheme, UriFormat.UriEscaped, StringComparison.OrdinalIgnoreCase) == 0) {
        PackUriHelper.ValidateAndGetPackUriComponents(sUri, out Uri packageUri, out Uri _);
        if (string.Compare(packageUri.GetComponents(UriComponents.AbsoluteUri, UriFormat.UriEscaped), "siteoforigin:///", StringComparison.OrdinalIgnoreCase) == 0) {
          return new Uri(sUri.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped)).MakeRelativeUri(sUri);
        }
      }
      return sUri;
    }

    [FriendAccessAllowed]
    internal static Uri ConvertPackUriToAbsoluteExternallyVisibleUri(Uri packUri) {
      Invariant.Assert(packUri.IsAbsoluteUri && SecurityHelper.AreStringTypesEqual(packUri.Scheme, PackAppBaseUri.Scheme));
      Uri uri = MakeRelativeToSiteOfOriginIfPossible(packUri);
      if (!uri.IsAbsoluteUri) {
        return new Uri(SiteOfOriginContainer.SiteOfOrigin, uri);
      }
      throw new InvalidOperationException(SR.Get("CannotNavigateToApplicationResourcesInWebBrowser", packUri));
    }

    [FriendAccessAllowed]
    internal static Uri FixFileUri(Uri uri) {
      if (uri != null && uri.IsAbsoluteUri && SecurityHelper.AreStringTypesEqual(uri.Scheme, Uri.UriSchemeFile) && string.Compare(uri.OriginalString, 0, Uri.UriSchemeFile, 0, Uri.UriSchemeFile.Length, StringComparison.OrdinalIgnoreCase) != 0) {
        return new Uri(uri.AbsoluteUri);
      }
      return uri;
    }

    internal static Uri AppendAssemblyVersion(Uri uri, Assembly assemblyInfo) {
      Uri uri2 = null;
      Uri uri3 = null;
      AssemblyName assemblyName = new AssemblyName(assemblyInfo.FullName);
      string value = assemblyName.Version?.ToString();
      if (uri != null && !string.IsNullOrEmpty(value)) {
        if (uri.IsAbsoluteUri) {
          if (IsPackApplicationUri(uri)) {
            uri2 = new Uri(uri.AbsolutePath, UriKind.Relative);
            uri3 = new Uri(uri.GetLeftPart(UriPartial.Authority), UriKind.Absolute);
          }
        } else {
          uri2 = uri;
        }
        if (uri2 != null) {
          GetAssemblyNameAndPart(uri2, out string partName, out string assemblyName2, out string assemblyVersion, out string assemblyKey);
          bool flag = !string.IsNullOrEmpty(assemblyKey);
          if (!string.IsNullOrEmpty(assemblyName2) && string.IsNullOrEmpty(assemblyVersion) && assemblyName2.Equals(assemblyName.Name, StringComparison.Ordinal) && (!flag || AssemblyMatchesKeyString(assemblyName, assemblyKey))) {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append('/');
            stringBuilder.Append(assemblyName2);
            stringBuilder.Append(';');
            stringBuilder.Append("v");
            stringBuilder.Append(value);
            if (flag) {
              stringBuilder.Append(';');
              stringBuilder.Append(assemblyKey);
            }
            stringBuilder.Append(";component");
            stringBuilder.Append('/');
            stringBuilder.Append(partName);
            string text = stringBuilder.ToString();
            if (uri3 != null) {
              return new Uri(uri3, text);
            }
            return new Uri(text, UriKind.Relative);
          }
        }
      }
      return null;
    }

    [SecurityCritical]
    [SecurityTreatAsSafe]
    internal static Uri GetBaseUriCore(DependencyObject element) {
      Uri uri = null;
      if (element != null) {
        try {
          DependencyObject dependencyObject = element;
          while (true) {
            if (dependencyObject == null) {
              return uri;
            }
            uri = (dependencyObject.GetValue(BaseUriProperty) as Uri);
            if (uri != null) {
              return uri;
            }
            IUriContext uriContext = dependencyObject as IUriContext;
            if (uriContext != null) {
              uri = uriContext.BaseUri;
              if (uri != null) {
                return uri;
              }
            }
            UIElement uIElement = dependencyObject as UIElement;
            if (uIElement != null) {
              dependencyObject = uIElement.GetUIParent(true);
            } else {
              ContentElement contentElement = dependencyObject as ContentElement;
              if (contentElement != null) {
                dependencyObject = contentElement.Parent;
              } else {
                Visual visual = dependencyObject as Visual;
                if (visual == null) {
                  break;
                }
                dependencyObject = VisualTreeHelper.GetParent(visual);
              }
            }
          }
          return uri;
        } finally {
          if (uri != null) {
            SecurityHelper.DemandUriDiscoveryPermission(uri);
          }
        }
      }
      throw new ArgumentNullException(nameof(element));
    }

    private static bool AssemblyMatchesKeyString(AssemblyName asmName, string assemblyKey) {
      byte[] curKeyToken = ParseAssemblyKey(assemblyKey);
      byte[] publicKeyToken = asmName.GetPublicKeyToken();
      return SafeSecurityHelper.IsSameKeyToken(publicKeyToken, curKeyToken);
    }

    private static byte[] ParseAssemblyKey(string assemblyKey) {
      if (!string.IsNullOrEmpty(assemblyKey)) {
        int num = assemblyKey.Length / 2;
        byte[] array = new byte[num];
        for (int i = 0; i < num; i++) {
          string s = assemblyKey.Substring(i * 2, 2);
          array[i] = byte.Parse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }
        return array;
      }
      return null;
    }
  }

}
