//using DSOFile;
using System.Linq;
using System.Text;

namespace System.IO {
  public static class FileInfoExtensions {

    public static string FullNameWithEnvironment(this FileInfo fi, string environmentName) => fi.FullName.Replace(fi.Extension, $".{environmentName}{fi.Extension}");

    public static void CopyToDirectory(this FileInfo f, DirectoryInfo dir, bool overwrite = false) => f.CopyTo(Path.Combine(dir.FullName, f.Name), overwrite);

    public static string NameWithoutExtension(this FileInfo f) => Path.GetFileNameWithoutExtension(f.Name);

    public static string SafeName(this FileInfo f, string replacementString = "", string spaceReplacement = null) {
      var file = Path.GetInvalidFileNameChars().Aggregate(f.Name.Trim(),
              (current, c) => current.Replace(c.ToString(), replacementString));
      file = file.Replace("#", "");
      if (!string.IsNullOrEmpty(spaceReplacement))
        file = file.Replace(" ", spaceReplacement);
      return file;
    }

    public static string ToBreadCrumbLinks(this FileInfo fileInfo, string removeBasePath = null) {
      var nameWithExtension = fileInfo.Name.Replace(fileInfo.Extension, "");
      var path = fileInfo.DirectoryName.Replace(removeBasePath, "");
      var partPaths = path.Split(@"\");
      var s = new StringBuilder();
      var href = "";
      foreach (var partPath in partPaths) {
        href += partPath + "/";
        s.Append($"<a href='{href}'>{partPath}</a> : ");
      }
      s.Append($"<a href='{href}{nameWithExtension}'>{nameWithExtension}</a>");
      return s.ToString();
    }

    public static void RemoveReadonlyFlagFromFile(this FileInfo fileInfo) {
      if (fileInfo.Exists && fileInfo.IsReadOnly) fileInfo.IsReadOnly = false;
    }

  }
}
