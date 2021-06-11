using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace xNonFactors.MvcTemplate.Resources {
  public static class Resource {
    private static ConcurrentDictionary<string, ResourceSet> Resources { get; }

    static Resource() {
      Resources = new ConcurrentDictionary<string, ResourceSet>();
      var path = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Resources";
      if (Directory.Exists(path)) {
        foreach (var resource in Directory.GetFiles(path, "*.json", SearchOption.AllDirectories)) {
          var type = Path.GetFileNameWithoutExtension(resource);
          var language = Path.GetExtension(type).TrimStart('.');
          type = Path.GetFileNameWithoutExtension(type);

          Set(type).Override(language, File.ReadAllText(resource));
        }
      }
    }

    public static ResourceSet Set(string type) {
      if (!Resources.ContainsKey(type))
        Resources[type] = new ResourceSet();
      return Resources[type];
    }

    public static string ForArea(string name) => Localized("Shared", "Areas", name);
    public static string ForAction(string name) => Localized("Shared", "Actions", name);
    public static string ForController(string name) => Localized("Shared", "Controllers", name);

    public static string ForLookup(string handler) => Localized("Lookup", "Titles", handler);

    public static string ForString(string key, params object[] args) {
      var value = Localized("Shared", "Strings", key);
      return string.IsNullOrEmpty(value) || args.Length == 0 ? value : string.Format(value, args);
    }

    public static string ForHeader(string model) => Localized("Page", "Headers", model);

    public static string ForPage(string path) => Localized("Page", "Titles", path);
    public static string ForPage(IDictionary<string, object?> path) {
      var area = path["area"] as string;
      var action = path["action"] as string;
      var controller = path["controller"] as string;

      return ForPage($"{area}/{controller}/{action}");
    }

    public static string ForSiteMap(string path) => Localized("SiteMap", "Titles", path);

    public static string ForProperty<TView, TProperty>(Expression<Func<TView, TProperty>> expression) => ForProperty(expression.Body);
    public static string ForProperty(string view, string name) {
      if (Localized(view, "Titles", name) is string title && title.Length > 0)
        return title;

      var properties = SplitCamelCase(name);
      var language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

      for (var skipped = 0; skipped < properties.Length; skipped++) {
        for (var viewSize = 1; viewSize < properties.Length - skipped; viewSize++) {
          var relation = $"{string.Concat(properties.Skip(skipped).Take(viewSize))}View";
          var property = string.Concat(properties.Skip(viewSize + skipped));

          if (Localized(relation, "Titles", property) is string relationTitle && relationTitle.Length > 0)
            return Set(view)[language, "Titles", name] = relationTitle;
        }
      }

      return "";
    }
    public static string ForProperty(Type view, string name) => ForProperty(view.Name, name);
    public static string ForProperty(Expression expression) => expression is MemberExpression member ? ForProperty(member.Expression.Type, member.Member.Name) : "";

    internal static string Localized(string type, string group, string key) {
      var resources = Set(type);
      var language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

      return resources[language, group, key] ?? resources["", group, key] ?? "";
    }

    private static string[] SplitCamelCase(string value) => Regex.Split(value, "(?<!^)(?=[A-Z])");
  }
}