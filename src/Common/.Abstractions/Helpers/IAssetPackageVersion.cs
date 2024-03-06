namespace Common.Helpers {
  public interface IAssetPackageVersion {
    IAssetPackage Package { get; set; }
    string Version { get; set; }
    IList<IAssetFile> Files { get; }
  }

  public class AssetPackageVersion : IAssetPackageVersion {
    //public AssetPackageVersion(string version, IEnumerable<IAssetFile> files) {
    //  Version = version;
    //  foreach (var file in files) {
    //    Files.Add(file);
    //  }
    //}
    public IAssetPackage Package { get; set; }
    public string Version { get; set; }
    public IList<IAssetFile> Files => new List<IAssetFile>();
  }

  public static class IAssetPackageVersionExtensions {

    public static string VirtualPath(this IAssetPackageVersion assetPackageVersion) => assetPackageVersion.Package.VirtualPath() + (string.IsNullOrWhiteSpace(assetPackageVersion.Version) ? string.Empty : assetPackageVersion.Version + Path.AltDirectorySeparatorChar);

    //public static IAssetPackageVersion AddFiles(this IAssetPackageVersion assetPackageVersion, params IAssetFile[] packageFiles) {
    //  foreach (var file in packageFiles) {
    //    assetPackageVersion.Files.Add(file);
    //  }
    //  return assetPackageVersion;
    //}

  }
}