using System;
using System.Diagnostics;
using System.IO;

namespace Common.InstalledSoftwareInfo {
  public class SoftwareInfo {
    public SoftwareInfo(string path) {
      var fileInfo = new FileInfo(path);
      if (fileInfo.Exists) {
        FileVersionInfo = FileVersionInfo.GetVersionInfo(fileInfo.FullName);
        CreationTimeUtc = fileInfo.CreationTimeUtc;
        LastAccessTimeUtc = fileInfo.LastAccessTimeUtc;
        LastWriteTimeUtc = fileInfo.LastWriteTimeUtc;
      }
    }

    public FileVersionInfo FileVersionInfo { get; }
    public DateTimeOffset CreationTimeUtc { get; }
    public DateTimeOffset LastAccessTimeUtc { get; }
    public DateTimeOffset LastWriteTimeUtc { get; }
  }
}
