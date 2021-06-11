using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Json.Serialization.ContractResolvers {
  public class IgnoreParentPropertiesContractResolver : DefaultContractResolver {
    bool IgnoreBase = false;
    public IgnoreParentPropertiesContractResolver(bool ignoreBase) {
      IgnoreBase = ignoreBase;
    }
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) {
      var allProps = base.CreateProperties(type, memberSerialization);
      if (!IgnoreBase) return allProps;
      //Choose the properties you want to serialize/deserialize
      var props = type.GetProperties(~BindingFlags.FlattenHierarchy);
      return allProps.Where(p => props.Any(a => a.Name == p.PropertyName)).ToList();
    }
  }

}