using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace Newtonsoft.Json.Converters {
  public class IPEndPointJsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) => (objectType == typeof(IPEndPoint));

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
      IPEndPoint ep = (IPEndPoint)value;
      JObject jo = new JObject {
        { nameof(IPEndPoint.Address), JToken.FromObject(ep.Address, serializer) },
        { nameof(IPEndPoint.Port), ep.Port }
      };
      jo.WriteTo(writer);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
      JObject jo = JObject.Load(reader);
      IPAddress address = jo[nameof(IPEndPoint.Address)].ToObject<IPAddress>(serializer);
      int port = (int)jo[nameof(IPEndPoint.Port)];
      return new IPEndPoint(address, port);
    }

  }
}