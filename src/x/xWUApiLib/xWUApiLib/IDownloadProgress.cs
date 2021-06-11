using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [Guid("D31A5BAC-F719-4178-9DBB-5E2CB47FD18A")]
  [TypeLibType(4288)]
  public interface IDownloadProgress {
    [DispId(1610743809)]
    decimal CurrentUpdateBytesDownloaded {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      get;
    }

    [DispId(1610743810)]
    decimal CurrentUpdateBytesToDownload {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      get;
    }

    [DispId(1610743811)]
    int CurrentUpdateIndex {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      get;
    }

    [DispId(1610743812)]
    int PercentComplete {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743812)]
      get;
    }

    [DispId(1610743813)]
    decimal TotalBytesDownloaded {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743813)]
      get;
    }

    [DispId(1610743814)]
    decimal TotalBytesToDownload {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743814)]
      get;
    }

    [DispId(1610743816)]
    [ComAliasName("WUApiLib.DownloadPhase")]
    DownloadPhase CurrentUpdateDownloadPhase {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743816)]
      [return: ComAliasName("WUApiLib.DownloadPhase")]
      get;
    }

    [DispId(1610743817)]
    int CurrentUpdatePercentComplete {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743817)]
      get;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743815)]
    [return: MarshalAs(UnmanagedType.Interface)]
    IUpdateDownloadResult GetUpdateResult([In] int updateIndex);
  }


}
