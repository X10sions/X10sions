using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Common.Models;

/// <summary>
/// Represents dates with values ranging from January 1, 0001 Anno Domini (Common Era) through December 31, 9999 A.D. (C.E.) in the Gregorian calendar.
/// https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/DateOnly.cs
/// </summary>
public readonly struct DateOnly
    : IComparable,
      IComparable<DateOnly>,
      IEquatable<DateOnly>,
      ISpanFormattable,
      ISpanParsable<DateOnly>,
      IUtf8SpanFormattable {
  private readonly uint _dayNumber;
  private const int MinDayNumber = 0;
  private const int MaxDayNumber = DateTimeOnlyExtensions.DaysTo10000 - 1;

  private static uint DayNumberFromDateTime(DateTime dt) => (uint)((ulong)dt.Ticks / TimeSpan.TicksPerDay);
  internal DateTime GetEquivalentDateTime() => new DateTime(_dayNumber * TimeSpan.TicksPerDay);
  private DateOnly(uint dayNumber) {
    Debug.Assert(dayNumber <= MaxDayNumber);
    _dayNumber = dayNumber;
  }

  public static DateOnly MinValue => new DateOnly(MinDayNumber);
  public static DateOnly MaxValue => new DateOnly(MaxDayNumber);
  public DateOnly(int year, int month, int day) => _dayNumber = DayNumberFromDateTime(new DateTime(year, month, day));
  public DateOnly(int year, int month, int day, Calendar calendar) => _dayNumber = DayNumberFromDateTime(new DateTime(year, month, day, calendar));

  public static DateOnly FromDayNumber(int dayNumber) {
    if ((uint)dayNumber > MaxDayNumber) {
      ThrowHelper.ThrowArgumentOutOfRange_DayNumber(dayNumber);
    }
    return new DateOnly((uint)dayNumber);
  }

  public int Year => GetEquivalentDateTime().Year;
  public int Month => GetEquivalentDateTime().Month;
  public int Day => GetEquivalentDateTime().Day;
  public DayOfWeek DayOfWeek => (DayOfWeek)((_dayNumber + 1) % 7);
  public int DayOfYear => GetEquivalentDateTime().DayOfYear;
  public int DayNumber => (int)_dayNumber;
  public DateOnly AddDays(int value) {
    uint newDayNumber = _dayNumber + (uint)value;
    if (newDayNumber > MaxDayNumber) {
      ThrowOutOfRange();
    }
    return new DateOnly(newDayNumber);
    static void ThrowOutOfRange() => throw new ArgumentOutOfRangeException(nameof(value), SR.ArgumentOutOfRange_AddValue);
  }
  public DateOnly AddMonths(int value) => new DateOnly(DayNumberFromDateTime(GetEquivalentDateTime().AddMonths(value)));
  public DateOnly AddYears(int value) => new DateOnly(DayNumberFromDateTime(GetEquivalentDateTime().AddYears(value)));
  public static bool operator ==(DateOnly left, DateOnly right) => left._dayNumber == right._dayNumber;
  public static bool operator !=(DateOnly left, DateOnly right) => left._dayNumber != right._dayNumber;
  public static bool operator >(DateOnly left, DateOnly right) => left._dayNumber > right._dayNumber;
  public static bool operator >=(DateOnly left, DateOnly right) => left._dayNumber >= right._dayNumber;
  public static bool operator <(DateOnly left, DateOnly right) => left._dayNumber < right._dayNumber;
  public static bool operator <=(DateOnly left, DateOnly right) => left._dayNumber <= right._dayNumber;
  [EditorBrowsable(EditorBrowsableState.Never)]
  //public void Deconstruct(out int year, out int month, out int day) => GetEquivalentDateTime().GetDate(out year, out month, out day);
  public DateTime ToDateTime() => ToDateTime(TimeOnly.MinValue);
  public DateTime ToDateTime(TimeOnly time) => new DateTime(_dayNumber * TimeSpan.TicksPerDay + time.Ticks);
  public DateTime ToDateTime(TimeOnly time, DateTimeKind kind) => DateTime.SpecifyKind(ToDateTime(time), kind);
  public static DateOnly FromDateTime(DateTime dateTime) => new DateOnly(DayNumberFromDateTime(dateTime));
  public int CompareTo(DateOnly value) => _dayNumber.CompareTo(value._dayNumber);
  public int CompareTo(object? value) {
    if (value == null) return 1;
    if (value is not DateOnly dateOnly) {
      throw new ArgumentException(SR.Arg_MustBeDateOnly);
    }
    return CompareTo(dateOnly);
  }
  public bool Equals(DateOnly value) => _dayNumber == value._dayNumber;
  public override bool Equals([NotNullWhen(true)] object? value) => value is DateOnly dateOnly && _dayNumber == dateOnly._dayNumber;
  public override int GetHashCode() => (int)_dayNumber;
  private const ParseFlags ParseFlagsDateMask = ParseFlags.HaveHour | ParseFlags.HaveMinute | ParseFlags.HaveSecond | ParseFlags.HaveTime | ParseFlags.TimeZoneUsed |
                                                ParseFlags.TimeZoneUtc | ParseFlags.CaptureOffset | ParseFlags.UtcSortPattern;
  //public static DateOnly Parse(ReadOnlySpan<char> s, IFormatProvider? provider = default, DateTimeStyles style = DateTimeStyles.None) {
  //  ParseFailureKind result = TryParseInternal(s, provider, style, out DateOnly dateOnly);
  //  if (result != ParseFailureKind.None) {
  //    ThrowOnError(result, s);
  //  }
  //  return dateOnly;
  //}
  private const string OFormat = "yyyy'-'MM'-'dd";
  private const string RFormat = "ddd, dd MMM yyyy";

  //public static DateOnly ParseExact(ReadOnlySpan<char> s, [StringSyntax(StringSyntaxAttribute.DateOnlyFormat)] ReadOnlySpan<char> format, IFormatProvider? provider = default, DateTimeStyles style = DateTimeStyles.None) {
  //  ParseFailureKind result = TryParseExactInternal(s, format, provider, style, out DateOnly dateOnly);
  //  if (result != ParseFailureKind.None) {
  //    ThrowOnError(result, s);
  //  }
  //  return dateOnly;
  //}

  //public static DateOnly ParseExact(ReadOnlySpan<char> s, [StringSyntax(StringSyntaxAttribute.DateOnlyFormat)] string[] formats) => ParseExact(s, formats, null, DateTimeStyles.None);

  //public static DateOnly ParseExact(ReadOnlySpan<char> s, [StringSyntax(StringSyntaxAttribute.DateOnlyFormat)] string[] formats, IFormatProvider? provider, DateTimeStyles style = DateTimeStyles.None) {
  //  ParseFailureKind result = TryParseExactInternal(s, formats, provider, style, out DateOnly dateOnly);
  //  if (result != ParseFailureKind.None) {
  //    ThrowOnError(result, s);
  //  }
  //  return dateOnly;
  //}

  //public static DateOnly Parse(string s) => Parse(s, null, DateTimeStyles.None);
  //public static DateOnly Parse(string s, IFormatProvider? provider, DateTimeStyles style = DateTimeStyles.None) {
  //  if (s == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
  //  return Parse(s.AsSpan(), provider, style);
  //}
  //public static DateOnly ParseExact(string s, [StringSyntax(StringSyntaxAttribute.DateOnlyFormat)] string format) => ParseExact(s, format, null, DateTimeStyles.None);
  //public static DateOnly ParseExact(string s, [StringSyntax(StringSyntaxAttribute.DateOnlyFormat)] string format, IFormatProvider? provider, DateTimeStyles style = DateTimeStyles.None) {
  //  if (s == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
  //  if (format == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.format);
  //  return ParseExact(s.AsSpan(), format.AsSpan(), provider, style);
  //}
  //public static DateOnly ParseExact(string s, [StringSyntax(StringSyntaxAttribute.DateOnlyFormat)] string[] formats) => ParseExact(s, formats, null, DateTimeStyles.None);
  //public static DateOnly ParseExact(string s, [StringSyntax(StringSyntaxAttribute.DateOnlyFormat)] string[] formats, IFormatProvider? provider, DateTimeStyles style = DateTimeStyles.None) {
  //  if (s == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
  //  return ParseExact(s.AsSpan(), formats, provider, style);
  //}
  //public static bool TryParse(ReadOnlySpan<char> s, out DateOnly result) => TryParse(s, null, DateTimeStyles.None, out result);
  //public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, DateTimeStyles style, out DateOnly result) => TryParseInternal(s, provider, style, out result) == ParseFailureKind.None;

  //private static ParseFailureKind TryParseInternal(ReadOnlySpan<char> s, IFormatProvider? provider, DateTimeStyles style, out DateOnly result) {
  //  if ((style & ~DateTimeStyles.AllowWhiteSpaces) != 0) {
  //    result = default;
  //    return ParseFailureKind.Argument_InvalidDateStyles;
  //  }
  //  DateTimeResult dtResult = default;
  //  dtResult.Init(s);
  //  if (!DateTimeParse.TryParse(s, DateTimeFormatInfo.GetInstance(provider), style, ref dtResult)) {
  //    result = default;
  //    return ParseFailureKind.Format_BadDateOnly;
  //  }
  //  if ((dtResult.flags & ParseFlagsDateMask) != 0) {
  //    result = default;
  //    return ParseFailureKind.Format_DateTimeOnlyContainsNoneDateParts;
  //  }
  //  result = new DateOnly(DayNumberFromDateTime(dtResult.parsedDate));
  //  return ParseFailureKind.None;
  //}

  //public static bool TryParseExact(ReadOnlySpan<char> s, [StringSyntax(StringSyntaxAttribute.DateOnlyFormat)] ReadOnlySpan<char> format, out DateOnly result) => TryParseExact(s, format, null, DateTimeStyles.None, out result);
  //public static bool TryParseExact(ReadOnlySpan<char> s, [StringSyntax(StringSyntaxAttribute.DateOnlyFormat)] ReadOnlySpan<char> format, IFormatProvider? provider, DateTimeStyles style, out DateOnly result) => TryParseExactInternal(s, format, provider, style, out result) == ParseFailureKind.None;
  //private static ParseFailureKind TryParseExactInternal(ReadOnlySpan<char> s, ReadOnlySpan<char> format, IFormatProvider? provider, DateTimeStyles style, out DateOnly result) {
  //  if ((style & ~DateTimeStyles.AllowWhiteSpaces) != 0) {
  //    result = default;
  //    return ParseFailureKind.Argument_InvalidDateStyles;
  //  }
  //  if (format.Length == 1) {
  //    switch (format[0] | 0x20) {
  //      case 'o':
  //        format = OFormat;
  //        provider = DateTimeFormat.InvariantFormatInfo;
  //        break;
  //      case 'r':
  //        format = RFormat;
  //        provider = DateTimeFormat.InvariantFormatInfo;
  //        break;
  //    }
  //  }
  //  DateTimeResult dtResult = default;
  //  dtResult.Init(s);
  //  if (!DateTimeParse.TryParseExact(s, format, DateTimeFormatInfo.GetInstance(provider), style, ref dtResult)) {
  //    result = default;
  //    return ParseFailureKind.Format_BadDateOnly;
  //  }
  //  if ((dtResult.flags & ParseFlagsDateMask) != 0) {
  //    result = default;
  //    return ParseFailureKind.Format_DateTimeOnlyContainsNoneDateParts;
  //  }
  //  result = new DateOnly(DayNumberFromDateTime(dtResult.parsedDate));
  //  return ParseFailureKind.None;
  //}

  //public static bool TryParseExact(ReadOnlySpan<char> s, [NotNullWhen(true), StringSyntax(StringSyntaxAttribute.DateOnlyFormat)] string?[]? formats, out DateOnly result) => TryParseExact(s, formats, null, DateTimeStyles.None, out result);
  //public static bool TryParseExact(ReadOnlySpan<char> s, [NotNullWhen(true), StringSyntax(StringSyntaxAttribute.DateOnlyFormat)] string?[]? formats, IFormatProvider? provider, DateTimeStyles style, out DateOnly result) => TryParseExactInternal(s, formats, provider, style, out result) == ParseFailureKind.None;
  //private static ParseFailureKind TryParseExactInternal(ReadOnlySpan<char> s, string?[]? formats, IFormatProvider? provider, DateTimeStyles style, out DateOnly result) {
  //  if ((style & ~DateTimeStyles.AllowWhiteSpaces) != 0 || formats == null) {
  //    result = default;
  //    return ParseFailureKind.Argument_InvalidDateStyles;
  //  }
  //  DateTimeFormatInfo dtfi = DateTimeFormatInfo.GetInstance(provider);
  //  for (int i = 0; i < formats.Length; i++) {
  //    DateTimeFormatInfo dtfiToUse = dtfi;
  //    string? format = formats[i];
  //    if (string.IsNullOrEmpty(format)) {
  //      result = default;
  //      return ParseFailureKind.Argument_BadFormatSpecifier;
  //    }
  //    if (format.Length == 1) {
  //      switch (format[0] | 0x20) {
  //        case 'o':
  //          format = OFormat;
  //          dtfiToUse = DateTimeFormat.InvariantFormatInfo;
  //          break;
  //        case 'r':
  //          format = RFormat;
  //          dtfiToUse = DateTimeFormat.InvariantFormatInfo;
  //          break;
  //      }
  //    }
  //    DateTimeResult dtResult = default;
  //    dtResult.Init(s);
  //    if (DateTimeParse.TryParseExact(s, format, dtfiToUse, style, ref dtResult) && ((dtResult.flags & ParseFlagsDateMask) == 0)) {
  //      result = new DateOnly(DayNumberFromDateTime(dtResult.parsedDate));
  //      return ParseFailureKind.None;
  //    }
  //  }
  //  result = default;
  //  return ParseFailureKind.Format_BadDateOnly;
  //}

  //public static bool TryParse([NotNullWhen(true)] string? s, out DateOnly result) => TryParse(s, null, DateTimeStyles.None, out result);
  //public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, DateTimeStyles style, out DateOnly result) {
  //  if (s == null) {
  //    result = default;
  //    return false;
  //  }
  //  return TryParse(s.AsSpan(), provider, style, out result);
  //}
  //public static bool TryParseExact([NotNullWhen(true)] string? s, [NotNullWhen(true), StringSyntax(StringSyntaxAttribute.DateOnlyFormat)] string? format, out DateOnly result) => TryParseExact(s, format, null, DateTimeStyles.None, out result);
  //public static bool TryParseExact([NotNullWhen(true)] string? s, [NotNullWhen(true), StringSyntax(StringSyntaxAttribute.DateOnlyFormat)] string? format, IFormatProvider? provider, DateTimeStyles style, out DateOnly result) {
  //  if (s == null || format == null) {
  //    result = default;
  //    return false;
  //  }
  //  return TryParseExact(s.AsSpan(), format.AsSpan(), provider, style, out result);
  //}
  //public static bool TryParseExact([NotNullWhen(true)] string? s, [NotNullWhen(true), StringSyntax(StringSyntaxAttribute.DateOnlyFormat)] string?[]? formats, out DateOnly result) => TryParseExact(s, formats, null, DateTimeStyles.None, out result);
  //public static bool TryParseExact([NotNullWhen(true)] string? s, [NotNullWhen(true), StringSyntax(StringSyntaxAttribute.DateOnlyFormat)] string?[]? formats, IFormatProvider? provider, DateTimeStyles style, out DateOnly result) {
  //  if (s == null) {
  //    result = default;
  //    return false;
  //  }
  //  return TryParseExact(s.AsSpan(), formats, provider, style, out result);
  //}

  private static void ThrowOnError(ParseFailureKind result, ReadOnlySpan<char> s) {
    Debug.Assert(result != ParseFailureKind.None);
    switch (result) {
      case ParseFailureKind.Argument_InvalidDateStyles: throw new ArgumentException(SR.Argument_InvalidDateStyles, "style");
      case ParseFailureKind.Argument_BadFormatSpecifier: throw new FormatException(SR.Argument_BadFormatSpecifier);
      case ParseFailureKind.Format_BadDateOnly: throw new FormatException(SR.Format(SR.Format_BadDateOnly, s.ToString()));
      default:
        Debug.Assert(result == ParseFailureKind.Format_DateTimeOnlyContainsNoneDateParts);
        throw new FormatException(SR.Format(SR.Format_DateTimeOnlyContainsNoneDateParts, s.ToString(), nameof(DateOnly)));
    }
  }

  //    public string ToLongDateString() => ToString("D");
  public string ToShortDateString() => ToString();
  //public override string ToString() => DateTimeFormat.Format(GetEquivalentDateTime(), "d", null);
  //public string ToString([StringSyntax(StringSyntaxAttribute.DateOnlyFormat)] string? format) => ToString(format, null);
  //public string ToString(IFormatProvider? provider) => DateTimeFormat.Format(GetEquivalentDateTime(), "d", provider);
  //public string ToString([StringSyntax(StringSyntaxAttribute.DateOnlyFormat)] string? format, IFormatProvider? provider) {
  //  if (string.IsNullOrEmpty(format)) {
  //    format = "d";
  //  }
  //  if (format.Length == 1) {
  //    return (format[0] | 0x20) switch {
  //      'o' => string.Create(10, this, (destination, value) => {
  //        DateTimeFormat.TryFormatDateOnlyO(value, destination, out int charsWritten);
  //        Debug.Assert(charsWritten == destination.Length);
  //      }),
  //      'r' => string.Create(16, this, (destination, value) => {
  //        DateTimeFormat.TryFormatDateOnlyR(value, destination, out int charsWritten);
  //        Debug.Assert(charsWritten == destination.Length);
  //      }),
  //      'm' or 'd' or 'y' => DateTimeFormat.Format(GetEquivalentDateTime(), format, provider),

  //      _ => throw new FormatException(SR.Format_InvalidString),
  //    };
  //  }
  //  DateTimeFormat.IsValidCustomDateOnlyFormat(format.AsSpan(), throwOnError: true);
  //  return DateTimeFormat.Format(GetEquivalentDateTime(), format, provider);
  //}

  //    public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.DateOnlyFormat)] ReadOnlySpan<char> format = default, IFormatProvider? provider = null) => TryFormatCore(destination, out charsWritten, format, provider);
  //    /// <inheritdoc cref="IUtf8SpanFormattable.TryFormat" />
  //    public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax(StringSyntaxAttribute.DateOnlyFormat)] ReadOnlySpan<char> format = default, IFormatProvider? provider = null) => TryFormatCore(utf8Destination, out bytesWritten, format, provider);

  //private bool TryFormatCore<TChar>(Span<TChar> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.DateOnlyFormat)] ReadOnlySpan<char> format, IFormatProvider? provider = null) where TChar : unmanaged, IUtfChar<TChar> {
  //  if (format.Length == 0) {
  //    format = "d";
  //  }
  //  if (format.Length == 1) {
  //    switch (format[0] | 0x20) {
  //      case 'o': return DateTimeFormat.TryFormatDateOnlyO(this, destination, out charsWritten);
  //      case 'r': return DateTimeFormat.TryFormatDateOnlyR(this, destination, out charsWritten);
  //      case 'm':
  //      case 'd':
  //      case 'y':
  //        return DateTimeFormat.TryFormat(GetEquivalentDateTime(), destination, out charsWritten, format, provider);
  //      default:
  //        ThrowHelper.ThrowFormatException_BadFormatSpecifier();
  //        break;
  //    }
  //  }
  //  if (!DateTimeFormat.IsValidCustomDateOnlyFormat(format, throwOnError: false)) {
  //    throw new FormatException(SR.Format(SR.Format_DateTimeOnlyContainsNoneDateParts, format.ToString(), nameof(DateOnly)));
  //  }
  //  return DateTimeFormat.TryFormat(GetEquivalentDateTime(), destination, out charsWritten, format, provider);
  //}

  // IParsable
  ///// <inheritdoc cref="IParsable{TSelf}.Parse(string, IFormatProvider?)" />
  //public static DateOnly Parse(string s, IFormatProvider? provider) => Parse(s, provider, DateTimeStyles.None);

  /// <inheritdoc cref="IParsable{TSelf}.TryParse(string?, IFormatProvider?, out TSelf)" />
  //public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out DateOnly result) => TryParse(s, provider, DateTimeStyles.None, out result);

  // ISpanParsable

  ///// <inheritdoc cref="ISpanParsable{TSelf}.Parse(ReadOnlySpan{char}, IFormatProvider?)" />
  //public static DateOnly Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => Parse(s, provider, DateTimeStyles.None);

  ///// <inheritdoc cref="ISpanParsable{TSelf}.TryParse(ReadOnlySpan{char}, IFormatProvider?, out TSelf)" />
  //public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out DateOnly result) => TryParse(s, provider, DateTimeStyles.None, out result);
}
