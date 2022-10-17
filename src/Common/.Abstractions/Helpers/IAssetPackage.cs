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

  public class AssetPackage : IAssetPackage {
    public AssetPackage(string rootVirtualPath, string name
      //, IEnumerable<IAssetPackageVersion> versions
      ) {
      Name = name;
      RootVirtualPath = rootVirtualPath;
      //  foreach (var version in versions) {
      //    Versions.Add(version);
      //  }
    }
    public string RootVirtualPath { get; set; }
    public string Name { get; set; }
    public IList<IAssetPackageVersion> Versions => new List<IAssetPackageVersion>();

  }


  public static class IAssetsPackageExtensions {
    public static string VirtualPath(this IAssetPackage assetPackage) => assetPackage.RootVirtualPath + (string.IsNullOrWhiteSpace(assetPackage.Name) ? string.Empty : assetPackage.Name + Path.AltDirectorySeparatorChar);
    //public static string AssToAssetsHelper(this IAssetsPackage assetsPackage) => $"{assetsPackage.RootPath}/{assetsPackage.Name}/{assetsPackage.Version}/";

    public static IAssetPackage AddVersions(this IAssetPackage assetPackage, params IAssetPackageVersion[] packageVersions) {
      foreach (IAssetPackageVersion version in packageVersions) {
        assetPackage.Versions.Add(version);
      }
      return assetPackage;
    }

  }

}