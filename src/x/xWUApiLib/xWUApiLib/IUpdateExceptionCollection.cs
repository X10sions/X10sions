using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [Guid("503626A3-8E14-4729-9355-0FE664BD2321")]
  [TypeLibType(4288)]
  public interface IUpdateExceptionCollection : IEnumerable {
    [DispId(0)]
    IUpdateException this[[In] int index] {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(0)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [DispId(1610743809)]
    int Count {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      get;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(-4)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler")]
    new IEnumerator GetEnumerator();
  }


}
