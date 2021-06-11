using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [Guid("68F1C6F9-7ECC-4666-A464-247FE12496C3")]
  [TypeLibType(4304)]
  public interface IUpdateDownloader {
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
    bool IsForced {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      [param: In]
      set;
    }

    [ComAliasName("WUApiLib.DownloadPriority")]
    [DispId(1610743811)]
    DownloadPriority Priority {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      [return: ComAliasName("WUApiLib.DownloadPriority")]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      [param: In]
      [param: ComAliasName("WUApiLib.DownloadPriority")]
      set;
    }

    [DispId(1610743812)]
    UpdateCollection Updates {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743812)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743812)]
      [param: In]
      [param: MarshalAs(UnmanagedType.Interface)]
      set;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743813)]
    [return: MarshalAs(UnmanagedType.Interface)]
    IDownloadJob BeginDownload([In] [MarshalAs(UnmanagedType.IUnknown)] object onProgressChanged, [In] [MarshalAs(UnmanagedType.IUnknown)] object onCompleted, [In] [MarshalAs(UnmanagedType.Struct)] object state);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743814)]
    [return: MarshalAs(UnmanagedType.Interface)]
    IDownloadResult Download();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743815)]
    [return: MarshalAs(UnmanagedType.Interface)]
    IDownloadResult EndDownload([In] [MarshalAs(UnmanagedType.Interface)] IDownloadJob value);
  }


}
