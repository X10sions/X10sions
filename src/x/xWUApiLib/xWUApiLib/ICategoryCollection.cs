using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [Guid("3A56BFB8-576C-43F7-9335-FE4838FD7E37")]
  [TypeLibType(4288)]
  public interface ICategoryCollection : IEnumerable {
    [DispId(0)]
    ICategory this[[In] int index] {
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
