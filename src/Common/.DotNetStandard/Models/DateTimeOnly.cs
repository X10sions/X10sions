using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Common.Models;

interface ISpanFormattable { }
interface ISpanParsable<T> { }
interface IUtfChar { }
interface IUtf8SpanFormattable { }

public static class DateTimeOnlyExtensions {
  // Number of days in a non-leap year
  private const int DaysPerYear = 365;
  // Number of days in 4 years
  private const int DaysPer4Years = DaysPerYear * 4 + 1;       // 1461
                                                               // Number of days in 100 years
  private const int DaysPer100Years = DaysPer4Years * 25 - 1;  // 36524
                                                               // Number of days in 400 years
  private const int DaysPer400Years = DaysPer100Years * 4 + 1; // 146097
                                                               // Number of days from 1/1/0001 to 12/31/1600
  private const int DaysTo1601 = DaysPer400Years * 4;          // 584388
                                                               // Number of days from 1/1/0001 to 12/30/1899
  private const int DaysTo1899 = DaysPer400Years * 4 + DaysPer100Years * 3 - 367;
  // Number of days from 1/1/0001 to 12/31/1969
  internal const int DaysTo1970 = DaysPer400Years * 4 + DaysPer100Years * 3 + DaysPer4Years * 17 + DaysPerYear; // 719,162
                                                                                                                // Number of days from 1/1/0001 to 12/31/9999
  internal const int DaysTo10000 = DaysPer400Years * 25 - 366;  // 3652059



  internal static void ThrowMillisecondOutOfRange() => throw new ArgumentOutOfRangeException("millisecond", SR.Format(SR.ArgumentOutOfRange_Range, 0, TimeSpanExtensions.MillisecondsPerSecond - 1));
  internal static void ThrowMicrosecondOutOfRange() => throw new ArgumentOutOfRangeException("microsecond", SR.Format(SR.ArgumentOutOfRange_Range, 0, TimeSpanExtensions.MicrosecondsPerMillisecond - 1));

  // Return the tick count corresponding to the given hour, minute, second.
  // Will check the if the parameters are valid.
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static ulong TimeToTicks(int hour, int minute, int second) {
    if ((uint)hour >= 24 || (uint)minute >= 60 || (uint)second >= 60) {
      ThrowHelper.ThrowArgumentOutOfRange_BadHourMinuteSecond();
    }
    int totalSeconds = hour * 3600 + minute * 60 + second;
    return (uint)totalSeconds * (ulong)TimeSpan.TicksPerSecond;
  }

  internal static ulong TimeToTicks(int hour, int minute, int second, int millisecond) {
    ulong ticks = TimeToTicks(hour, minute, second);
    if ((uint)millisecond >= TimeSpanExtensions.MillisecondsPerSecond) ThrowMillisecondOutOfRange();
    ticks += (uint)millisecond * (uint)TimeSpanExtensions.TicksPerMillisecond;
    Debug.Assert(ticks <= TimeSpanExtensions.MaxTicks, "Input parameters validated already");
    return ticks;
  }
  internal static ulong TimeToTicks(int hour, int minute, int second, int millisecond, int microsecond) {
    ulong ticks = TimeToTicks(hour, minute, second, millisecond);
    if ((uint)microsecond >= TimeSpanExtensions.MicrosecondsPerMillisecond) ThrowMicrosecondOutOfRange();
    ticks += (uint)microsecond * (uint)TimeSpanExtensions.TicksPerMicrosecond;
    Debug.Assert(ticks <= TimeSpanExtensions.MaxTicks, "Input parameters validated already");
    return ticks;
  }

}

public class DateTimeFormat {

  internal static readonly DateTimeFormatInfo InvariantFormatInfo = CultureInfo.InvariantCulture.DateTimeFormat;
  private static readonly string[] s_invariantAbbreviatedMonthNames = InvariantFormatInfo.AbbreviatedMonthNames;
  private static readonly string[] s_invariantAbbreviatedDayNames = InvariantFormatInfo.AbbreviatedDayNames;

