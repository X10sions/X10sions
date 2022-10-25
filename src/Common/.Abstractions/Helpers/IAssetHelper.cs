using System.Text;

namespace Common.Helpers;

public interface IAssetHelper {

  // http://stackoverflow.com/questions/5110028/add-css-or-js-files-to-layout-head-from-views-or-partial-views
  // https://github.com/speier/mvcassetshelperper

  //string PackageRootVirtualPath { get; }
  IDictionary<string, IAssetFile> Files { get; }
  //IList<IAssetPackage> Packages { get; }

  //xIAssetPackage NewAssetPackage(string directoryPath, string name, int order = 0);
  //xIAssetPackageFile NewAssetPackageFile(string file, xIAssetPackageVersion packageVersion);
  //xIAssetPackageVersion NewAssetPackageVersion(string version, xIAssetPackage assetPackage);

  //IAssetFile NewAssetFile(string rootVirtualPath, string fileName, int priority = 0, string? packageName, string? packageVersion);
}

public class AssetHelper : IAssetHelper {
  //public AssetHelper(string packageRootVirtualPath = "/cdn/packages/") {
  //  PackageRootVirtualPath = packageRootVirtualPath;
  //}

  //public string PackageRootVirtualPath { get; }
  public IDictionary<string, IAssetFile> Files { get; } = new Dictionary<string, IAssetFile>();
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

  //public IAssetFile NewAssetFile(string rootVirtualPath, string? packageName, string? packageVersion, string fileName, int priority = 0)
  //  => new AssetFile(rootVirtualPath, fileName, priority) {
  //    PackageName = packageName,
  //    PackageVersion = packageVersion,
  //  };

}


public static class IAssetHelperExtensions {
  //public static IEnumerable<IAssetFile> Files(this IAssetHelper assetHelper) => from v in assetHelper.PackageVersions() from f in v.Files orderby f.Priority select f;

  //public static IEnumerable<string> FileVirtualPaths(this IAssetHelper assetHelper, Func<IAssetFile, bool> predicate)
  //  => from x in assetHelper.Files.Where(predicate) orderby x.Priority select x.FileVirtualPath();


  public static IEnumerable<string> GetPaths(this IAssetHelper assetHelper, Func<IAssetFile, bool>? predicate = null)
    => from x in assetHelper.Files.Values.Where(predicate ?? (_ => true)) orderby x.Priority, x.Path select x.Path;

  public static IEnumerable<string> GetScriptPaths(this IAssetHelper assetHelper) => assetHelper.GetPaths(x => x.IsScript());
  public static IEnumerable<string> GetStylesheetPaths(this IAssetHelper assetHelper) => assetHelper.GetPaths(x => x.IsStylesheet());

  //public static IAssetHelper AddFiles(this IAssetHelper assetHelper, params IAssetFile[] assetFiles) {
  //  foreach (var assetFile in assetFiles) {
  //    assetHelper.Packages.Add(assetFile.PackageVersion.Package);
  //    assetHelper.Files.Add(assetFile);
  //  }
  //  return assetHelper;
  //}

  public static IAssetHelper AddFile(this IAssetHelper assetHelper, string path, int priority = 0, string? packageName = null, Version? packageVersion = null) {
    if (!string.IsNullOrWhiteSpace(path)) {
      var exists = assetHelper.Files.TryGetValue(path, out var assetFile);
      if (!exists) {
        assetFile.Path = path;
      }
      assetFile.Priority = priority;
      //assetFile.PackageName = packageName;
      //assetFile.PackageVersion = packageVersion;
      assetHelper.Files[path] = assetFile;
    }
    return assetHelper;
  }

  public static IAssetHelper AddFileToBottom(this IAssetHelper assetHelper, string path)
    => assetHelper.AddFile(path, int.MaxValue);

  public static IAssetHelper AddFileToTop(this IAssetHelper assetHelper, string path)
    => assetHelper.AddFile(path, int.MinValue);

  //public static IAssetHelper AddFiles(this IAssetHelper assetHelper, string rootVirtualPath, string packageName, string packageVersion, IEnumerable<string> fileNames, int priority = 0)
  //  => assetHelper.AddFiles(rootVirtualPath, packageName, packageVersion, fileNames, priority);

  //public static IAssetHelper AddFiles<T>(this IAssetHelper assetHelper, string[] filePaths, int priority = 0) where T : IAssetFile, new() {
  //  foreach (var path in filePaths) {
  //    assetHelper.AddFile(path, priority,packageName,packageVersion);
  //  }
  //  return assetHelper;
  //}

  public static IAssetHelper AddFiles(this IAssetHelper assetHelper, string rootPath, string[] fileNames, int priority = 0) {
    foreach (var fileName in fileNames) {
      assetHelper.AddFile(rootPath + fileName, priority);
    }
    return assetHelper;
  }

  //public static IAssetHelper AddPackageFiles(this IAssetHelper assetHelper, string packageName, string? packageVersion, string fileName, int priority = 0)
  //  => assetHelper.AddPackageFiles(packageName, packageVersion, new[] { fileName }, priority);

  //public static IAssetHelper AddPackageFiles(this IAssetHelper assetHelper, string packageName, string? packageVersion, string rootPath, string[] fileNames, int priority = 0)
  //   => assetHelper.AddFiles(rootPath, fileNames, priority, packageName, new Version(packageVersion));

  //public static xIAssetItemRegistrar Add(this xIAssetItemRegistrar assetItemRegistrar, int order, params string[] urls) {
  //  foreach (var path in urls) {
  //    if (!assetItemRegistrar.Items.Any(w => w.Path == path)) {
  //      var item = assetItemRegistrar.NewAssetItem(order, path);
  //      assetItemRegistrar.Items.Add(item);
  //    }
  //  }
  //  return assetItemRegistrar;
  //}


  static string Render(this IAssetHelper assetHelper, Func<IAssetFile, bool>? predicate = null) {
    var sb = new StringBuilder();
    foreach (var htmlTag in from x in assetHelper.Files.Values.Where(predicate ?? (x => true)) orderby x.Priority, x.Path select x.HtmlTagString()) {
      sb.AppendLine(htmlTag);
    }
    return sb.ToString();
  }

  public static string GetStylesheetHtmlTagStrings(this IAssetHelper assetHelper) => assetHelper.Render(x => x.IsStylesheet());
  public static string GetScriptHtmlTagStrings(this IAssetHelper assetHelper) => assetHelper.Render(x => x.IsScript());

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
        assetHelper.AddFile(assetFile.Path, assetFile.Priority);
      }
    }
    return assetHelper;
  }

  #endregion
}

public static class AssetHelperExtensions {


}