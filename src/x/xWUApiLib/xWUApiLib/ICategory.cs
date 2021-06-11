using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [Guid("81DDC1B8-9D35-47A6-B471-5B80F519223B")]
  [TypeLibType(4288)]
  [DefaultMember("Name")]
  public interface ICategory {
    [DispId(0)]
    string Name {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(0)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743809)]
    string CategoryID {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743810)]
    ICategoryCollection Children {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [DispId(1610743811)]
    string Description {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743812)]
    IImageInformation Image {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743812)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [DispId(1610743813)]
    int Order {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743813)]
      get;
    }

    [DispId(1610743814)]
    ICategory Parent {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743814)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [DispId(1610743815)]
    string Type {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743815)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743816)]
    UpdateCollection Updates {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743816)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }
  }


}
