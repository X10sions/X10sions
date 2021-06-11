using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.Json;

namespace xNonFactors.MvcTemplate.Resources {
  public class ResourceSet {
    private ConcurrentDictionary<string, ConcurrentDictionary<string, ResourceDictionary>> Source { get; }

    public ResourceSet() {
      Source = new ConcurrentDictionary<string, ConcurrentDictionary<string, ResourceDictionary>>();
    }

    public string? this[string language, string group, string key] {
      get {
        if (!Source.ContainsKey(language)) return null;
        if (!Source[language].ContainsKey(group)) return null;
        return Source[language][group].TryGetValue(key, out var title) ? title : null;
      }
      set {
        if (!Source.ContainsKey(language))
          Source[language] = new ConcurrentDictionary<string, ResourceDictionary>();
        if (!Source[language].ContainsKey(group))
          Source[language][group] = new ResourceDictionary();
        Source[language][group][key] = value;
      }
    }

    public void Override(string language, string source) {
      var resources = JsonSerializer.Deserialize<Dictionary<string, ResourceDictionary>>(source);
      foreach (var group in resources.Keys)
        foreach (var key in resources[group].Keys)
          this[language, group, key] = resources[group][key];
    }

    public void Inherit(ResourceSet resources) {
      foreach (var language in resources.Source.Keys)
        foreach (var group in resources.Source[language].Keys)
          foreach (var key in resources.Source[language][group].Keys)
            this[language, group, key] ??= resources.Source[language][group][key];
    }
  }
}