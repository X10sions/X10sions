using Microsoft.WindowsAPICodePack.Internal;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem {
  [ComImport,
  Guid(ShellIIDGuid.IPropertyDescription2),
  InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IPropertyDescription2 : IPropertyDescription {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetPropertyKey(out PropertyKey pkey);
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetCanonicalName([MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetPropertyType(out VarEnum pvartype);
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetDisplayName([MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetEditInvitation([MarshalAs(UnmanagedType.LPWStr)] out string ppszInvite);
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetTypeFlags([In] PropertyTypeOptions mask, out PropertyTypeOptions ppdtFlags);
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetViewFlags(out PropertyViewOptions ppdvFlags);
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetDefaultColumnWidth(out uint pcxChars);
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetDisplayType(out PropertyDisplayType pdisplaytype);
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetColumnState(out uint pcsFlags);
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetGroupingRange(out PropertyGroupingRange pgr);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetRelativeDescriptionType(out PropertySystemNativeMethods.RelativeDescriptionType prdt);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetRelativeDescription(
       [In] PropVariant propvar1,
       [In] PropVariant propvar2,
       [MarshalAs(UnmanagedType.LPWStr)] out string ppszDesc1,
       [MarshalAs(UnmanagedType.LPWStr)] out string ppszDesc2);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetSortDescription(out PropertySortDescription psd);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetSortDescriptionLabel([In] int fDescending, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDescription);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetAggregationType(out PropertyAggregationType paggtype);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetConditionType(
        out PropertyConditionType pcontype,
        out PropertyConditionOperation popDefault);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetEnumTypeList([In] ref Guid riid, out IntPtr ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void CoerceToCanonicalValue([In, Out] PropVariant ppropvar);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void FormatForDisplay([In] PropVariant propvar, [In] ref PropertyDescriptionFormatOptions pdfFlags, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplay);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new HResult IsValueCanonical([In] PropVariant propvar);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetImageReferenceForValue(
        [In] PropVariant propvar,
        [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszImageRes);
  }
}