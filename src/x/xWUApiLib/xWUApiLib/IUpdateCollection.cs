using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [Guid("07F7438C-7709-4CA5-B518-91279288134E")]
  [TypeLibType(4304)]
  public interface IUpdateCollection : IEnumerable {
    [DispId(0)]
    IUpdate this[[In] int index] {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(0)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(0)]
      [param: In]
      [param: MarshalAs(UnmanagedType.Interface)]
      set;
    }

    [DispId(1610743809)]
    int Count {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      get;
    }

    [DispId(1610743810)]
    bool ReadOnly {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      get;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(-4)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler")]
    new IEnumerator GetEnumerator();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743811)]
    int Add([In] [MarshalAs(UnmanagedType.Interface)] IUpdate value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743812)]
    void Clear();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743813)]
    [return: MarshalAs(UnmanagedType.Interface)]
    UpdateCollection Copy();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743814)]
    void Insert([In] int index, [In] [MarshalAs(UnmanagedType.Interface)] IUpdate value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743815)]
    void RemoveAt([In] int index);
  }



}
