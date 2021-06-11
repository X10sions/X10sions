using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [TypeLibType(4304)]
  [Guid("918EFD1E-B5D8-4C90-8540-AEB9BDC56F9D")]
  public interface IUpdateSession3 : IUpdateSession2 {
    [DispId(1610743809)]
    new string ClientApplicationID {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      [param: In]
      [param: MarshalAs(UnmanagedType.BStr)]
      set;
    }

    [DispId(1610743810)]
    new bool ReadOnly {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      get;
    }

    [DispId(1610743811)]
    new WebProxy WebProxy {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      [param: In]
      [param: MarshalAs(UnmanagedType.Interface)]
      set;
    }

    [DispId(1610809345)]
    new uint UserLocale {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610809345)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610809345)]
      [param: In]
      set;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743812)]
    [return: MarshalAs(UnmanagedType.Interface)]
    new IUpdateSearcher CreateUpdateSearcher();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743813)]
    [return: MarshalAs(UnmanagedType.Interface)]
    new UpdateDownloader CreateUpdateDownloader();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743814)]
    [return: MarshalAs(UnmanagedType.Interface)]
    new IUpdateInstaller CreateUpdateInstaller();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610874881)]
    [return: MarshalAs(UnmanagedType.Interface)]
    UpdateServiceManager CreateUpdateServiceManager();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610874882)]
    [return: MarshalAs(UnmanagedType.Interface)]
    IUpdateHistoryEntryCollection QueryHistory([In] [MarshalAs(UnmanagedType.BStr)] string criteria, [In] int startIndex, [In] int Count);
  }

}