  //internal static bool TryFormat<TChar>(DateTime dateTime, Span<TChar> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) where TChar : unmanaged, IUtfChar<TChar> => TryFormat(dateTime, destination, out charsWritten, format, provider, new TimeSpan(NullOffset));

  //internal static unsafe bool TryFormatTimeOnlyO<TChar>(TimeOnly value, Span<TChar> destination, out int charsWritten) where TChar : unmanaged, IUtfChar<TChar> {
  //  if (destination.Length < 16) {
  //    charsWritten = 0;
  //    return false;
  //  }
  //  charsWritten = 16;
  //  value.ToDateTime().GetTimePrecise(out int hour, out int minute, out int second, out int fraction);
  //  fixed (TChar* dest = &MemoryMarshal.GetReference(destination)) {
  //    Number.WriteTwoDigits((uint)hour, dest);
  //    dest[2] = TChar.CastFrom(':');
  //    Number.WriteTwoDigits((uint)minute, dest + 3);
  //    dest[5] = TChar.CastFrom(':');
  //    Number.WriteTwoDigits((uint)second, dest + 6);
  //    dest[8] = TChar.CastFrom('.');
  //    Number.WriteDigits((uint)fraction, dest + 9, 7);
  //  }
  //  return true;
  //}

  //internal static unsafe bool TryFormatTimeOnlyR<TChar>(TimeOnly value, Span<TChar> destination, out int charsWritten) where TChar : unmanaged, IUtfChar<TChar> {
  //  if (destination.Length < 8) {
  //    charsWritten = 0;
  //    return false;
  //  }
  //  charsWritten = 8;
  //  value.ToDateTime().GetTime(out int hour, out int minute, out int second);
  //  fixed (TChar* dest = &MemoryMarshal.GetReference(destination)) {
  //    Number.WriteTwoDigits((uint)hour, dest);
  //    dest[2] = TChar.CastFrom(':');
  //    Number.WriteTwoDigits((uint)minute, dest + 3);
  //    dest[5] = TChar.CastFrom(':');
  //    Number.WriteTwoDigits((uint)second, dest + 6);
  //  }
  //  return true;
  //}

}

internal static class DateTimeParse {


}

internal enum ParseFailureKind {
  None,

  ArgumentNull_String,
  Format_BadDatePattern,
  Format_BadDateTime,
  Format_BadDateTimeCalendar,
  Format_BadDayOfWeek,
  Format_BadFormatSpecifier,
  Format_BadQuote,
  Format_DateOutOfRange,
  Format_MissingIncompleteDate,
  Format_NoFormatSpecifier,
  Format_OffsetOutOfRange,
  Format_RepeatDateTimePattern,
  Format_UnknownDateTimeWord,
  Format_UTCOutOfRange,

  Argument_InvalidDateStyles,
  Argument_BadFormatSpecifier,
  Format_BadDateOnly,
  Format_BadTimeOnly,
  Format_DateTimeOnlyContainsNoneDateParts,  // DateOnly and TimeOnly specific value. Unrelated date parts when parsing DateOnly or Unrelated time parts when parsing TimeOnly
}

[Flags]
internal enum ParseFlags {
  HaveYear = 0x00000001,
  HaveMonth = 0x00000002,
  HaveDay = 0x00000004,
  HaveHour = 0x00000008,
  HaveMinute = 0x00000010,
  HaveSecond = 0x00000020,
  HaveTime = 0x00000040,
  HaveDate = 0x00000080,
  TimeZoneUsed = 0x00000100,
  TimeZoneUtc = 0x00000200,
  ParsedMonthName = 0x00000400,
  CaptureOffset = 0x00000800,
  YearDefault = 0x00001000,
  Rfc1123Pattern = 0x00002000,
  UtcSortPattern = 0x00004000,
}

