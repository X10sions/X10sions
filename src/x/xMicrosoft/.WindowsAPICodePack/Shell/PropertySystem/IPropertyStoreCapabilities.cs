using Microsoft.WindowsAPICodePack.Internal;
using System;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem {
  // Disable warning if a method declaration hides another inherited from a parent COM interface
  // To successfully import a COM interface, all inherited methods need to be declared again with 
  // the exception of those already declared in "IUnknown"
#pragma warning disable 108

  #region Property System COM Interfaces

  [ComImport]
  [Guid(ShellIIDGuid.IPropertyStoreCapabilities)]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IPropertyStoreCapabilities {
    HResult IsPropertyWritable([In]ref PropertyKey propertyKey);
  }

  #endregion

#pragma warning restore 108
}
