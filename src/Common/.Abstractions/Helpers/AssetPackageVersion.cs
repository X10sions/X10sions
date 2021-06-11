using System.Collections.Generic;

namespace Common.Helpers {
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
}