using System.Collections.Generic;

namespace Common.Helpers {
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
}