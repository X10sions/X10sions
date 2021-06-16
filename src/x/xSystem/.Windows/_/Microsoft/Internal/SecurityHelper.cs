using Microsoft.Internal.Permissions;
using Microsoft.Internal.PresentationCore;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Markup;

namespace Microsoft.Internal {
  internal static class SecurityHelper {
    private static SecurityPermission _unmanagedCodePermission;
    private static UserInitiatedRoutedEventPermission _userInitiatedRoutedEventPermission;
    private static PermissionSet _fullTrustPermissionSet;
    private static SecurityPermission _serializationSecurityPermission;
    private static UIPermission _uiPermissionAllClipboard;
    private static EnvironmentPermission _unrestrictedEnvironmentPermission;
    private static PermissionSet _envelopePermissionSet;
    private static RegistryPermission _unrestrictedRegistryPermission;
    private static UIPermission _allWindowsUIPermission;
    private static SecurityPermission _infrastructurePermission;
    private static UIPermission _unrestrictedUIPermission;

    [SecurityCritical]
    private static bool? _appDomainGrantedUnrestrictedUIPermission;

    private static PermissionSet _plugInSerializerPermissions;

    internal static PermissionSet EnvelopePermissionSet {
      [SecurityCritical]
      get {
        if (_envelopePermissionSet == null) {
          _envelopePermissionSet = CreateEnvelopePermissionSet();
        }
        return _envelopePermissionSet;
      }
    }

    internal static bool AppDomainGrantedUnrestrictedUIPermission {
      [SecurityCritical]
      get {
        if (!_appDomainGrantedUnrestrictedUIPermission.HasValue) {
          _appDomainGrantedUnrestrictedUIPermission = AppDomainHasPermission(new UIPermission(PermissionState.Unrestricted));
        }
        return _appDomainGrantedUnrestrictedUIPermission.Value;
      }
    }

    [SecuritySafeCritical]
    internal static bool CheckUnmanagedCodePermission() {
      try {
        DemandUnmanagedCode();
      } catch (SecurityException) {
        return false;
      }
      return true;
    }

    [SecurityCritical]
    internal static void DemandUnmanagedCode() {
      if (_unmanagedCodePermission == null) {
        _unmanagedCodePermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
      }
      _unmanagedCodePermission.Demand();
    }

    [SecurityCritical]
    internal static CodeAccessPermission CreateUserInitiatedRoutedEventPermission() {
      if (_userInitiatedRoutedEventPermission == null) {
        _userInitiatedRoutedEventPermission = new UserInitiatedRoutedEventPermission();
      }
      return _userInitiatedRoutedEventPermission;
    }

    [SecuritySafeCritical]
    internal static bool CallerHasUserInitiatedRoutedEventPermission() {
      try {
        CreateUserInitiatedRoutedEventPermission().Demand();
      } catch (SecurityException) {
        return false;
      }
      return true;
    }

    [SecuritySafeCritical]
    internal static bool IsFullTrustCaller() {
      try {
        if (_fullTrustPermissionSet == null) {
          _fullTrustPermissionSet = new PermissionSet(PermissionState.Unrestricted);
        }
        _fullTrustPermissionSet.Demand();
      } catch (SecurityException) {
        return false;
      }
      return true;
    }

    [SecuritySafeCritical]
    internal static bool CallerHasPermissionWithAppDomainOptimization(params IPermission[] permissionsToCheck) {
      if (permissionsToCheck == null) {
        return true;
      }
      PermissionSet permissionSet = new PermissionSet(PermissionState.None);
      for (int i = 0; i < permissionsToCheck.Length; i++) {
        permissionSet.AddPermission(permissionsToCheck[i]);
      }
      PermissionSet permissionSet2 = AppDomain.CurrentDomain.PermissionSet;
      if (permissionSet.IsSubsetOf(permissionSet2)) {
        return true;
      }
      return false;
    }

    [SecuritySafeCritical]
    internal static bool AppDomainHasPermission(IPermission permissionToCheck) {
      Invariant.Assert(permissionToCheck != null);
      PermissionSet permissionSet = new PermissionSet(PermissionState.None);
      permissionSet.AddPermission(permissionToCheck);
      return permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);
    }

