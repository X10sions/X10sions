namespace Microsoft.AspNetCore.Routing {
  public static class RouteValueDictionaryExtensions {

    public static string Namespace(this RouteValueDictionary rvd) => string.Join(".", rvd.Namespaces());
    public static string[] Namespaces(this RouteValueDictionary rvd) => (string[])rvd["Namespace"];

    public static string? GetString(this RouteValueDictionary rvd, string key, string? defaultIfNull = null) => rvd[key]?.ToString() ?? defaultIfNull;

    public static string MvcCodeRouting_BaseRoute(this RouteValueDictionary rvd) => rvd.GetString("MvcCodeRouting.BaseRoute");
    public static string MvcCodeRouting_RouteContext(this RouteValueDictionary rvd) => rvd.GetString("MvcCodeRouting.RouteContext");
    public static string MvcCodeRouting_ViewsLocation(this RouteValueDictionary rvd) => rvd.GetString("MvcCodeRouting.ViewsLocation");

  }
}