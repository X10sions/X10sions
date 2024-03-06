using System.Reflection;

namespace X10sions.Wsus.Pages.Shared {
  public class _LayoutSettings {
    public _LayoutSettings(string webAppName, string webAppTitle) {
      WebAppAssemblyName = webAppName;
      WebAppTitle = webAppTitle;
    }
    public _LayoutSettings(Assembly webAppAssembly, string webAppTitle) : this(webAppAssembly.GetName().Name, webAppTitle) { }
    public _LayoutSettings(string webAppTitle) : this(Assembly.GetEntryAssembly(), webAppTitle) { }

    public string WebAppAssemblyName { get; }
    public string WebAppTitle { get; }
    public string WebAppNameStylesCss => $"/{WebAppAssemblyName}.styles.css";

    public string ContentPath<T>(string pathSuffix) => $"/_content/{typeof(T).Assembly.GetName().Name}{pathSuffix}";

  }
}