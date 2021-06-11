using System.Collections.Generic;
using System.IO;

namespace Common.Helpers {
  public interface IAssetPackageVersion {
    IAssetPackage Package { get; set; }
    string Version { get; set; }
    IList<IAssetFile> Files { get; }
  }

  public static class IAssetPackageVersionExtensions {

    public static string VirtualPath(this IAssetPackageVersion assetPackageVersion) => assetPackageVersion.Package.VirtualPath() + (string.IsNullOrWhiteSpace(assetPackageVersion.Version) ? string.Empty : assetPackageVersion.Version + Path.AltDirectorySeparatorChar);

    public static IAssetPackageVersion AddFiles(this IAssetPackageVersion assetPackageVersion, params IAssetFile[] packageFiles) {
      foreach (var file in packageFiles) {
        assetPackageVersion.Files.Add(file);
      }
      return assetPackageVersion;
    }

  }
}