using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Newtonsoft.Json.Serialization {
  public class PrivatePropertiesAndFieldsContractResolver : DefaultContractResolver {
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) {
      var props = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
        .Select(p => base.CreateProperty(p, memberSerialization))
        .Union(type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
        .Select(f => base.CreateProperty(f, memberSerialization)))
        .ToList();
      props.ForEach(p => { p.Writable = true; p.Readable = true; });
      return props;
    }
  }

}