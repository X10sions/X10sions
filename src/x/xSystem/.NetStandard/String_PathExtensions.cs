namespace System {
  public static class StringPathExtensions {
    public static StringPathHelper PathHelper(this string value) => new StringPathHelper(value);
    public static StringPathsHelper PathsHelper(this string[] values) => new StringPathsHelper(values);
  }

  public class StringPathHelper(string path) {
    public string Extension => Path.GetExtension(path);
    public bool IsPhysicalPath => !string.IsNullOrWhiteSpace(path) && path.Contains(":");

    public string FileName(bool includeExtension = true) => includeExtension ? Path.GetFileName(path) : Path.GetFileNameWithoutExtension(path);

    public bool HasFileExtension(string fileExtension) => string.Equals(Extension, fileExtension.TrimStart('.') + ".", StringComparison.OrdinalIgnoreCase);
    public bool HasFileExtensions(params string[] fileExtension) => fileExtension.Exists(HasFileExtension);

    public static readonly char VirtualPathSeparatorChar = Path.AltDirectorySeparatorChar;
    public static readonly string VirtualPathSeparatorString = VirtualPathSeparatorChar.ToString();

    public string AddVirtualPathSeparator => (string.IsNullOrWhiteSpace(path) ? VirtualPathSeparatorString : string.Empty) + path;

    #region "FileHelpers"
    public string NormalizePath => string.IsNullOrEmpty(path) ? path : path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar).Replace(Path.DirectorySeparatorChar.ToString() + Path.DirectorySeparatorChar.ToString(), Path.DirectorySeparatorChar.ToString());
    public string TrimTrailingPathSeparatorChar => NormalizePath.TrimEnd(Path.DirectorySeparatorChar);
    public string DemandTrailingPathSeparatorChar => TrimTrailingPathSeparatorChar + Path.DirectorySeparatorChar;
    public string AsPathSegment => $"{Path.DirectorySeparatorChar}{path}{Path.DirectorySeparatorChar}";
    #endregion

    public string VirtualDirectory => "~" + Path.GetDirectoryName(path).Replace('\\', Path.AltDirectorySeparatorChar);
    public string VirtualFile(bool includeExtension = true) => VirtualDirectory.TrimEnd(Path.AltDirectorySeparatorChar) + Path.AltDirectorySeparatorChar + FileName(includeExtension);

  }

  public class StringPathsHelper(string[] paths) {
    public string VirtualPathCombine => Path.AltDirectorySeparatorChar + string.Join(Path.AltDirectorySeparatorChar.ToString(), paths.Where(x => !string.IsNullOrWhiteSpace(x)));

    public IEnumerable<string> WithFileExtension(string fileExtension) => from path in paths where path.PathHelper().HasFileExtension(fileExtension) select path;
    public IEnumerable<string> WithFileExtensions(string fileExtension) => from path in paths where path.PathHelper().HasFileExtensions(fileExtension) select path;

  }
}