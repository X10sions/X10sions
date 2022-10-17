using Common.Constants;

namespace Common.Helpers {
  public interface IAssetFile {
    //string RootVirtualPath { get; set; } //   ~/cdn/packages/
    //string? PackageName { get; set; }     //    jquery
    //Version? PackageVersion { get; set; }  //    3.3.1
    string Path { get; set; }        //    jquery.min.js
    int Priority { get; set; }              //    0
    //string? PackageName { get; set; }
  }

  public class AssetFile : IAssetFile {
    public AssetFile(string path, int priority = 0) {
      Path = path;
      Priority = priority;
    }
    //public AssetFile(string rootVirtualPath, string fileName, int priority = 0) {
    //  RootVirtualPath = rootVirtualPath;
    //  FileName = fileName;
    //  Priority = priority;
    //}

    //public AssetFile(string rootVirtualPath, string? packageName, string? packageVersion, string fileName, int priority = 0)
    //  : this(rootVirtualPath, fileName, priority) {
    //  PackageName = packageName;
    //  PackageVersion = packageVersion;
    //}

    public string Path { get; set; }
    //public string RootVirtualPath { get; set; }
    //public string? PackageName { get; set; }
    //public Version? PackageVersion { get; set; }
    //public string FileName { get; set; }
    public int Priority { get; set; }
  }


  public static class IAssetFileExtensions {

    //public static string RootVirtualPath(this IAssetFile assetFile) => assetFile.Package().RootVirtualPath;
    //public static string PackageName(this IAssetFile assetFile) => assetFile.Package().Name;
    //public static IAssetPackage Package(this IAssetFile assetFile) => assetFile.PackageVersion.Package;

    //public static string PackageVirtualPath(this IAssetFile assetFile)
    //  => assetFile.RootVirtualPath 
    //  + (string.IsNullOrWhiteSpace(assetFile.PackageName) ? string.Empty : assetFile.PackageName + Path.AltDirectorySeparatorChar);

    //public static string PackageVersionVirtualPath(this IAssetFile assetFile)
    //  => assetFile.PackageVirtualPath()
    //  + (string.IsNullOrWhiteSpace(assetFile.PackageVersion) ? string.Empty : assetFile.PackageVersion + Path.AltDirectorySeparatorChar);

    //public static string DirectoryVirtualPath(this IAssetFile assetFile)
    //  => assetFile.PackageVersionVirtualPath();

    //public static string FileVirtualPath(this IAssetFile assetFile) => assetFile.DirectoryVirtualPath() + assetFile.FileName;

    public static string Extension(this IAssetFile assetFile) => Path.GetExtension(assetFile.Path).ToLower();
    public static bool IsScript(this IAssetFile assetFile) => assetFile.Extension() == ".js";
    public static bool IsStylesheet(this IAssetFile assetFile) => assetFile.Extension() == ".css";

    public static string HtmlTagString(this IAssetFile assetFile) => assetFile.Extension() switch {
      ".css" => HtmlConstants.StylesheetHtmlTagString(assetFile.Path),
      ".js" => HtmlConstants.ScriptHtmlTagString(assetFile.Path),
      _ => throw new NotImplementedException()
    };

  }
}