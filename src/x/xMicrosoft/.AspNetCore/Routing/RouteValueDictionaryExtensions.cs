namespace Microsoft.AspNetCore.Routing {
  public static class RouteValueDictionaryExtensions {

    // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-2.2#reserved-routing-names
    public static string? Action(this RouteValueDictionary rvd) => rvd[nameof(Action)]?.ToString();
    public static string? Area(this RouteValueDictionary rvd) => rvd[nameof(Area)]?.ToString();
    public static string? Controller(this RouteValueDictionary rvd) => rvd[nameof(Controller)]?.ToString();
    public static string? Handler(this RouteValueDictionary rvd) => rvd[nameof(Handler)]?.ToString();
    public static string? Page(this RouteValueDictionary rvd) => rvd[nameof(Page)]?.ToString();

    public static string Namespace(this RouteValueDictionary rvd) => string.Join(".", rvd.Namespaces());
    public static string[] Namespaces(this RouteValueDictionary rvd) => (string[])rvd["Namespace"];

    public static string? GetString(this RouteValueDictionary rvd, string key, string? defaultIfNull = null) => rvd[key]?.ToString() ?? defaultIfNull;

    public static string MvcCodeRouting_BaseRoute(this RouteValueDictionary rvd) => rvd.GetString("MvcCodeRouting.BaseRoute");
    public static string MvcCodeRouting_RouteContext(this RouteValueDictionary rvd) => rvd.GetString("MvcCodeRouting.RouteContext");
    public static string MvcCodeRouting_ViewsLocation(this RouteValueDictionary rvd) => rvd.GetString("MvcCodeRouting.ViewsLocation");

  }
}