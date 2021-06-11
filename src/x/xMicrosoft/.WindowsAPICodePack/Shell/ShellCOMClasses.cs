

using System;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Shell {
  [ComImport,
  Guid(ShellIIDGuid.IShellLibrary),
  CoClass(typeof(ShellLibraryCoClass))]
  public interface INativeShellLibrary : IShellLibrary {
  }

  [ComImport,
  ClassInterface(ClassInterfaceType.None),
  //TODO  TypeLibType(TypeLibTypeFlags.FCanCreate),
  Guid(ShellCLSIDGuid.ShellLibrary)]
  public class ShellLibraryCoClass {
  }
}
