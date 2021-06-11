using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [TypeLibType(4288)]
  [DefaultMember("Name")]
  [Guid("76B3B17E-AED6-4DA5-85F0-83587F81ABE3")]
  public interface IUpdateService {
    [DispId(0)]
    string Name {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(0)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743809)]
    object ContentValidationCert {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      [return: MarshalAs(UnmanagedType.Struct)]
      get;
    }

    [DispId(1610743810)]
    DateTime ExpirationDate {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      get;
    }

    [DispId(1610743811)]
    bool IsManaged {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      get;
    }

    [DispId(1610743812)]
    bool IsRegisteredWithAU {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743812)]
      get;
    }

    [DispId(1610743813)]
    DateTime IssueDate {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743813)]
      get;
    }

    [DispId(1610743814)]
    bool OffersWindowsUpdates {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743814)]
      get;
    }

    [DispId(1610743815)]
    StringCollection RedirectUrls {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743815)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [DispId(1610743816)]
    string ServiceID {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743816)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743818)]
    bool IsScanPackageService {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743818)]
      get;
    }

    [DispId(1610743819)]
    bool CanRegisterWithAU {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743819)]
      get;
    }

    [DispId(1610743820)]
    string ServiceUrl {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743820)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743821)]
    string SetupPrefix {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743821)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }
  }

}
