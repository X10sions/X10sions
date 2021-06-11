using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [TypeLibType(4288)]
  [Guid("7366EA16-7A1A-4EA2-B042-973D3E9CD99B")]
  public interface ISearchJob {
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

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743811)]
    void CleanUp();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743812)]
    void RequestAbort();
  }


}
