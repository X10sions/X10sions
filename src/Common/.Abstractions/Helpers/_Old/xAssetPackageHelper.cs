//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;

//namespace Common.Helpers {
//  #region  "Helper"

//  public interface xIAssetPackageHelper {
//    string DefaultPackageDirectoryPath { get; }
//    IList<xIAssetPackage> Packages { get; }

//    xIAssetPackage NewAssetPackage(string directoryPath, string name, int order = 0);
//    xIAssetPackageFile NewAssetPackageFile(string file, xIAssetPackageVersion packageVersion);
//    xIAssetPackageVersion NewAssetPackageVersion(string version, xIAssetPackage assetPackage);

//  }

//  public static class xIAssetPackageHelperExtensions {

//    public static xIAssetPackageHelper AddPackageVersion(this xIAssetPackageHelper assetPackageHelper, string name, string version, IEnumerable<string> files, int order = 0) => assetPackageHelper.AddPackagePath(assetPackageHelper.DefaultPackageDirectoryPath, name, files, order, version);
//    public static xIAssetPackageHelper AddPackageVersion(this xIAssetPackageHelper assetPackageHelper, string name, string version, string file, int order = 0) => assetPackageHelper.AddPackageVersion(name, version, new[] { file }, order);
//    public static xIAssetPackageHelper AddPackage(this xIAssetPackageHelper assetPackageHelper, xIAssetPackage package) => assetPackageHelper.AddPackages(new[] { package });
//    public static xIAssetPackageHelper AddPackages(this xIAssetPackageHelper assetPackageHelper, IEnumerable<xIAssetPackage> packages) {
//      foreach (var package in packages) {
//        assetPackageHelper.Packages.Add(package);
//      }
//      return assetPackageHelper;
//    }

//    public static xIAssetPackageHelper AddPackagePath(this xIAssetPackageHelper assetPackageHelper, string path, string name, string file, int order = 0, string version = null) => assetPackageHelper.AddPackagePath(path, name, new[] { file }, order, version);

//    public static xIAssetPackageHelper AddPackagePath(this xIAssetPackageHelper assetPackageHelper, string path, string name, IEnumerable<string> files, int order = 0, string version = null) {
//      var p = assetPackageHelper.NewAssetPackage(path, name, order);
//      var pv = assetPackageHelper.NewAssetPackageVersion(version, p);
//      foreach (var file in files) {
//        pv.Files.Add(assetPackageHelper.NewAssetPackageFile(file, pv));
//      }
//      p.AddVersions(pv);
//      return assetPackageHelper.AddPackage(p);
//    }

//    public static IEnumerable<xIAssetPackageVersion> PackageVersions(this xIAssetPackageHelper assetPackageHelper) => from p in assetPackageHelper.Packages orderby p.Order from v in p.Versions select v;
//    public static IEnumerable<xIAssetPackageFile> PackageFiles(this xIAssetPackageHelper assetPackageHelper) => from v in assetPackageHelper.PackageVersions() from f in v.Files select f;

//    public static IEnumerable<string> FileVirtualPaths(this xIAssetPackageHelper assetPackageHelper, Func<xIAssetPackageFile, bool> predicate) => from x in assetPackageHelper.PackageFiles().Where(predicate) select x.VirtualPath();
//    public static IEnumerable<string> FileVirtualPaths(this xIAssetPackageHelper assetPackageHelper) => assetPackageHelper.FileVirtualPaths(_ => true);
//    public static IEnumerable<string> ScriptFileVirtualPaths(this xIAssetPackageHelper assetPackageHelper) => assetPackageHelper.FileVirtualPaths(x => x.IsScript());
//    public static IEnumerable<string> StylesheetVirtualPaths(this xIAssetPackageHelper assetPackageHelper) => assetPackageHelper.FileVirtualPaths(x => x.IsStylesheet());

//  }

//  public class xAssetPackageHelper : xIAssetPackageHelper {
//    public xAssetPackageHelper(string defaultPackageDirectoryPath = "~/cdn/packages/") {
//      DefaultPackageDirectoryPath = defaultPackageDirectoryPath.TrimEnd('/') + '/';
//    }
//    public string DefaultPackageDirectoryPath { get; }
//    public IList<xIAssetPackage> Packages { get; } = new List<xIAssetPackage>();

