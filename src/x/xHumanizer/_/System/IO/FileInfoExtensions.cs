using Humanizer;

namespace System.IO {
  public static class FileInfoExtensions {

    public static string CamelCaseSafeName(this FileInfo f) => f.SafeName().Camelize() + Path.GetExtension(f.Name);

  }
}