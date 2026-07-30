using System.Diagnostics;

namespace System.Reflection;

public static class AssemblyExtensions {

  public static string CultureName(this Assembly assembly) => assembly.GetName().CultureName;

  public static IEnumerable<TypeInfo> GetConstructableTypes(this Assembly assembly) => GetLoadableDefinedTypes(assembly).Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition);
  public static DateTime GetCreationTime(this Assembly a) => File.GetCreationTime(a.Location);
  public static async Task<string> GetEmbeddedResourceStringAsync(this Assembly assembly, string resourceName) => await assembly.GetEmbeddedResourceStringAsync(resourceName, Encoding.UTF8);

  public static async Task<string> GetEmbeddedResourceStringAsync(this Assembly assembly, string resourceName, Encoding encoding) {
    using (var resourceStream = assembly.GetManifestResourceStream(resourceName)) {
      using (var reader = new StreamReader(resourceStream, encoding)) {
        return await reader.ReadToEndAsync();
      }
    }
  }
  public static FileInfo GetFileInfo(this Assembly a) => new FileInfo(a.Location);
  public static string GetFileVersion(this Assembly a) => a.GetFileVersionInfo().FileVersion;
  public static FileVersionInfo GetFileVersionInfo(this Assembly a) => FileVersionInfo.GetVersionInfo(a.Location);
  public static DateTime GetLastAccessTime(this Assembly a) => File.GetLastAccessTime(a.Location);
  public static DateTime GetLastWriteTime(this Assembly a) => File.GetLastWriteTime(a.Location);
  public static string GetProductVersion(this Assembly a) => a.GetFileVersionInfo().ProductVersion;

  public static IEnumerable<TypeInfo> GetLoadableDefinedTypes(this Assembly assembly) {
    try {
      return assembly.DefinedTypes;
    } catch (ReflectionTypeLoadException ex) {
      return (from t in ex.Types where t != null select t).Select(IntrospectionExtensions.GetTypeInfo);
    }
  }

  public static string GetSimpleName(this Assembly a) => a.GetName().Name;

  public static bool HasEmbeddedResource(this Assembly assembly, string resourceName) => assembly.GetManifestResourceNames().Exists(s => string.Compare(s, resourceName, true) == 0);

  public static Task<byte[]> GetEmbeddedResourceBytesAsync(this Assembly assembly, string resourceName) {
    var embeddedResource = Assembly.GetExecutingAssembly().GetManifestResourceNames().Find(s => string.Compare(s, resourceName, true) == 0);
    using (var stream = assembly.GetManifestResourceStream(resourceName)) {
      var data = new byte[stream.Length];
      stream.Read(data, 0, data.Length);
      return Task.FromResult(data);
    }
  }

  public static Version Version(this Assembly assembly) => assembly.GetName().Version;

}
