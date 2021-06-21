using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json {
  public static class JsonConvertExtensions {

    public static string SerializeObjectCamelCase(object value, string dateTimeFormat = "yyyy-MM-dd") {
      return JsonConvert.SerializeObject(value,
          new JsonSerializerSettings {
            DateFormatString = dateTimeFormat,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
          });
    }

    public static string SerializeObjectSnakeCase(object value, string dateTimeFormat = "yyyy-MM-dd") {
      return JsonConvert.SerializeObject(value,
          new JsonSerializerSettings {
            DateFormatString = dateTimeFormat,
            ContractResolver = new SnakeCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
          });
    }

  }
}