/// <summary>
/// https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/TimeSpan.cs
/// </summary>
public static class TimeSpanExtensions {
  public const long NanosecondsPerTick = 100;                                                 //             100
  public const long TicksPerMicrosecond = 10;                                                 //              10
  public const long TicksPerMillisecond = TicksPerMicrosecond * 1000;                         //          10,000
  public const long TicksPerSecond = TicksPerMillisecond * 1000;                              //      10,000,000
  public const long TicksPerMinute = TicksPerSecond * 60;                                     //     600,000,000
  public const long TicksPerHour = TicksPerMinute * 60;                                       //  36,000,000,000
  public const long TicksPerDay = TicksPerHour * 24;                                          // 864,000,000,000
  public const long MicrosecondsPerMillisecond = TicksPerMillisecond / TicksPerMicrosecond;   //           1,000
  public const long MicrosecondsPerSecond = TicksPerSecond / TicksPerMicrosecond;             //       1,000,000
  public const long MicrosecondsPerMinute = TicksPerMinute / TicksPerMicrosecond;             //      60,000,000
  public const long MicrosecondsPerHour = TicksPerHour / TicksPerMicrosecond;                 //   3,600,000,000
  public const long MicrosecondsPerDay = TicksPerDay / TicksPerMicrosecond;                   //  86,400,000,000
  public const long MillisecondsPerSecond = TicksPerSecond / TicksPerMillisecond;             //           1,000
  public const long MillisecondsPerMinute = TicksPerMinute / TicksPerMillisecond;             //          60,000
  public const long MillisecondsPerHour = TicksPerHour / TicksPerMillisecond;                 //       3,600,000
  public const long MillisecondsPerDay = TicksPerDay / TicksPerMillisecond;                   //      86,400,000
  public const long SecondsPerMinute = TicksPerMinute / TicksPerSecond;                       //              60
  public const long SecondsPerHour = TicksPerHour / TicksPerSecond;                           //           3,600
  public const long SecondsPerDay = TicksPerDay / TicksPerSecond;                             //          86,400
  public const long MinutesPerHour = TicksPerHour / TicksPerMinute;                           //              60
  public const long MinutesPerDay = TicksPerDay / TicksPerMinute;                             //           1,440
  public const int HoursPerDay = (int)(TicksPerDay / TicksPerHour);                           //              24

  internal const long MinTicks = long.MinValue;                                               // -9,223,372,036,854,775,808
  internal const long MaxTicks = long.MaxValue;                                               // +9,223,372,036,854,775,807
  internal const long MinMicroseconds = MinTicks / TicksPerMicrosecond;                       // -  922,337,203,685,477,580
  internal const long MaxMicroseconds = MaxTicks / TicksPerMicrosecond;                       // +  922,337,203,685,477,580
  internal const long MinMilliseconds = MinTicks / TicksPerMillisecond;                       // -      922,337,203,685,477
  internal const long MaxMilliseconds = MaxTicks / TicksPerMillisecond;                       // +      922,337,203,685,477
  internal const long MinSeconds = MinTicks / TicksPerSecond;                                 // -          922,337,203,685
  internal const long MaxSeconds = MaxTicks / TicksPerSecond;                                 // +          922,337,203,685
  internal const long MinMinutes = MinTicks / TicksPerMinute;                                 // -           15,372,286,728
  internal const long MaxMinutes = MaxTicks / TicksPerMinute;                                 // +           15,372,286,728
  internal const long MinHours = MinTicks / TicksPerHour;                                     // -              256,204,778
  internal const long MaxHours = MaxTicks / TicksPerHour;                                     // +              256,204,778
  internal const long MinDays = MinTicks / TicksPerDay;                                       // -               10,675,199
  internal const long MaxDays = MaxTicks / TicksPerDay;                                       // +               10,675,199
  internal const long TicksPerTenthSecond = TicksPerMillisecond * 100;

}