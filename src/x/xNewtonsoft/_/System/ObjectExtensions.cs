using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
//using System.Diagnostics;
//using System.IO;
//using System.Text;

namespace System {
  public static class ObjectExtensions {

    public static JObject ToJObject(this object obj) => JObject.Parse(obj.ToJsonString());

    public static string ToJsonString(this object obj, JsonSerializerSettings? settings = null) => JsonConvert.SerializeObject(obj, settings ?? DefaultJsonSerializerSettings);

    public static string ToJsonString(this object obj, string[] excludePropertyNames) => obj.ToJsonString(new JsonSerializerSettings {
      ContractResolver = new ExcludePropertyNamesContractResolver(excludePropertyNames),
      Formatting = Formatting.Indented,
      ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    });

    public static JsonSerializerSettings DefaultJsonSerializerSettings = new JsonSerializerSettings {
      Formatting = Formatting.Indented
    };

    //public static string ToJsonString2(this object obj, JsonSerializerSettings settings = null) {
    //  settings = settings ?? new JsonSerializerSettings();
    //  var sb = new StringBuilder();
    //  using (var writer = new StringWriter(sb))
    //  using (var jsonWriter = new JsonTextWriter(writer)) {
    //    var oldError = settings.Error;
    //    var oldTraceWriter = settings.TraceWriter;
    //    var oldFormatting = settings.Formatting;
    //    try {
    //      settings.Formatting = Formatting.Indented;
    //      if (settings.TraceWriter == null)
    //        settings.TraceWriter = new MemoryTraceWriter();
    //      settings.Error = oldError + ((object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args) => {
    //        jsonWriter.Flush();
    //        var logSb = new StringBuilder();
    //        logSb.AppendLine("Serialization error: ");
    //        logSb.Append("Path: ").Append(args.ErrorContext.Path).AppendLine();
    //        logSb.Append("Member: ").Append(args.ErrorContext.Member).AppendLine();
    //        logSb.Append("OriginalObject: ").Append(args.ErrorContext.OriginalObject).AppendLine();
    //        logSb.AppendLine("Error: ").Append(args.ErrorContext.Error).AppendLine();
    //        logSb.AppendLine("Partial serialization results: ").Append(sb).AppendLine();
    //        logSb.AppendLine("TraceWriter contents: ").Append(settings.TraceWriter).AppendLine();
    //        logSb.AppendLine("JavaScriptSerializer serialization: ");
    //        try {
    //          logSb.AppendLine(JsonConvert.SerializeObject(obj));
    //        } catch (Exception ex) {
    //          logSb.AppendLine("Failed, error: ").AppendLine(ex.ToString());
    //        }
    //        logSb.AppendLine("XmlSerializer serialization: ");
    //        try {
    //          logSb.AppendLine(obj.GetXmlString());
    //        } catch (Exception ex) {
    //          logSb.AppendLine("Failed, error: ").AppendLine(ex.ToString());
    //        }
    //        logSb.AppendLine("BinaryFormatter serialization: ");
    //        try {
    //          logSb.AppendLine(obj.ToBase64String());
    //        } catch (Exception ex) {
    //          logSb.AppendLine("Failed, error: ").AppendLine(ex.ToString());
    //        }
    //        Debug.WriteLine(logSb);
    //      });
    //      var serializer = JsonSerializer.CreateDefault(settings);
    //      serializer.Serialize(jsonWriter, obj);
    //    } finally {
    //      settings.Error = oldError;
    //      settings.TraceWriter = oldTraceWriter;
    //      settings.Formatting = oldFormatting;
    //    }
    //  }
    //  return sb.ToString();
    //}

  }
}