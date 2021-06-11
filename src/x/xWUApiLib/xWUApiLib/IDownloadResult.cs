using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [Guid("DAA4FDD0-4727-4DBE-A1E7-745DCA317144")]
  [TypeLibType(4288)]
  public interface IDownloadResult {
    [DispId(1610743809)]
    int HResult {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      get;
    }

    [DispId(1610743810)]
    [ComAliasName("WUApiLib.OperationResultCode")]
    OperationResultCode ResultCode {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      [return: ComAliasName("WUApiLib.OperationResultCode")]
      get;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743811)]
    [return: MarshalAs(UnmanagedType.Interface)]
    IUpdateDownloadResult GetUpdateResult([In] int updateIndex);
  }


}
