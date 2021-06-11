using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Reflection;

namespace Common.Json.Serialization.ContractResolvers {
  public class IgnoreTypesContractResolver : DefaultContractResolver {

    private Type[] _typesToIgnore;

    private string[] _propertyNamesToInclude;

    public IgnoreTypesContractResolver(string[] propertyNamesToInclude, params Type[] typesToIgnore) {
      _propertyNamesToInclude = propertyNamesToInclude;
      _typesToIgnore = typesToIgnore;
    }


    public IgnoreTypesContractResolver(params Type[] typesToIgnore) : this(new string[] { }, typesToIgnore) { }

    //protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) {
    //  var properties = base.CreateProperties(type, memberSerialization);
    //  properties = properties.Where(p => !_typesToIgnore.Contains(p.PropertyType)).ToList();
    //  return properties;
    //}

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
      var prop = base.CreateProperty(member, memberSerialization);
      if (_typesToIgnore.Contains(prop.PropertyType)) {
        prop.Ignored = true;
      }
      if (_propertyNamesToInclude.Contains(prop.PropertyName)) {
        prop.Ignored = false;
      }
      return prop;
    }

  }

}