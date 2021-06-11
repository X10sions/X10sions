//using DSOFile;

namespace System.IO {

  public static class DirectoryInfoExtensions {

    public static string AddTrailingSlash(this DirectoryInfo di) {
      var separator = Path.DirectorySeparatorChar.ToString();
      var path = di.FullName.TrimEnd();
      if (path.EndsWith(separator, StringComparison.Ordinal) || path.EndsWith(Path.AltDirectorySeparatorChar.ToString(), StringComparison.Ordinal))
        return path;
      return path + separator;
    }

    public static DirectoryInfo GetChildDirectoryInfo(this DirectoryInfo di, string childDirectoryName) => new DirectoryInfo(Path.Combine(di.FullName, childDirectoryName));

    public static FileInfo GetChildFileInfo(this DirectoryInfo di, string childFileName) => new FileInfo(Path.Combine(di.FullName, childFileName));

    public static string NormalizeDirectory(this DirectoryInfo di) {
      var path = di.NormalizePath();
      if (!path.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
        path += Path.DirectorySeparatorChar;
      return path;
    }




    public static string NormalizePath(this DirectoryInfo di) {
      var slash = Path.DirectorySeparatorChar;
      var path = di.FullName.Replace('/', slash).Replace('\\', slash);
      return path.Replace(slash.ToString() + slash.ToString(), slash.ToString());
    }

    public static void CopyDirectory(DirectoryInfo di, string targetPath, bool deleteFirst = false, bool deepCopy = true) {
      if (deleteFirst && Directory.Exists(targetPath))
        Directory.Delete(targetPath, true);
      var searchOption = deepCopy ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
      foreach (var dirPath in di.GetDirectories("*", searchOption)) {
        Directory.CreateDirectory(dirPath.FullName.Replace(di.FullName, targetPath));
      }
      foreach (var newPath in di.GetFiles("*.*", searchOption)) {
        File.Copy(newPath.FullName, newPath.FullName.Replace(di.FullName, targetPath), true);
      }
    }

  }
}
