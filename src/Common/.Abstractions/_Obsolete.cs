using Common.Constants;
using Common.Enums;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Common.Abstractions {
  [Obsolete("To be deleted")]
  public static class _ObsoleteExtensions {

    public static object Obsolete_GetDefaultValue2(this Type type) {
      if (!type.IsValueType) {
        return null;
      }
      if (!Obsolete_DefaultValueDictionary.Instance.TryGetValue(type, out var result)) {
        return Activator.CreateInstance(type);
      }
      return result;
    } 

    [Obsolete]
    public static long Obsolete_DateDiff(this DateInterval intervalType, DateTime dateOne, DateTime dateTwo, DayOfWeek? firstDayOfWeek = null) {
      switch (intervalType) {
        case DateInterval.Day:
        case DateInterval.DayOfYear: return (long)(dateTwo - dateOne).TotalDays;
        case DateInterval.Hour: return (long)(dateTwo - dateOne).TotalHours;
        case DateInterval.Minute: return (long)(dateTwo - dateOne).TotalMinutes;
        case DateInterval.Month: return ((dateTwo.Year - dateOne.Year) * 12) + (dateTwo.Month - dateOne.Month);
        case DateInterval.Quarter: return (long)((4 * (dateTwo.Year - dateOne.Year)) + Math.Ceiling(dateTwo.Month / 3.0) - Math.Ceiling(dateOne.Month / 3.0));
        case DateInterval.Second: return (long)(dateTwo - dateOne).TotalSeconds;
        case DateInterval.Weekday: return (long)((dateTwo - dateOne).TotalDays / 7.0);
        case DateInterval.WeekOfYear:
          firstDayOfWeek = firstDayOfWeek ?? DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek;
          return (long)((dateOne.FirstDayOfWeek(firstDayOfWeek) - dateTwo.FirstDayOfWeek(firstDayOfWeek)).TotalDays / 7.0);
        case DateInterval.Year: return dateTwo.Year - dateOne.Year;
        default: return 0;
      }
    }

    [Obsolete("VB replace with: ")] public static readonly TypeCode[] Obsolete_TypeCodeConstants_NumericVB = TypeCodeConstants.Numeric.Union(new[] { TypeCode.Boolean }).ToArray();

    [Obsolete("Use application/javascript")] public const string Obsolete_Constants_MediaTypeNames_Text_Javascript = "text/javascript";

    [Obsolete("VB replace with: ")] public static bool Obsolete_IsOldNumeric(this TypeCode typeCode) => Obsolete_TypeCodeConstants_NumericVB.Contains(typeCode);

  }
}

namespace Common.Enums {
  [Obsolete]
  public enum Obsolete_AllowStringType {
    Any,
    NotNull,
    NotNullOrEmpty,
    NotNullOrWhitespace
  }
}
