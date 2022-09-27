using Newtonsoft.Json.Linq;
using System.Net;

namespace Newtonsoft.Json.Converters {
  public class IPEndPointJsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) => objectType == typeof(IPEndPoint);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
      JObject jo = JObject.Load(reader);
      var address = jo[nameof(IPEndPoint.Address)]?.ToObject<IPAddress>(serializer);
      var port = (int?)jo[nameof(IPEndPoint.Port)] ;
      return  address != null && port != null ? new IPEndPoint(address, port.Value) : null;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
      //if (value == null) return;
      JToken? port = null;
      JToken? address = null;
      if (value is IPEndPoint) {
        var ep = (IPEndPoint)value;
        port = ep.Port;
        address = JToken.FromObject(ep.Address, serializer);
      }
      JObject jo = new JObject {
        { nameof(IPEndPoint.Address), address},
        { nameof(IPEndPoint.Port), port }
      };
      jo.WriteTo(writer);
    }

  }
}