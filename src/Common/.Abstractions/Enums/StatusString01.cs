using Common.Attributes;
using System;

namespace Common.Enums {
  public enum StatusString01 {
    [EnumValue("0")] NotActive = 0,
    [EnumValue("1")] Active = 1
  }

  public static class StatusString01Extensions {
    public static string AsIntToString(this StatusString01 e) => ((int)e).ToString();
    public static string StringValue(this StatusString01 e) => e.GetAttribute<EnumValueAttribute>().Value.ToString();
  }
}