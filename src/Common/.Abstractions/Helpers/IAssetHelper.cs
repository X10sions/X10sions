namespace Common.Helpers;

public interface IAssetHelper {

  // http://stackoverflow.com/questions/5110028/add-css-or-js-files-to-layout-head-from-views-or-partial-views
  // https://github.com/speier/mvcassetshelperper

  string PackageRootVirtualPath { get; }
  IList<IAssetFile> Files { get; }
  //IList<IAssetPackage> Packages { get; }

  //xIAssetPackage NewAssetPackage(string directoryPath, string name, int order = 0);
  //xIAssetPackageFile NewAssetPackageFile(string file, xIAssetPackageVersion packageVersion);
  //xIAssetPackageVersion NewAssetPackageVersion(string version, xIAssetPackage assetPackage);

  IAssetFile NewAssetFile(string rootVirtualPath, string? packageName, string? packageVersion, string fileName, int priority = 0);
}

public static class IAssetHelperExtensions {
  //public static IEnumerable<IAssetFile> Files(this IAssetHelper assetHelper) => from v in assetHelper.PackageVersions() from f in v.Files orderby f.Priority select f;

  public static IEnumerable<string> FileVirtualPaths(this IAssetHelper assetHelper, Func<IAssetFile, bool> predicate)
    => from x in assetHelper.Files.Where(predicate) orderby x.Priority select x.FileVirtualPath();

  public static IEnumerable<string> FileVirtualPaths(this IAssetHelper assetHelper) => assetHelper.FileVirtualPaths(_ => true);
  public static IEnumerable<string> ScriptFileVirtualPaths(this IAssetHelper assetHelper) => assetHelper.FileVirtualPaths(x => x.IsScript());
  public static IEnumerable<string> StylesheetVirtualPaths(this IAssetHelper assetHelper) => assetHelper.FileVirtualPaths(x => x.IsStylesheet());

  //public static IAssetHelper AddFiles(this IAssetHelper assetHelper, params IAssetFile[] assetFiles) {
  //  foreach (var assetFile in assetFiles) {
  //    assetHelper.Packages.Add(assetFile.PackageVersion.Package);
  //    assetHelper.Files.Add(assetFile);
  //  }
  //  return assetHelper;
  //}
  public static IAssetHelper AddFiles(this IAssetHelper assetHelper, string rootVirtualPath, string packageName, string packageVersion, string fileName, int priority = 0)
    => assetHelper.AddFiles(rootVirtualPath, packageName, packageVersion, new[] { fileName }, priority);

  public static IAssetHelper AddFiles(this IAssetHelper assetHelper, string rootVirtualPath, string? packageName, string? packageVersion, IEnumerable<string> fileNames, int priority = 0) {
    foreach (var fileName in fileNames) {
      var assetFile = assetHelper.NewAssetFile(rootVirtualPath, packageName, packageVersion, fileName, priority);
      assetHelper.Files.Add(assetFile);
    }
    return assetHelper;
  }
  public static IAssetHelper AddDirectoryFiles(this IAssetHelper assetHelper, string rootVirtualPath, string fileName, int priority = 0)
    => assetHelper.AddDirectoryFiles(rootVirtualPath, new[] { fileName }, priority);

  public static IAssetHelper AddDirectoryFiles(this IAssetHelper assetHelper, string rootVirtualPath, IEnumerable<string> fileNames, int priority = 0)
     => assetHelper.AddFiles(rootVirtualPath, null, null, fileNames, priority);

  public static IAssetHelper AddPackageFiles(this IAssetHelper assetHelper, string packageName, string packageVersion, string fileName, int priority = 0)
    => assetHelper.AddPackageFiles(packageName, packageVersion, new[] { fileName }, priority);

  public static IAssetHelper AddPackageFiles(this IAssetHelper assetHelper, string packageName, string packageVersion, IEnumerable<string> fileNames, int priority = 0)
     => assetHelper.AddFiles(assetHelper.PackageRootVirtualPath, packageName, packageVersion, fileNames, priority);

  //public static xIAssetItemRegistrar Add(this xIAssetItemRegistrar assetItemRegistrar, int order, params string[] urls) {
  //  foreach (var path in urls) {
  //    if (!assetItemRegistrar.Items.Any(w => w.Path == path)) {
  //      var item = assetItemRegistrar.NewAssetItem(order, path);
  //      assetItemRegistrar.Items.Add(item);
  //    }
  //  }
  //  return assetItemRegistrar;
  //}

  #region "IAssetPackage"

  //public static IAssetHelper AddPackagePath(this IAssetHelper assetHelper, string path, string name, string file, int order = 0, string version = null) => assetPackageHelper.AddPackagePath(path, name, new[] { file }, order, version);
  //public static IAssetHelper AddPackagePath(this IAssetHelper assetHelper, string path, string name, IEnumerable<string> files, int order = 0, string version = null) {
  //  var p = assetPackageHelper.NewAssetPackage(path, name, order);
  //  var pv = assetPackageHelper.NewAssetPackageVersion(version, p);
  //  foreach (var file in files) {
  //    pv.Files.Add(assetPackageHelper.NewAssetPackageFile(file, pv));
  //  }
  //  p.AddVersions(pv);
  //  return assetPackageHelper.AddPackage(p);
  //}

  public static IAssetHelper AddPackages(this IAssetHelper assetHelper, params IAssetPackage[] assetPackages) {
    foreach (var assetPackage in assetPackages) {
      assetHelper.AddPackageVersions(assetPackage.Versions.ToArray());
    }
    return assetHelper;
  }
  #endregion

  #region IAssetPackageVersion
  //public static IAssetHelper AddPackageVersion(this IAssetHelper assetHelper, string name, string version, IEnumerable<string> files, int order = 0) => assetPackageHelper.AddPackagePath(assetPackageHelper.DefaultPackageDirectoryPath, name, files, order, version);
  //public static IAssetHelper AddPackageVersion(this IAssetHelper assetHelper, string name, string version, string file, int order = 0) => assetPackageHelper.AddPackageVersion(name, version, new[] { file }, order);
  //public static IEnumerable<IAssetPackageVersion> PackageVersions(this IAssetHelper assetHelper) => from p in assetHelper.Packages from v in p.Versions select v;

  public static IAssetHelper AddPackageVersions(this IAssetHelper assetHelper, params IAssetPackageVersion[] assetPackageVersions) {
    foreach (var assetPackageVersion in assetPackageVersions) {
      foreach (var assetFile in assetPackageVersion.Files) {
        assetHelper.Files.Add(assetFile);
      }
    }
    return assetHelper;
  }

  #endregion
}