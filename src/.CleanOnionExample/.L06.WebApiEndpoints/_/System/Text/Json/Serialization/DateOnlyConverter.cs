namespace System.Text.Json.Serialization;

public static class Extensions {
  public static IServiceCollection AddDateOnlyConverter(this IServiceCollection services) => services.Configure<JsonSerializerOptions>(options => options.Converters.Add(new DateOnlyConverter()));
  public static IServiceCollection AddTimeOnlyConverter(this IServiceCollection services) => services.Configure<JsonSerializerOptions>(options => options.Converters.Add(new TimeOnlyConverter()));

}


[Obsolete(".NET 7")]
public class DateOnlyConverter : JsonConverter<DateOnly> {
  private readonly string serializationFormat;

  public DateOnlyConverter() : this(null) {


  }

  public DateOnlyConverter(string? serializationFormat) {
    this.serializationFormat = serializationFormat ?? "yyyy-MM-dd";
  }

  public override DateOnly Read(ref Utf8JsonReader reader,
                          Type typeToConvert, JsonSerializerOptions options) {
    var value = reader.GetString();
    return DateOnly.Parse(value!);
  }

  public override void Write(Utf8JsonWriter writer, DateOnly value,
                                      JsonSerializerOptions options)
      => writer.WriteStringValue(value.ToString(serializationFormat));
}

[Obsolete(".NET 7")]
public class TimeOnlyConverter : JsonConverter<TimeOnly> {
  private readonly string serializationFormat;

  public TimeOnlyConverter() : this(null) {
  }

  public TimeOnlyConverter(string? serializationFormat) {
    this.serializationFormat = serializationFormat ?? "HH:mm:ss.fff";
  }

  public override TimeOnly Read(ref Utf8JsonReader reader,
                          Type typeToConvert, JsonSerializerOptions options) {
    var value = reader.GetString();
    return TimeOnly.Parse(value!);
  }

  public override void Write(Utf8JsonWriter writer, TimeOnly value,
                                      JsonSerializerOptions options)
      => writer.WriteStringValue(value.ToString(serializationFormat));
}

