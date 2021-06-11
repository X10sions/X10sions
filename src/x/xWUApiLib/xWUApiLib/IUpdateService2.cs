using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [DefaultMember("Name")]
  [Guid("1518B460-6518-4172-940F-C75883B24CEB")]
  [TypeLibType(4288)]
  public interface IUpdateService2 : IUpdateService {
    [DispId(0)]
    new string Name {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(0)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743809)]
    new object ContentValidationCert {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      [return: MarshalAs(UnmanagedType.Struct)]
      get;
    }

    [DispId(1610743810)]
    new DateTime ExpirationDate {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      get;
    }

    [DispId(1610743811)]
    new bool IsManaged {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      get;
    }

    [DispId(1610743812)]
    new bool IsRegisteredWithAU {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743812)]
      get;
    }

    [DispId(1610743813)]
    new DateTime IssueDate {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743813)]
      get;
    }

    [DispId(1610743814)]
    new bool OffersWindowsUpdates {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743814)]
      get;
    }

    [DispId(1610743815)]
    new StringCollection RedirectUrls {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743815)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [DispId(1610743816)]
    new string ServiceID {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743816)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743818)]
    new bool IsScanPackageService {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743818)]
      get;
    }

    [DispId(1610743819)]
    new bool CanRegisterWithAU {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743819)]
      get;
    }

    [DispId(1610743820)]
    new string ServiceUrl {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743820)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743821)]
    new string SetupPrefix {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743821)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610809345)]
    bool IsDefaultAUService {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610809345)]
      get;
    }
  }


}
