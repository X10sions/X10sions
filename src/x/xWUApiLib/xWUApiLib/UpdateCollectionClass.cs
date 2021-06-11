using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [Guid("13639463-00DB-4646-803D-528026140D88")]
  [ClassInterface(ClassInterfaceType.None)]
  [TypeLibType(2)]
  public class UpdateCollectionClass : UpdateCollection {
    [DispId(0)]
    public extern virtual IUpdate this[[In] int index] {
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
    public extern virtual int Count {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      get;
    }

    [DispId(1610743810)]
    public extern virtual bool ReadOnly {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      get;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(-4)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler")]
    public extern virtual IEnumerator GetEnumerator();


    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743811)]
    public extern virtual int Add([In] [MarshalAs(UnmanagedType.Interface)] IUpdate value);


    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743812)]
    public extern virtual void Clear();


    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743813)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public extern virtual UpdateCollection Copy();


    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743814)]
    public extern virtual void Insert([In] int index, [In] [MarshalAs(UnmanagedType.Interface)] IUpdate value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743815)]
    public extern virtual void RemoveAt([In] int index);


  }
}
