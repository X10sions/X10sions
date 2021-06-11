using System.Collections.Generic;
using System.IO;

namespace Common.Helpers {

  public interface IAssetPackage {
    string RootVirtualPath { get; set; }
    string Name { get; set; }
    //IAssetHelper AssetHelper { get; set; }
    //string DirectoryPath { get; }
    IList<IAssetPackageVersion> Versions { get; }

    //IEnumerable<string> CssFiles { get; }
    //IEnumerable<string> JsFiles { get; }
  }

  public static class IAssetsPackageExtensions {
    public static string VirtualPath(this IAssetPackage assetPackage) => assetPackage.RootVirtualPath + (string.IsNullOrWhiteSpace(assetPackage.Name) ? string.Empty : assetPackage.Name + Path.AltDirectorySeparatorChar);
    //public static string AssToAssetsHelper(this IAssetsPackage assetsPackage) => $"{assetsPackage.RootPath}/{assetsPackage.Name}/{assetsPackage.Version}/";

    public static IAssetPackage AddVersions(this IAssetPackage assetPackage, params IAssetPackageVersion[] packageVersions) {
      foreach (var version in packageVersions) {
        assetPackage.Versions.Add(version);
      }
      return assetPackage;
    }

  }

}