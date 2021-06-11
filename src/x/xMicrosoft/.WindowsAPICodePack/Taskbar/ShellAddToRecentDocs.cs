namespace Microsoft.WindowsAPICodePack.Taskbar {
  public enum ShellAddToRecentDocs {
    Pidl = 0x1,
    PathA = 0x2,
    PathW = 0x3,
    AppIdInfo = 0x4,       // indicates the data type is a pointer to a SHARDAPPIDINFO structure
    AppIdInfoIdList = 0x5, // indicates the data type is a pointer to a SHARDAPPIDINFOIDLIST structure
    Link = 0x6,            // indicates the data type is a pointer to an IShellLink instance
    AppIdInfoLink = 0x7,   // indicates the data type is a pointer to a SHARDAPPIDINFOLINK structure 
  }
}
