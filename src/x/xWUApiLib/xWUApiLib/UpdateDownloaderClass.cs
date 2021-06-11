using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [TypeLibType(2)]
  [ClassInterface(ClassInterfaceType.None)]
  [Guid("5BAF654A-5A07-4264-A255-9FF54C7151E7")]
  public class UpdateDownloaderClass : IUpdateDownloader, UpdateDownloader {
    [DispId(1610743809)]
    public extern virtual string ClientApplicationID {
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
    public extern virtual bool IsForced {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      [param: In]
      set;
    }

    [DispId(1610743811)]
    [ComAliasName("WUApiLib.DownloadPriority")]
    public extern virtual DownloadPriority Priority {
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
    public extern virtual UpdateCollection Updates {
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
    public extern virtual IDownloadJob BeginDownload([In] [MarshalAs(UnmanagedType.IUnknown)] object onProgressChanged, [In] [MarshalAs(UnmanagedType.IUnknown)] object onCompleted, [In] [MarshalAs(UnmanagedType.Struct)] object state);



    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743814)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public extern virtual IDownloadResult Download();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743815)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public extern virtual IDownloadResult EndDownload([In] [MarshalAs(UnmanagedType.Interface)] IDownloadJob value);

  }
}