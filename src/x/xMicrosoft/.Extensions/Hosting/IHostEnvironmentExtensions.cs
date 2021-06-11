using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.Extensions.Hosting {
  public static class IHostEnvironmentExtensions {
    public const string AppSettingsFileName = "AppSettings";
    public const string AppSettingsFileExtension = "json";

    public static string[] AppSettingsPaths(this IHostEnvironment hostEnvironment) => new[] { hostEnvironment.AppSettingsCurrentPath(), hostEnvironment.AppSettingsEnvironmentPath() };
    public static string AppSettingsCurrentPath(this IHostEnvironment hostEnvironment) => hostEnvironment.ContentRootFilePath(AppSettingsFileName, AppSettingsFileExtension);

    public static string AppSettingsEnvironmentPath(this IHostEnvironment hostEnvironment) => hostEnvironment.ContentRootFileEnvironmentPath(AppSettingsFileName, AppSettingsFileExtension);

    public static string ContentRootFilePath(this IHostEnvironment hostEnvironment, string fileName, string fileExtension) => hostEnvironment.MapContentRootPath($"{fileName}.{fileExtension.TrimStart('.')}");

    public static string ContentRootFileEnvironmentPath(this IHostEnvironment hostEnvironment, string fileName, string fileExtension) => hostEnvironment.ContentRootFilePath($"{fileName}.{hostEnvironment.EnvironmentName}", fileExtension);

    public static string FileName(this IHostEnvironment env, string fileName) {
      var fi = new FileInfo(fileName);
      return fileName.Replace(fi.Extension, $"{env.EnvironmentName}.{fi.Extension}");
    }

    public static T GetByEnvironment<T>(this IHostEnvironment hostEnvironment, Func<T> production, Func<T> dev) => hostEnvironment.IsProduction() ? production() : dev();
    public static T GetByEnvironment<T>(this IHostEnvironment hostEnvironment, Func<T> production, Func<T> staging, Func<T> dev) => hostEnvironment.IsStaging() ? staging() : hostEnvironment.GetByEnvironment(production, dev);

    public static string MapContentRootPath(this IHostEnvironment hostEnvironment, params string[] paths) => Path.Combine(new[] { hostEnvironment.ContentRootPath }.Concat(paths).ToArray());
    public static FileInfo MapContentRootPathFileInfo(this IHostEnvironment hostEnvironment, params string[] paths) => new FileInfo(hostEnvironment.MapContentRootPath(paths));
    public static DirectoryInfo MapContentRootPathDirectoryInfo(this IHostEnvironment hostEnvironment, string path) => hostEnvironment.MapContentRootPathFileInfo(path).Directory;
    public static IEnumerable<FileInfo> MapContentRootPathFileInfos(this IHostEnvironment hostEnvironment, string path) => hostEnvironment.MapContentRootPathDirectoryInfo(path).EnumerateFiles();
    public static IEnumerable<FileInfo> MapContentRootPathFileInfos(this IHostEnvironment hostEnvironment, string path, string searchPattern, SearchOption searchOption = SearchOption.TopDirectoryOnly) => hostEnvironment.MapContentRootPathDirectoryInfo(path).EnumerateFiles(searchPattern, searchOption);

  }
}