using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [TypeLibType(4304)]
  [Guid("0BB8531D-7E8D-424F-986C-A0B8F60A3E7B")]
  public interface IUpdateServiceManager2 : IUpdateServiceManager {
    [DispId(1610743809)]
    new IUpdateServiceCollection Services {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [DispId(1610809345)]
    string ClientApplicationID {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610809345)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610809345)]
      [param: In]
      [param: MarshalAs(UnmanagedType.BStr)]
      set;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743810)]
    [return: MarshalAs(UnmanagedType.Interface)]
    new IUpdateService AddService([In] [MarshalAs(UnmanagedType.BStr)] string ServiceID, [In] [MarshalAs(UnmanagedType.BStr)] string authorizationCabPath);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743811)]
    new void RegisterServiceWithAU([In] [MarshalAs(UnmanagedType.BStr)] string ServiceID);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743812)]
    new void RemoveService([In] [MarshalAs(UnmanagedType.BStr)] string ServiceID);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743813)]
    new void UnregisterServiceWithAU([In] [MarshalAs(UnmanagedType.BStr)] string ServiceID);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743814)]
    [return: MarshalAs(UnmanagedType.Interface)]
    new IUpdateService AddScanPackageService([In] [MarshalAs(UnmanagedType.BStr)] string serviceName, [In] [MarshalAs(UnmanagedType.BStr)] string scanFileLocation, [In] int flags = 0);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610678279)]
    new void SetOption([In] [MarshalAs(UnmanagedType.BStr)] string optionName, [In] [MarshalAs(UnmanagedType.Struct)] object optionValue);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610809346)]
    [return: MarshalAs(UnmanagedType.Interface)]
    IUpdateServiceRegistration QueryServiceRegistration([In] [MarshalAs(UnmanagedType.BStr)] string ServiceID);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610809347)]
    [return: MarshalAs(UnmanagedType.Interface)]
    IUpdateServiceRegistration AddService2([In] [MarshalAs(UnmanagedType.BStr)] string ServiceID, [In] int flags, [In] [MarshalAs(UnmanagedType.BStr)] string authorizationCabPath);
  }

}
