using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace chartjs {

  public class PluginDynamic {
    public string? PropertyName { get; set; }
    public object? PropertyValue { get; set; }
  }

  public class PluginDynamicConverter : JsonConverter {
    public override bool CanConvert(Type objecttype) => objecttype == typeof(IList<PluginDynamic>);

    public override object ReadJson(JsonReader reader, Type objecttype, object? existingvalue, JsonSerializer serializer) => throw new NotImplementedException();

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
      var dynamicProperties = (IList<PluginDynamic>)value;
      // create a jobject from the document, respecting existing json attribs
      var jobject = JArray.FromObject(value);
      // replace the incorrectly serialized plugindynamic list with all required objects
      var jproperty = jobject.Children<JProperty>().Where(p => p.Name == nameof(PluginDynamic)).First();
      foreach (PluginDynamic dynamicproperty in dynamicProperties) {
        jproperty.AddAfterSelf(new JProperty(dynamicproperty.PropertyName, dynamicproperty.PropertyValue));
      }
      jproperty.Remove();
      // write out the json
      jobject.WriteTo(writer);
    }

  }
}