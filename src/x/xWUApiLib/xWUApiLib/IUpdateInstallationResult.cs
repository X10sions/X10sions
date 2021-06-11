using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [TypeLibType(4288)]
  [Guid("D940F0F8-3CBB-4FD0-993F-471E7F2328AD")]
  public interface IUpdateInstallationResult {
    [DispId(1610743809)]
    int HResult {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      get;
    }

    [DispId(1610743810)]
    bool RebootRequired {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      get;
    }

    [ComAliasName("WUApiLib.OperationResultCode")]
    [DispId(1610743811)]
    OperationResultCode ResultCode {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      [return: ComAliasName("WUApiLib.OperationResultCode")]
      get;
    }
  }

}
