using Microsoft.WindowsAPICodePack.Internal;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem {
  [ComImport,
  Guid(ShellIIDGuid.IPropertyEnumType2),
  InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IPropertyEnumType2 : IPropertyEnumType {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetEnumType([Out] out PropEnumType penumtype);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetValue([Out] PropVariant ppropvar);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetRangeMinValue([Out] PropVariant ppropvar);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetRangeSetValue([Out] PropVariant ppropvar);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetDisplayText([Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplay);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetImageReference([Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszImageRes);
  }
}