using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [Guid("91CAF7B0-EB23-49ED-9937-C52D817F46F7")]
  [TypeLibType(4288)]
  public interface IUpdateSession2 : IUpdateSession {
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
    uint UserLocale {
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
  }

}
