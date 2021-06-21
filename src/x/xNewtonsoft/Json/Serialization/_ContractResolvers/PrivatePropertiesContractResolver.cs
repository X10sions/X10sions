using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Newtonsoft.Json.Serialization {
  public class PrivatePropertiesContractResolver : DefaultContractResolver {

    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) => type
          .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic)
          .Select(p => CreateProperty(p, memberSerialization))
          .ToList();

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
      var prop = base.CreateProperty(member, memberSerialization);
      if (!prop.Writable && (member as PropertyInfo)?.GetSetMethod(true) != null) {
        prop.Writable = true;
      }
      return prop;
    }
  }

  public class PrivateFieldsContractResolver : DefaultContractResolver {
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) {
      var props = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
        .Select(f => base.CreateProperty(f, memberSerialization))
        .ToList();
      props.ForEach(p => { p.Writable = true; p.Readable = true; });
      return props;
    }
  }

}