namespace Common.Helpers {
  public class AssetFile : IAssetFile {
    //public AssetFile(string fileName, IAssetPackageVersion packageVersion, int priority = 0) {
    //  FileName = fileName;
    //  PackageVersion = packageVersion;
    //  Priority = priority;
    //}

    //public AssetFile(string fileName, string rootVirtualPath = null, string packageName=null, string packageVersion=null, int priority = 0) {
    //  RootVirtualPath = rootVirtualPath.TrimEnd(Path.AltDirectorySeparatorChar) + Path.AltDirectorySeparatorChar;
    //  PackageName = packageName;
    //  PackageVersion = packageVersion;
    //  FileName = fileName;
    //  Priority = priority;

    //AssetPackage = new AssetPackage(rootVirtualPath, packageName, )

    //}

    //public AssetFile(string rootVirtualPath, string packageName, string fileName, int priority = 0)
    //  : this(rootVirtualPath, packageName, null, fileName, priority) { }

    //public AssetFile(string rootVirtualPath, string fileName, int priority = 0)
    //  : this(rootVirtualPath, null, null, fileName, priority) { }

    public string RootVirtualPath { get; set; }
    public string PackageName { get; set; }
    public string PackageVersion { get; set; }
    //public IAssetPackageVersion PackageVersion { get; set; }

    public string FileName { get; set; }
    public int Priority { get; set; }


    //public IAssetPackage AssetPackage { get; set; }
    //public IAssetPackage AssetPackage => new AssetPackage(rootVirtualPath, PackageName, new asset)
    //      public IAssetPackageVersion AssetPackageVersion => new AssetPackageVersion(PackageVersion, new[] { new AssetFile(  FileName });

  }
}