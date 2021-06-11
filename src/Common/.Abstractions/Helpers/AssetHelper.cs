using System.Collections.Generic;

namespace Common.Helpers {
  public class AssetHelper : IAssetHelper {
    public AssetHelper(string packageRootVirtualPath = "/cdn/packages/") {
      PackageRootVirtualPath = packageRootVirtualPath;
    }

    public string PackageRootVirtualPath { get; }
    public IList<IAssetFile> Files { get; } = new List<IAssetFile>();
    //public IList<IAssetPackage> Packages { get; } = new List<IAssetPackage>();

    //public IAssetPackage NewAssetPackage(string directoryPath, string name, int order = 0) => new AssetPackage(directoryPath, name, this, order);
    //public IAssetPackageFile NewAssetPackageFile(string file, IAssetPackageVersion packageVersion) => new AssetPackageFile(file, packageVersion);
    //public IAssetPackageVersion NewAssetPackageVersion(string version, IAssetPackage assetPackage) => new AssetPackageVersion(version, assetPackage);

    //public xIAssetItemRegistrar InlineScripts { get; set; } = new xAssetItemRegistrar(xAssetItemRegistrarFormat.InlineScripts);
    //public xIAssetItemRegistrar InlineStyles { get; set; } = new xAssetItemRegistrar(xAssetItemRegistrarFormat.InlineStyles);
    //public xIAssetItemRegistrar Scripts { get; set; } = new xAssetItemRegistrar(xAssetItemRegistrarFormat.Script);
    //public xIAssetItemRegistrar Styles { get; set; } = new xAssetItemRegistrar(xAssetItemRegistrarFormat.Style);

    //public IAssetPackage NewAssetPackage(string directoryPath, string name, int order = 0) => new AssetPackage(directoryPath, name, this, order);
    //public IAssetFile NewAssetFile(string file, IAssetPackageVersion packageVersion) => new AssetFile(file, packageVersion);
    //public IAssetPackageVersion NewAssetPackageVersion(string version, IAssetPackage assetPackage) => new AssetPackageVersion(version, assetPackage);

    public IAssetFile NewAssetFile(string rootVirtualPath, string packageName, string packageVersion, string fileName, int priority = 0)
      => new AssetFile {
        //PackageVersion = new AssetPackageVersion()

        RootVirtualPath = rootVirtualPath,
        PackageName = packageName,
        PackageVersion = packageVersion,
        FileName = fileName,
        Priority = priority
      };


  }
}