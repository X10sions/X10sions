

using Microsoft.WindowsAPICodePack.Internal;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Shell {
  // Disable warning if a method declaration hides another inherited from a parent COM interface
  // To successfully import a COM interface, all inherited methods need to be declared again with 
  // the exception of those already declared in "IUnknown"
#pragma warning disable 0108

  [ComImport,
  Guid(KnownFoldersIIDGuid.IKnownFolder),
  InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IKnownFolderNative {
    [MethodImpl(MethodImplOptions.InternalCall,
        MethodCodeType = MethodCodeType.Runtime)]
    Guid GetId();

    [MethodImpl(MethodImplOptions.InternalCall,
        MethodCodeType = MethodCodeType.Runtime)]
    FolderCategory GetCategory();

    [MethodImpl(MethodImplOptions.InternalCall,
        MethodCodeType = MethodCodeType.Runtime)]
    [PreserveSig]
    HResult GetShellItem([In] int i,
         ref Guid interfaceGuid,
         [Out, MarshalAs(UnmanagedType.Interface)] out IShellItem2 shellItem);

    [return: MarshalAs(UnmanagedType.LPWStr)]
    [MethodImpl(MethodImplOptions.InternalCall,
        MethodCodeType = MethodCodeType.Runtime)]
    string GetPath([In] int option);

    [MethodImpl(MethodImplOptions.InternalCall,
        MethodCodeType = MethodCodeType.Runtime)]
    void SetPath([In] int i, [In] string path);

    [MethodImpl(MethodImplOptions.InternalCall,
        MethodCodeType = MethodCodeType.Runtime)]
    void GetIDList([In] int i,
        [Out] out IntPtr itemIdentifierListPointer);

    [MethodImpl(MethodImplOptions.InternalCall,
        MethodCodeType = MethodCodeType.Runtime)]
    Guid GetFolderType();

    [MethodImpl(MethodImplOptions.InternalCall,
        MethodCodeType = MethodCodeType.Runtime)]
    RedirectionCapability GetRedirectionCapabilities();

    [MethodImpl(MethodImplOptions.InternalCall,
        MethodCodeType = MethodCodeType.Runtime)]
    void GetFolderDefinition(
        [Out, MarshalAs(UnmanagedType.Struct)] out KnownFoldersSafeNativeMethods.NativeFolderDefinition definition);

  }
}
