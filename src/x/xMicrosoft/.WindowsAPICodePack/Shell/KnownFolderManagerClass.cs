

using Microsoft.WindowsAPICodePack.Internal;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Shell {

  [ComImport]
  [Guid("4df0c730-df9d-4ae3-9153-aa6b82e9795a")]
  public class KnownFolderManagerClass : IKnownFolderManager {

    [MethodImpl(MethodImplOptions.InternalCall,
        MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void FolderIdFromCsidl(int csidl,
        [Out] out Guid knownFolderID);

    [MethodImpl(MethodImplOptions.InternalCall,
        MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void FolderIdToCsidl(
        [In, MarshalAs(UnmanagedType.LPStruct)] Guid id,
        [Out] out int csidl);

    [MethodImpl(MethodImplOptions.InternalCall,
        MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void GetFolderIds(
        [Out] out IntPtr folders,
        [Out] out uint count);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall,
        MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual HResult GetFolder(
        [In, MarshalAs(UnmanagedType.LPStruct)] Guid id,
        [Out, MarshalAs(UnmanagedType.Interface)]
              out IKnownFolderNative knownFolder);

    [MethodImpl(MethodImplOptions.InternalCall,
        MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void GetFolderByName(
        string canonicalName,
        [Out, MarshalAs(UnmanagedType.Interface)] out IKnownFolderNative knownFolder);

    [MethodImpl(MethodImplOptions.InternalCall,
        MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void RegisterFolder(
        [In, MarshalAs(UnmanagedType.LPStruct)] Guid knownFolderGuid,
        [In] ref KnownFoldersSafeNativeMethods.NativeFolderDefinition knownFolderDefinition);

    [MethodImpl(MethodImplOptions.InternalCall,
        MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void UnregisterFolder(
        [In, MarshalAs(UnmanagedType.LPStruct)] Guid knownFolderGuid);

    [MethodImpl(MethodImplOptions.InternalCall,
        MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void FindFolderFromPath(
        [In, MarshalAs(UnmanagedType.LPWStr)] string path,
        [In] int mode,
        [Out, MarshalAs(UnmanagedType.Interface)] out IKnownFolderNative knownFolder);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall,
        MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual HResult FindFolderFromIDList(IntPtr pidl, [Out, MarshalAs(UnmanagedType.Interface)] out IKnownFolderNative knownFolder);

    [MethodImpl(MethodImplOptions.InternalCall,
        MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void Redirect();
  }
}
