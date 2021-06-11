using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {

  [ComImport]
  [TypeLibType(4288)]
  [Guid("816858A4-260D-4260-933A-2585F1ABC76B")]
  public interface IUpdateSession {
    [DispId(1610743809)]
    string ClientApplicationID {
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
    bool ReadOnly {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      get;
    }

    [DispId(1610743811)]
    WebProxy WebProxy {
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

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743812)]
    [return: MarshalAs(UnmanagedType.Interface)]
    IUpdateSearcher CreateUpdateSearcher();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743813)]
    [return: MarshalAs(UnmanagedType.Interface)]
    UpdateDownloader CreateUpdateDownloader();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743814)]
    [return: MarshalAs(UnmanagedType.Interface)]
    IUpdateInstaller CreateUpdateInstaller();
  }

}
