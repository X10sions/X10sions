using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Shell {
  /// <summary>
  /// Structure used internally to store property values for 
  /// a known folder. This structure holds the information
  /// returned in the FOLDER_DEFINITION structure, and 
  /// resources referenced by fields in NativeFolderDefinition,
  /// such as icon and tool tip.
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  public struct FolderProperties {
    internal string name;
    internal FolderCategory category;
    internal string canonicalName;
    internal string description;
    internal Guid parentId;
    internal string parent;
    internal string relativePath;
    internal string parsingName;
    internal string tooltipResourceId;
    internal string tooltip;
    internal string localizedName;
    internal string localizedNameResourceId;
    internal string iconResourceId;
    internal Bitmap icon;
    //internal System.Windows.Media.Imaging.BitmapSource icon2;
    internal DefinitionOptions definitionOptions;
    internal System.IO.FileAttributes fileAttributes;
    internal Guid folderTypeId;
    internal string folderType;
    internal Guid folderId;
    internal string path;
    internal bool pathExists;
    internal RedirectionCapability redirection;
    internal string security;
  }


}
