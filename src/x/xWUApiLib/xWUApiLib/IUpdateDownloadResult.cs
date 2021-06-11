using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [TypeLibType(4288)]
  [Guid("BF99AF76-B575-42AD-8AA4-33CBB5477AF1")]
  public interface IUpdateDownloadResult {
    [DispId(1610743809)]
    int HResult {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      get;
    }

    [ComAliasName("WUApiLib.OperationResultCode")]
    [DispId(1610743810)]
    OperationResultCode ResultCode {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      [return: ComAliasName("WUApiLib.OperationResultCode")]
      get;
    }
  }


}
