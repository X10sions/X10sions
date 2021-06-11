using System.Diagnostics;
using System.IO;

namespace System {
  public static class StringExtensions {

    public static FileVersionInfo GetFileVersionInfo(this string _path) => File.Exists(_path) ? FileVersionInfo.GetVersionInfo(_path) : null;

  }
}
