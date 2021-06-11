using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [Guid("BC5513C8-B3B8-4BF7-A4D4-361C0D8C88BA")]
  [TypeLibType(4288)]
  public interface IUpdateDownloadContentCollection : IEnumerable {
    [DispId(0)]
    IUpdateDownloadContent this[[In] int index] {
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