    [SecurityCritical]
    internal static Uri GetBaseDirectory(AppDomain domain) {
      new FileIOPermission(PermissionState.Unrestricted).Assert();
      try {
        return new Uri(domain.BaseDirectory);
      } finally {
        CodeAccessPermission.RevertAssert();
      }
    }

    internal static Uri ExtractUriForClickOnceDeployedApp() {
      return SiteOfOriginContainer.SiteOfOriginForClickOnceApp;
    }

    [SecurityCritical]
    internal static void BlockCrossDomainForHttpsApps(Uri uri) {
      Uri uri2 = ExtractUriForClickOnceDeployedApp();
      if (uri2 != null && uri2.Scheme == Uri.UriSchemeHttps) {
        if (uri.IsUnc || uri.IsFile) {
          new FileIOPermission(FileIOPermissionAccess.Read, uri.LocalPath).Demand();
        } else {
          new WebPermission(NetworkAccess.Connect, BindUriHelper.UriToString(uri)).Demand();
        }
      }
    }

    [SecurityCritical]
    internal static void EnforceUncContentAccessRules(Uri contentUri) {
      Invariant.Assert(contentUri.IsUnc);
      Uri uri = ExtractUriForClickOnceDeployedApp();
      if (!(uri == null)) {
        int num = MapUrlToZoneWrapper(uri);
        bool flag = num >= 3;
        bool flag2 = num == 1 && uri.Scheme == Uri.UriSchemeHttps;
        if (flag | flag2) {
          new FileIOPermission(FileIOPermissionAccess.Read, contentUri.LocalPath).Demand();
        }
      }
    }

    [SecurityCritical]
    internal static int MapUrlToZoneWrapper(Uri uri) {
      int pdwZone = 0;
      int num = 0;
      num = UnsafeNativeMethods.CoInternetCreateSecurityManager(null, out var ppISecurityManager, 0);
      if (MS.Win32.NativeMethods.Failed(num)) {
        throw new Win32Exception(num);
      }
      UnsafeNativeMethods.IInternetSecurityManager internetSecurityManager = (MS.Win32.UnsafeNativeMethods.IInternetSecurityManager)ppISecurityManager;
      string pwszUrl = BindUriHelper.UriToString(uri);
      if (uri.IsFile) {
        internetSecurityManager.MapUrlToZone(pwszUrl, out pdwZone, 1);
      } else {
        internetSecurityManager.MapUrlToZone(pwszUrl, out pdwZone, 0);
      }
      if (pdwZone < 0) {
        throw new SecurityException(SR.Get("Invalid_URI"));
      }
      internetSecurityManager = null;
      ppISecurityManager = null;
      return pdwZone;
    }

    [SecurityCritical]
    internal static void DemandFilePathDiscoveryWriteRead() {
      FileIOPermission fileIOPermission = new FileIOPermission(PermissionState.None) {
        AllFiles = (FileIOPermissionAccess.Read | FileIOPermissionAccess.Write | FileIOPermissionAccess.PathDiscovery)
      };
      fileIOPermission.Demand();
    }

    [SecurityCritical]
    internal static PermissionSet ExtractAppDomainPermissionSetMinusSiteOfOrigin() {
      PermissionSet permissionSet = AppDomain.CurrentDomain.PermissionSet;
      Uri siteOfOrigin = SiteOfOriginContainer.SiteOfOrigin;
      CodeAccessPermission codeAccessPermission = null;
      if (siteOfOrigin.Scheme == Uri.UriSchemeFile) {
        codeAccessPermission = new FileIOPermission(PermissionState.Unrestricted);
      } else if (siteOfOrigin.Scheme == Uri.UriSchemeHttp) {
        codeAccessPermission = new WebPermission(PermissionState.Unrestricted);
      }
      if (codeAccessPermission != null && permissionSet.GetPermission(codeAccessPermission.GetType()) != null) {
        permissionSet.RemovePermission(codeAccessPermission.GetType());
      }
      return permissionSet;
    }

