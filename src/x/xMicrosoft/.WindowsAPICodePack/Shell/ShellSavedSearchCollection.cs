

using Microsoft.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell {
  /// <summary>
  /// Represents a saved search
  /// </summary>
  public class ShellSavedSearchCollection : ShellSearchCollection {
    internal ShellSavedSearchCollection(IShellItem2 shellItem)
        : base(shellItem) {
      CoreHelpers.ThrowIfNotVista();
    }
  }
}
