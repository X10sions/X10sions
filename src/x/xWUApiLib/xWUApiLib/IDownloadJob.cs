using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [Guid("C574DE85-7358-43F6-AAE8-8697E62D8BA7")]
  [TypeLibType(4288)]
  public interface IDownloadJob {
    [DispId(1610743809)]
    object AsyncState {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      [return: MarshalAs(UnmanagedType.Struct)]
      get;
    }

    [DispId(1610743810)]
    bool IsCompleted {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      get;
    }

    [DispId(1610743811)]
    UpdateCollection Updates {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743812)]
    void CleanUp();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743813)]
    [return: MarshalAs(UnmanagedType.Interface)]
    IDownloadProgress GetProgress();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743814)]
    void RequestAbort();
  }


}
