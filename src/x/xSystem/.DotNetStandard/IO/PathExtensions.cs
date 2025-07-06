namespace System.IO {
  public static class PathExtensions {

    public static readonly string AltDirectorySeparatorString = Path.AltDirectorySeparatorChar.ToString();
    public static string ReplaceInvalidPathChars(this string filename, string replaceWith = "_") => string.Join(replaceWith, filename.Split(Path.GetInvalidPathChars()));
    public static string ReplaceInvalidFileNameChars(this string filename, string replaceWith = "_") => string.Join(replaceWith, filename.Split(Path.GetInvalidFileNameChars()));

  }
}