    [SecuritySafeCritical]
    internal static bool CallerHasSerializationPermission() {
      try {
        if (_serializationSecurityPermission == null) {
          _serializationSecurityPermission = new SecurityPermission(SecurityPermissionFlag.SerializationFormatter);
        }
        _serializationSecurityPermission.Demand();
      } catch (SecurityException) {
        return false;
      }
      return true;
    }

    [SecuritySafeCritical]
    internal static bool CallerHasAllClipboardPermission() {
      try {
        DemandAllClipboardPermission();
      } catch (SecurityException) {
        return false;
      }
      return true;
    }

    [SecurityCritical]
    internal static void DemandAllClipboardPermission() {
      if (_uiPermissionAllClipboard == null) {
        _uiPermissionAllClipboard = new UIPermission(UIPermissionClipboard.AllClipboard);
      }
      _uiPermissionAllClipboard.Demand();
    }

    [SecurityCritical]
    internal static void DemandPathDiscovery(string path) {
      new FileIOPermission(FileIOPermissionAccess.PathDiscovery, path).Demand();
    }

    [SecuritySafeCritical]
    internal static bool CheckEnvironmentPermission() {
      try {
        DemandEnvironmentPermission();
      } catch (SecurityException) {
        return false;
      }
      return true;
    }

    [SecurityCritical]
    internal static void DemandEnvironmentPermission() {
      if (_unrestrictedEnvironmentPermission == null) {
        _unrestrictedEnvironmentPermission = new EnvironmentPermission(PermissionState.Unrestricted);
      }
      _unrestrictedEnvironmentPermission.Demand();
    }

    [SecurityCritical]
    internal static void DemandUriDiscoveryPermission(Uri uri) {
      CreateUriDiscoveryPermission(uri)?.Demand();
    }

    [SecurityCritical]
    internal static CodeAccessPermission CreateUriDiscoveryPermission(Uri uri) {
      if (uri.GetType().IsSubclassOf(typeof(Uri))) {
        DemandInfrastructurePermission();
      }
      if (uri.IsFile) {
        return new FileIOPermission(FileIOPermissionAccess.PathDiscovery, uri.LocalPath);
      }
      return null;
    }

    [SecurityCritical]
    internal static CodeAccessPermission CreateUriReadPermission(Uri uri) {
      if (uri.GetType().IsSubclassOf(typeof(Uri))) {
        DemandInfrastructurePermission();
      }
      if (uri.IsFile) {
        return new FileIOPermission(FileIOPermissionAccess.Read, uri.LocalPath);
      }
      return null;
    }

    [SecurityCritical]
    internal static void DemandUriReadPermission(Uri uri) {
      CreateUriReadPermission(uri)?.Demand();
    }

    [SecuritySafeCritical]
    internal static bool CallerHasPathDiscoveryPermission(string path) {
      try {
        DemandPathDiscovery(path);
        return true;
      } catch (SecurityException) {
        return false;
      }
    }

    [SecurityCritical]
    private static PermissionSet CreateEnvelopePermissionSet() {
      PermissionSet permissionSet = new PermissionSet(PermissionState.None);
      permissionSet.AddPermission(new RightsManagementPermission());
      permissionSet.AddPermission(new CompoundFileIOPermission());
      return permissionSet;
    }

    [SecuritySafeCritical]
    internal static Exception GetExceptionForHR(int hr) {
      return Marshal.GetExceptionForHR(hr, new IntPtr(-1));
    }

    [SecuritySafeCritical]
    internal static void ThrowExceptionForHR(int hr) {
      Marshal.ThrowExceptionForHR(hr, new IntPtr(-1));
    }

    [SecuritySafeCritical]
    internal static int GetHRForException(Exception exception) {
      if (exception == null) {
        throw new ArgumentNullException("exception");
      }
      int hRForException = Marshal.GetHRForException(exception);
      Marshal.GetHRForException(new Exception());
      return hRForException;
    }

    [SecurityCritical]
    internal static void DemandRegistryPermission() {
      if (_unrestrictedRegistryPermission == null) {
        _unrestrictedRegistryPermission = new RegistryPermission(PermissionState.Unrestricted);
      }
      _unrestrictedRegistryPermission.Demand();
    }

