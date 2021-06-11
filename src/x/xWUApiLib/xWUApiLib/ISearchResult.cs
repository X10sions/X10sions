using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [Guid("D40CFF62-E08C-4498-941A-01E25F0FD33C")]
  [TypeLibType(4288)]
  public interface ISearchResult {
    [DispId(1610743809)]
    [ComAliasName("WUApiLib.OperationResultCode")]
    OperationResultCode ResultCode {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      [return: ComAliasName("WUApiLib.OperationResultCode")]
      get;
    }

    [DispId(1610743810)]
    ICategoryCollection RootCategories {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [DispId(1610743811)]
    UpdateCollection Updates {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [DispId(1610743812)]
    IUpdateExceptionCollection Warnings {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743812)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }
  }

}
