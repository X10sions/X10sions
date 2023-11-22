using Common.Attributes;

namespace Common.Enums;
public enum StatusString01 {
  [EnumValue<string>("0")] NotActive = 0,
  [EnumValue<string>("1")] Active = 1
}

public static class StatusString01Extensions {
  public static string AsIntToString(this StatusString01 e) => ((int)e).ToString();
  public static string StringValue(this StatusString01 e) => e.GetCustomAttribute<EnumValueAttribute<string>>().Value;
}