    [SecurityCritical]
    internal static void DemandUIWindowPermission() {
      if (_allWindowsUIPermission == null) {
        _allWindowsUIPermission = new UIPermission(UIPermissionWindow.AllWindows);
      }
      _allWindowsUIPermission.Demand();
    }

    [SecurityCritical]
    internal static void DemandInfrastructurePermission() {
      if (_infrastructurePermission == null) {
        _infrastructurePermission = new SecurityPermission(SecurityPermissionFlag.Infrastructure);
      }
      _infrastructurePermission.Demand();
    }

    [SecurityCritical]
    internal static void DemandMediaPermission(MediaPermissionAudio audioPermissionToDemand, MediaPermissionVideo videoPermissionToDemand, MediaPermissionImage imagePermissionToDemand) {
      new MediaPermission(audioPermissionToDemand, videoPermissionToDemand, imagePermissionToDemand).Demand();
    }

    [SecuritySafeCritical]
    internal static bool CallerHasMediaPermission(MediaPermissionAudio audioPermissionToDemand, MediaPermissionVideo videoPermissionToDemand, MediaPermissionImage imagePermissionToDemand) {
      try {
        new MediaPermission(audioPermissionToDemand, videoPermissionToDemand, imagePermissionToDemand).Demand();
        return true;
      } catch (SecurityException) {
        return false;
      }
    }

    [SecurityCritical]
    internal static void DemandUnrestrictedUIPermission() {
      if (_unrestrictedUIPermission == null) {
        _unrestrictedUIPermission = new UIPermission(PermissionState.Unrestricted);
      }
      _unrestrictedUIPermission.Demand();
    }

    [SecurityCritical]
    internal static void DemandFileIOReadPermission(string fileName) {
      new FileIOPermission(FileIOPermissionAccess.Read, fileName).Demand();
    }

    [SecurityCritical]
    internal static void DemandMediaAccessPermission(string uri) {
      CreateMediaAccessPermission(uri)?.Demand();
    }

    [SecurityCritical]
    internal static CodeAccessPermission CreateMediaAccessPermission(string uri) {
      CodeAccessPermission result = null;
      if (uri != null) {
        if (string.Compare("image", uri, true, TypeConverterHelper.InvariantEnglishUS) == 0) {
          result = new MediaPermission(MediaPermissionAudio.NoAudio, MediaPermissionVideo.NoVideo, MediaPermissionImage.AllImage);
        } else if (string.Compare(BaseUriHelper.GetResolvedUri(BaseUriHelper.BaseUri, new Uri(uri, UriKind.RelativeOrAbsolute)).Scheme, PackUriHelper.UriSchemePack, true, TypeConverterHelper.InvariantEnglishUS) != 0 && !CallerHasWebPermission(new Uri(uri, UriKind.RelativeOrAbsolute))) {
          result = new MediaPermission(MediaPermissionAudio.NoAudio, MediaPermissionVideo.NoVideo, MediaPermissionImage.AllImage);
        }
      } else {
        result = new MediaPermission(MediaPermissionAudio.NoAudio, MediaPermissionVideo.NoVideo, MediaPermissionImage.AllImage);
      }
      return result;
    }

    [SecuritySafeCritical]
    internal static bool CallerHasWebPermission(Uri uri) {
      try {
        DemandWebPermission(uri);
        return true;
      } catch (SecurityException) {
        return false;
      }
    }

    [SecurityCritical]
    internal static void DemandWebPermission(Uri uri) {
      string uriString = BindUriHelper.UriToString(uri);
      if (uri.IsFile) {
        string localPath = uri.LocalPath;
        new FileIOPermission(FileIOPermissionAccess.Read, localPath).Demand();
      } else {
        new WebPermission(NetworkAccess.Connect, uriString).Demand();
      }
    }

    [SecurityCritical]
    internal static void DemandPlugInSerializerPermissions() {
      if (_plugInSerializerPermissions == null) {
        _plugInSerializerPermissions = new PermissionSet(PermissionState.Unrestricted);
      }
      _plugInSerializerPermissions.Demand();
    }

    internal static bool AreStringTypesEqual(string m1, string m2) {
      return string.Compare(m1, m2, StringComparison.OrdinalIgnoreCase) == 0;
    }
  }
}
