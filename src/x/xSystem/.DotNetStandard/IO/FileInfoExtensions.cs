using System.Diagnostics;

namespace System.IO;
public static class FileInfoExtensions {

  public static void CopyTo(this FileInfo f, FileInfo target, bool overwrite = false) => f.CopyTo(target.FullName, overwrite).Refresh(true);
  public static void CopyTo(this FileInfo f, DirectoryInfo dir, bool overwrite = false) => f.CopyTo(Path.Combine(dir.FullName, f.Name), overwrite).Refresh(true);

  public static bool StartProcessAsAdmin(this FileInfo f) {
    var proc = new Process();
    proc.StartInfo.FileName = f.FullName;
    proc.StartInfo.UseShellExecute = true;
    proc.StartInfo.Verb = "runas";
    return proc.Start();
  }

  public static string FullNameWithEnvironment(this FileInfo fi, string environmentName) => fi.FullName.Replace(fi.Extension, $".{environmentName}{fi.Extension}");
  public static string NameWithoutExtension(this FileInfo f) => Path.GetFileNameWithoutExtension(f.Name);
  public static string ReadAllText(this FileInfo fileInfo) => File.ReadAllText(fileInfo.FullName);

  public static void RemoveReadonlyFlagFromFile(this FileInfo fileInfo) {
    if (fileInfo.Exists && fileInfo.IsReadOnly) fileInfo.IsReadOnly = false;
  }

  static FileInfo Refresh(this FileInfo f, bool doRefresh) {
    if (doRefresh) {
      f.Refresh();
    }
    return f;
  }
  
  public static string SafeName(this FileInfo f, string replacementString = "", string? spaceReplacement = null) {
    var file = Path.GetInvalidFileNameChars().Aggregate(f.Name.Trim(), (current, c) => current.Replace(c.ToString(), replacementString));
    file = file.Replace("#", "");
    if (!string.IsNullOrEmpty(spaceReplacement))
      file = file.Replace(" ", spaceReplacement);
    return file;
  }

  public static string ToBreadCrumbLinks(this FileInfo fileInfo, string? removeBasePath = null) {
    var nameWithExtension = fileInfo.Name.Replace(fileInfo.Extension, "");
    var path = fileInfo.DirectoryName.Replace(removeBasePath, "");
    var partPaths = path.Split(@"\");
    var s = new StringBuilder();
    var href = new StringBuilder();
    foreach (var partPath in partPaths) {
      href.Append(partPath + "/");
      s.Append($"<a href='{href}'>{partPath}</a> : ");
    }
    s.Append($"<a href='{href}{nameWithExtension}'>{nameWithExtension}</a>");
    return s.ToString();
  }

  public static FileInfo WriteAll(this FileInfo file, byte[] contents) {
    File.WriteAllBytes(file.FullName, contents);
    return file.Refresh(true);
  }

  public static FileInfo WriteAll(this FileInfo file, string[] contents) {
    File.WriteAllLines(file.FullName, contents);
    return file.Refresh(true);
  }

  public static FileInfo WriteAll(this FileInfo file, string contents) {
    File.WriteAllText(file.FullName, contents);
    return file.Refresh(true);
  }
}
