using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [Guid("54A2CB2D-9A0C-48B6-8A50-9ABB69EE2D02")]
  [TypeLibType(4288)]
  public interface IUpdateDownloadContent {
    [DispId(1610743809)]
    string DownloadUrl {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }
  }

}
