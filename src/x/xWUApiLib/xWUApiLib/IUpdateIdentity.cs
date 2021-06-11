using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [Guid("46297823-9940-4C09-AED9-CD3EA6D05968")]
  [TypeLibType(4288)]
  public interface IUpdateIdentity {
    [DispId(1610743810)]
    int RevisionNumber {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      get;
    }

    [DispId(1610743811)]
    string UpdateID {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }
  }


}
