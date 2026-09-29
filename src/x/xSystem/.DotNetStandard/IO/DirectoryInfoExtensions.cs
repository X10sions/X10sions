namespace System.IO;

public static class DirectoryInfoExtensions {

  public static string AddTrailingSlash(this DirectoryInfo di) {
    var separator = Path.DirectorySeparatorChar.ToString();
    var path = di.FullName.TrimEnd();
    if (path.EndsWith(separator, StringComparison.Ordinal) || path.EndsWith(Path.AltDirectorySeparatorChar.ToString(), StringComparison.Ordinal))
      return path;
    return path + separator;
  }

  public static void CopyDirectory(DirectoryInfo source, DirectoryInfo target, bool deleteFirst = false, bool deepCopy = true) {
    if (deleteFirst && target.Exists) {
      target.Delete(true);
    }
    var searchOption = deepCopy ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
    foreach (var dirPath in source.GetDirectories("*", searchOption)) {
      Directory.CreateDirectory(dirPath.FullName.Replace(source.FullName, target.FullName));
    }
    foreach (var newPath in source.GetFiles("*.*", searchOption)) {
      File.Copy(newPath.FullName, newPath.FullName.Replace(source.FullName, target.FullName), true);
    }
  }

  public static FileInfo CreateFile(this DirectoryInfo directory, string fileName, byte[] contents) => directory.GetFileInfo(fileName).WriteAll(contents);
  public static FileInfo CreateFile(this DirectoryInfo directory, string fileName, string contents) => directory.GetFileInfo(fileName).WriteAll(contents);
  public static FileInfo CreateFile(this DirectoryInfo directory, string fileName, string[] contents) => directory.GetFileInfo(fileName).WriteAll(contents);
  public static void DeleteFiles(this DirectoryInfo directoryInfo, string searchPattern = "*", SearchOption searchOption = SearchOption.AllDirectories) {
    foreach (var file in directoryInfo.EnumerateFiles(searchPattern, searchOption)) {
      file.Delete();
    }
  }
  public static void DeleteFilesInTopDirectoryOnly(this DirectoryInfo directoryInfo, string searchPattern = "*") => directoryInfo.DeleteFiles(searchPattern, SearchOption.TopDirectoryOnly);
  public static void DeleteFilesInAllDirectories(this DirectoryInfo directoryInfo, string searchPattern = "*") => directoryInfo.DeleteFiles(searchPattern, SearchOption.AllDirectories);

  public static void EnsureExists(this DirectoryInfo directoryInfo) {
    if (!directoryInfo.Exists) Directory.CreateDirectory(directoryInfo.FullName);
  }

  public static FileInfo GetFileInfo(this DirectoryInfo directoryInfo, string childFilePath) => new FileInfo(Path.Combine(directoryInfo.FullName, childFilePath));
  public static DirectoryInfo GetDirectory(this DirectoryInfo directoryInfo, string childName) => new DirectoryInfo(Path.Combine(directoryInfo.FullName, childName));

  public static DirectoryInfo GetDirectoryInfo(this DirectoryInfo directoryInfo, string childDirectoryName) => new DirectoryInfo(Path.Combine(directoryInfo.FullName, childDirectoryName));

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

}