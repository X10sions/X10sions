using System.IO;

namespace Common.Helpers {
  public interface IAssetFile {
    string RootVirtualPath { get; set; } //   ~/cdn/packages/
    string PackageName { get; set; }     //    jquery
    string PackageVersion { get; set; }  //    3.3.1

    //IAssetPackageVersion PackageVersion { get; set; }

    string FileName { get; set; }            //    jquery.min.js
    int Priority { get; set; }           //    -1

    //public string PathNameAlias { get; set; }
    //public IDictionary<string, string> HtmlAttributes { get; private set; }; }

    //string FilePath { get; set; }

  }

  public static class IAssetFileExtensions {

    //public static string RootVirtualPath(this IAssetFile assetFile) => assetFile.Package().RootVirtualPath;
    //public static string PackageName(this IAssetFile assetFile) => assetFile.Package().Name;
    //public static IAssetPackage Package(this IAssetFile assetFile) => assetFile.PackageVersion.Package;

    public static string PackageVirtualPath(this IAssetFile assetFile)
      => assetFile.RootVirtualPath
      + (string.IsNullOrWhiteSpace(assetFile.PackageName) ? string.Empty : assetFile.PackageName + Path.AltDirectorySeparatorChar);

    public static string PackageVersionVirtualPath(this IAssetFile assetFile)
      => assetFile.PackageVirtualPath()
      + (string.IsNullOrWhiteSpace(assetFile.PackageVersion) ? string.Empty : assetFile.PackageVersion + Path.AltDirectorySeparatorChar);

    public static string DirectoryVirtualPath(this IAssetFile assetFile)
      => assetFile.PackageVersionVirtualPath();

    public static string FileVirtualPath(this IAssetFile assetFile) => assetFile.DirectoryVirtualPath() + assetFile.FileName;

    public static string Extension(this IAssetFile assetFile) => Path.GetExtension(assetFile.FileName);
    public static bool IsScript(this IAssetFile assetFile) => string.Equals(assetFile.Extension(), ".js", System.StringComparison.OrdinalIgnoreCase);
    public static bool IsStylesheet(this IAssetFile assetFile) => string.Equals(assetFile.Extension(), ".css", System.StringComparison.OrdinalIgnoreCase);

  }
}