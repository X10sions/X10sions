﻿using System.Reflection;

namespace Newtonsoft.Json.Serialization;
public class PropertyRenameAndIgnoreSerializerContractResolver : DefaultContractResolver {
  // https://blog.rsuter.com/advanced-newtonsoft-json-dynamically-rename-or-ignore-properties-without-changing-the-serialized-class/

  private readonly Dictionary<Type, HashSet<string>> _ignores = new Dictionary<Type, HashSet<string>>();
  private readonly Dictionary<Type, Dictionary<string, string>> _renames = new Dictionary<Type, Dictionary<string, string>>();

  public void IgnoreProperty(Type type, params string[] jsonPropertyNames) {
    if (!_ignores.ContainsKey(type))
      _ignores[type] = new HashSet<string>();
    foreach (var prop in jsonPropertyNames)
      _ignores[type].Add(prop);
  }

  //public void RenameProperty(Type type, string propertyName, string newJsonPropertyName) {
  //  if (!_renames.ContainsKey(type))
  //    _renames[type] = new Dictionary<string, string>();
  //  _renames[type][propertyName] = newJsonPropertyName;
  //}

  protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
    var property = base.CreateProperty(member, memberSerialization);
    if (IsIgnored(property.DeclaringType, property.PropertyName)) {
      property.ShouldSerialize = i => false;
      property.Ignored = true;
    }
    if (IsRenamed(property.DeclaringType, property.PropertyName, out var newJsonPropertyName))
      property.PropertyName = newJsonPropertyName;
    return property;
  }

  private bool IsIgnored(Type? type, string? jsonPropertyName) {
    if (type == null || jsonPropertyName == null) return false;
    if (!_ignores.ContainsKey(type)) return false;
    return _ignores[type].Contains(jsonPropertyName);
  }

  private bool IsRenamed(Type? type, string? jsonPropertyName, out string? newJsonPropertyName) {
    var renames = new Dictionary<string, string>();
    newJsonPropertyName = null;
    if (type != null && !_renames.TryGetValue(type, out renames) || jsonPropertyName != null && !renames.TryGetValue(jsonPropertyName, out newJsonPropertyName)) {
      return false;
    }
    return true;
  }

}
