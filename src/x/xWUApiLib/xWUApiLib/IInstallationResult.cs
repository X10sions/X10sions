using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [TypeLibType(4288)]
  [Guid("A43C56D6-7451-48D4-AF96-B6CD2D0D9B7A")]
  public interface IInstallationResult {
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

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743812)]
    [return: MarshalAs(UnmanagedType.Interface)]
    IUpdateInstallationResult GetUpdateResult([In] int updateIndex);
  }
}