//    public xIAssetPackage NewAssetPackage(string directoryPath, string name, int order = 0) => new xAssetPackage(directoryPath, name, this, order);
//    public xIAssetPackageFile NewAssetPackageFile(string file, xIAssetPackageVersion packageVersion) => new xAssetPackageFile(file, packageVersion);
//    public xIAssetPackageVersion NewAssetPackageVersion(string version, xIAssetPackage assetPackage) => new xAssetPackageVersion(version, assetPackage);
//  }

//  #endregion

//  #region "Package"

//  public interface xIAssetPackage {
//    int Order { get; set; }
//    xIAssetPackageHelper PackageHelper { get; set; }
//    string DirectoryPath { get; }
//    string Name { get; set; }
//    IList<xIAssetPackageVersion> Versions { get; }


//  }

//  public static class xIAssetPackageExtensions {

//    public static xIAssetPackage AddVersions(this xIAssetPackage assetPackage, params xIAssetPackageVersion[] packageVersions) {
//      foreach (var version in packageVersions) {
//        assetPackage.Versions.Add(version);
//      }
//      return assetPackage;
//    }

//    public static string Path(this xIAssetPackage assetPackage) => $"{assetPackage.DirectoryPath}{assetPackage.Name}/";

//  }

//  public class xAssetPackage : xIAssetPackage {
//    public xAssetPackage(string directoryPath, string name, xIAssetPackageHelper packageHelper, int order = 0) {
//      Name = name;
//      DirectoryPath = directoryPath.TrimEnd('/') + '/';
//      PackageHelper = packageHelper;
//      Order = order;
//    }
//    public int Order { get; set; }
//    public xIAssetPackageHelper PackageHelper { get; set; }
//    public string DirectoryPath { get; }
//    public string Name { get; set; }
//    public IList<xIAssetPackageVersion> Versions { get; } = new List<xIAssetPackageVersion>();
//  }

//  #endregion

//  #region "PacakgeVersion"

//  public interface xIAssetPackageVersion {
//    string Version { get; set; }
//    xIAssetPackage Package { get; set; }
//    IList<xIAssetPackageFile> Files { get; }
//  }

//  public static class xIAssetPackageVersionExtensions {

//    public static xIAssetPackageVersion AddFiles(this xIAssetPackageVersion assetPackageVersion, params xIAssetPackageFile[] packageFiles) {
//      foreach (var file in packageFiles) {
//        assetPackageVersion.Files.Add(file);
//      }
//      return assetPackageVersion;
//    }
//    public static string Path(this xIAssetPackageVersion assetPackageVersion) => assetPackageVersion.Package.Path() + (string.IsNullOrWhiteSpace(assetPackageVersion.Version) ? string.Empty : $"{assetPackageVersion.Version}/");

//  }

//  public class xAssetPackageVersion : xIAssetPackageVersion {
//    public xAssetPackageVersion(string version, xIAssetPackage package) {
//      Version = version;
//      Package = package;
//    }
//    public string Version { get; set; }
//    public xIAssetPackage Package { get; set; }
//    public IList<xIAssetPackageFile> Files { get; } = new List<xIAssetPackageFile>();
//  }



//  #endregion


//  #region "PacakgeFile"

//  public interface xIAssetPackageFile {
//    xIAssetPackageVersion PackageVersion { get; set; }
//    string FilePath { get; set; }
//  }

//  public static class xIAssetPackageFileExtensions {
//    public static string FileExtension(this xIAssetPackageFile assetPackageFile) => Path.GetExtension(assetPackageFile.FilePath).ToLower();
//    public static bool IsScript(this xIAssetPackageFile assetPackageFile) => assetPackageFile.FileExtension() == ".js";
//    public static bool IsStylesheet(this xIAssetPackageFile assetPackageFile) => assetPackageFile.FileExtension() == ".css";
//    public static string VirtualPath(this xIAssetPackageFile assetPackageFile) => assetPackageFile.PackageVersion.Path() + assetPackageFile.FilePath;

//  }

//  public class xAssetPackageFile : xIAssetPackageFile {
//    public xAssetPackageFile(string filePath, xIAssetPackageVersion packageVersion) {
//      FilePath = filePath;
//      PackageVersion = packageVersion;
//    }
//    public xIAssetPackageVersion PackageVersion { get; set; }
//    public string FilePath { get; set; }
//  }

//  #endregion


//  #region
//  #endregion










//}