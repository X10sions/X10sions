using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [TypeLibType(4288)]
  [Guid("5C209F0B-BAD5-432A-9556-4699BED2638A")]
  public interface IInstallationJob {
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
    IInstallationProgress GetProgress();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743814)]
    void RequestAbort();
  }


}
