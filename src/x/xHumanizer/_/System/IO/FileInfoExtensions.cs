namespace System.IO {
  public static class FileInfoExtensions {

    public static string CamelCaseSafeName(this FileInfo f) => f.SafeName().ToCamelCase() + Path.GetExtension(f.Name);

  }
}