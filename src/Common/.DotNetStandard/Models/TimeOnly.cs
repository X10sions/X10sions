namespace Common.Models;

/// <summary>
/// Represents a time of day, as would be read from a clock, within the range 00:00:00 to 23:59:59.9999999.
/// https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/TimeOnly.cs
/// </summary>
public readonly struct TimeOnly
      //      : IComparable,
      //        IComparable<TimeOnly>,
      //        IEquatable<TimeOnly>,
      //        ISpanFormattable,
      //        ISpanParsable<TimeOnly>,
      //        IUtf8SpanFormattable
      {
  // represent the number of ticks map to the time of the day. 1 ticks = 100-nanosecond in time measurements.
  private readonly ulong _ticks;
  private const long MinTimeTicks = 0;
  private const long MaxTimeTicks = TimeSpan.TicksPerDay - 1;
  public static TimeOnly MinValue => new TimeOnly((ulong)MinTimeTicks);
  public static TimeOnly MaxValue => new TimeOnly((ulong)MaxTimeTicks);

  public TimeOnly(int hour, int minute) : this(DateTimeOnlyExtensions.TimeToTicks(hour, minute, 0, 0)) { }
  public TimeOnly(int hour, int minute, int second) : this(DateTimeOnlyExtensions.TimeToTicks(hour, minute, second, 0)) { }
  public TimeOnly(int hour, int minute, int second, int millisecond) : this(DateTimeOnlyExtensions.TimeToTicks(hour, minute, second, millisecond)) { }
  public TimeOnly(int hour, int minute, int second, int millisecond, int microsecond) : this(DateTimeOnlyExtensions.TimeToTicks(hour, minute, second, millisecond, microsecond)) { }

  public TimeOnly(long ticks) {
    if ((ulong)ticks > MaxTimeTicks) {
      throw new ArgumentOutOfRangeException(nameof(ticks), SR.ArgumentOutOfRange_TimeOnlyBadTicks);
    }
    _ticks = (ulong)ticks;
  }

  internal TimeOnly(ulong ticks) => _ticks = ticks;
  public int Hour => (int)(_ticks / TimeSpan.TicksPerHour);
  public int Minute => (int)((uint)(_ticks / TimeSpan.TicksPerMinute) % (uint)TimeSpanExtensions.MinutesPerHour);
  public int Second => (int)((uint)(_ticks / TimeSpan.TicksPerSecond) % (uint)TimeSpanExtensions.SecondsPerMinute);
  public int Millisecond => (int)((uint)(_ticks / TimeSpan.TicksPerMillisecond) % (uint)TimeSpanExtensions.MillisecondsPerSecond);
  public int Microsecond => (int)(_ticks / TimeSpanExtensions.TicksPerMicrosecond % (uint)TimeSpanExtensions.MicrosecondsPerMillisecond);
  public int Nanosecond => (int)(_ticks % TimeSpanExtensions.TicksPerMicrosecond * TimeSpanExtensions.NanosecondsPerTick);
  public long Ticks => (long)_ticks;

  private TimeOnly AddTicks(long ticks) => new TimeOnly((_ticks + TimeSpan.TicksPerDay + (ulong)(ticks % TimeSpan.TicksPerDay)) % TimeSpan.TicksPerDay);

  //private TimeOnly AddTicks(long ticks, out int wrappedDays) {
  //  (long days, long newTicks) = Math.DivRem(ticks, TimeSpan.TicksPerDay);
  //  newTicks += (long)_ticks;
  //  if (newTicks < 0) {
  //    days--;
  //    newTicks += TimeSpan.TicksPerDay;
  //  } else if (newTicks >= TimeSpan.TicksPerDay) {
  //    days++;
  //    newTicks -= TimeSpan.TicksPerDay;
  //  }
  //  wrappedDays = (int)days;
  //  return new TimeOnly((ulong)newTicks);
  //}

  //    public TimeOnly Add(TimeSpan value) => AddTicks(value.Ticks);
  //    public TimeOnly Add(TimeSpan value, out int wrappedDays) => AddTicks(value.Ticks, out wrappedDays);
  //    public TimeOnly AddHours(double value) => AddTicks((long)(value * TimeSpan.TicksPerHour));
  //    public TimeOnly AddHours(double value, out int wrappedDays) => AddTicks((long)(value * TimeSpan.TicksPerHour), out wrappedDays);
  //    public TimeOnly AddMinutes(double value) => AddTicks((long)(value * TimeSpan.TicksPerMinute));
  //    public TimeOnly AddMinutes(double value, out int wrappedDays) => AddTicks((long)(value * TimeSpan.TicksPerMinute), out wrappedDays);
  //    public bool IsBetween(TimeOnly start, TimeOnly end) {
  //      ulong time = _ticks;
  //      ulong startTicks = start._ticks;
  //      ulong endTicks = end._ticks;

  //      return startTicks <= endTicks
  //          ? (time - startTicks < endTicks - startTicks)
  //          : (time - endTicks >= startTicks - endTicks);
  //    }
  //    public static bool operator ==(TimeOnly left, TimeOnly right) => left._ticks == right._ticks;
  //    public static bool operator !=(TimeOnly left, TimeOnly right) => left._ticks != right._ticks;
  //    public static bool operator >(TimeOnly left, TimeOnly right) => left._ticks > right._ticks;
  //    public static bool operator >=(TimeOnly left, TimeOnly right) => left._ticks >= right._ticks;
  //    public static bool operator <(TimeOnly left, TimeOnly right) => left._ticks < right._ticks;
  //    public static bool operator <=(TimeOnly left, TimeOnly right) => left._ticks <= right._ticks;
  //    public static TimeSpan operator -(TimeOnly t1, TimeOnly t2) {
  //      long diff = (long)(t1._ticks - t2._ticks);
  //      // If the result is negative, add 24h to make it positive again using the sign bit.
  //      return new TimeSpan(diff + ((diff >> 63) & TimeSpan.TicksPerDay));
  //    }
  //    [EditorBrowsable(EditorBrowsableState.Never)]
  //    public void Deconstruct(out int hour, out int minute) {
  //      hour = Hour;
  //      minute = Minute;
  //    }
  //    [EditorBrowsable(EditorBrowsableState.Never)]
  //    public void Deconstruct(out int hour, out int minute, out int second) {
  //      ToDateTime().GetTime(out hour, out minute, out second);
  //    }
  //    [EditorBrowsable(EditorBrowsableState.Never)]
  //    public void Deconstruct(out int hour, out int minute, out int second, out int millisecond) {
  //      ToDateTime().GetTime(out hour, out minute, out second, out millisecond);
  //    }
  //    [EditorBrowsable(EditorBrowsableState.Never)]
  //    public void Deconstruct(out int hour, out int minute, out int second, out int millisecond, out int microsecond) {
  //      (hour, minute, second, millisecond) = this;
  //      microsecond = Microsecond;
  //    }
  public static TimeOnly FromTimeSpan(TimeSpan timeSpan) => new TimeOnly(timeSpan.Ticks);
  public static TimeOnly FromDateTime(DateTime dateTime) => new TimeOnly((ulong)dateTime.TimeOfDay.Ticks);
  public TimeSpan ToTimeSpan() => new TimeSpan((long)_ticks);
  //    internal DateTime ToDateTime() => DateTime.CreateUnchecked((long)_ticks);
  //    public int CompareTo(TimeOnly value) => _ticks.CompareTo(value._ticks);
  //    public int CompareTo(object? value) {
  //      if (value == null) return 1;
  //      if (value is not TimeOnly timeOnly) {
  //        throw new ArgumentException(SR.Arg_MustBeTimeOnly);
  //      }
  //      return CompareTo(timeOnly);
  //    }
  //    public bool Equals(TimeOnly value) => _ticks == value._ticks;
  //    public override bool Equals([NotNullWhen(true)] object? value) => value is TimeOnly timeOnly && _ticks == timeOnly._ticks;
  //    public override int GetHashCode() {
  //      ulong ticks = _ticks;
  //      return unchecked((int)ticks) ^ (int)(ticks >> 32);
  //    }

  //    private const ParseFlags ParseFlagsTimeMask = ParseFlags.HaveYear | ParseFlags.HaveMonth | ParseFlags.HaveDay | ParseFlags.HaveDate | ParseFlags.TimeZoneUsed |
  //                                                  ParseFlags.TimeZoneUtc | ParseFlags.ParsedMonthName | ParseFlags.CaptureOffset | ParseFlags.UtcSortPattern;

  //    public static TimeOnly Parse(ReadOnlySpan<char> s, IFormatProvider? provider = default, DateTimeStyles style = DateTimeStyles.None) {
  //      ParseFailureKind result = TryParseInternal(s, provider, style, out TimeOnly timeOnly);
  //      if (result != ParseFailureKind.None) {
  //        ThrowOnError(result, s);
  //      }
  //      return timeOnly;
  //    }

  //    private const string OFormat = "HH':'mm':'ss'.'fffffff";
  //    private const string RFormat = "HH':'mm':'ss";

  //    public static TimeOnly ParseExact(ReadOnlySpan<char> s, [StringSyntax(StringSyntaxAttribute.TimeOnlyFormat)] ReadOnlySpan<char> format, IFormatProvider? provider = default, DateTimeStyles style = DateTimeStyles.None) {
  //      ParseFailureKind result = TryParseExactInternal(s, format, provider, style, out TimeOnly timeOnly);
  //      if (result != ParseFailureKind.None) {
  //        ThrowOnError(result, s);
  //      }
  //      return timeOnly;
  //    }

  //    public static TimeOnly ParseExact(ReadOnlySpan<char> s, [StringSyntax(StringSyntaxAttribute.TimeOnlyFormat)] string[] formats) => ParseExact(s, formats, null, DateTimeStyles.None);

  //    public static TimeOnly ParseExact(ReadOnlySpan<char> s, [StringSyntax(StringSyntaxAttribute.TimeOnlyFormat)] string[] formats, IFormatProvider? provider, DateTimeStyles style = DateTimeStyles.None) {
  //      ParseFailureKind result = TryParseExactInternal(s, formats, provider, style, out TimeOnly timeOnly);
  //      if (result != ParseFailureKind.None) {
  //        ThrowOnError(result, s);
  //      }

  //      return timeOnly;
  //    }

  //    public static TimeOnly Parse(string s) => Parse(s, null, DateTimeStyles.None);

  //    public static TimeOnly Parse(string s, IFormatProvider? provider, DateTimeStyles style = DateTimeStyles.None) {
  //      if (s == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
  //      return Parse(s.AsSpan(), provider, style);
  //    }

  //    public static TimeOnly ParseExact(string s, [StringSyntax(StringSyntaxAttribute.TimeOnlyFormat)] string format) => ParseExact(s, format, null, DateTimeStyles.None);

  //    public static TimeOnly ParseExact(string s, [StringSyntax(StringSyntaxAttribute.TimeOnlyFormat)] string format, IFormatProvider? provider, DateTimeStyles style = DateTimeStyles.None) {
  //      if (s == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
  //      if (format == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.format);
  //      return ParseExact(s.AsSpan(), format.AsSpan(), provider, style);
  //    }

  //    public static TimeOnly ParseExact(string s, [StringSyntax(StringSyntaxAttribute.TimeOnlyFormat)] string[] formats) => ParseExact(s, formats, null, DateTimeStyles.None);

  //    public static TimeOnly ParseExact(string s, [StringSyntax(StringSyntaxAttribute.TimeOnlyFormat)] string[] formats, IFormatProvider? provider, DateTimeStyles style = DateTimeStyles.None) {
  //      if (s == null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
  //      return ParseExact(s.AsSpan(), formats, provider, style);
  //    }

  //    public static bool TryParse(ReadOnlySpan<char> s, out TimeOnly result) => TryParse(s, null, DateTimeStyles.None, out result);

  //    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, DateTimeStyles style, out TimeOnly result) =>
  //                        TryParseInternal(s, provider, style, out result) == ParseFailureKind.None;
  //    private static ParseFailureKind TryParseInternal(ReadOnlySpan<char> s, IFormatProvider? provider, DateTimeStyles style, out TimeOnly result) {
  //      if ((style & ~DateTimeStyles.AllowWhiteSpaces) != 0) {
  //        result = default;
  //        return ParseFailureKind.Argument_InvalidDateStyles;
  //      }
  //      DateTimeResult dtResult = default;
  //      dtResult.Init(s);
  //      if (!DateTimeParse.TryParse(s, DateTimeFormatInfo.GetInstance(provider), style, ref dtResult)) {
  //        result = default;
  //        return ParseFailureKind.Format_BadTimeOnly;
  //      }
  //      if ((dtResult.flags & ParseFlagsTimeMask) != 0) {
  //        result = default;
  //        return ParseFailureKind.Format_DateTimeOnlyContainsNoneDateParts;
  //      }
  //      result = FromDateTime(dtResult.parsedDate);
  //      return ParseFailureKind.None;
  //    }

  //    public static bool TryParseExact(ReadOnlySpan<char> s, [StringSyntax(StringSyntaxAttribute.TimeOnlyFormat)] ReadOnlySpan<char> format, out TimeOnly result) => TryParseExact(s, format, null, DateTimeStyles.None, out result);

  //    public static bool TryParseExact(ReadOnlySpan<char> s, [StringSyntax(StringSyntaxAttribute.TimeOnlyFormat)] ReadOnlySpan<char> format, IFormatProvider? provider, DateTimeStyles style, out TimeOnly result) =>
  //                        TryParseExactInternal(s, format, provider, style, out result) == ParseFailureKind.None;

  //    private static ParseFailureKind TryParseExactInternal(ReadOnlySpan<char> s, ReadOnlySpan<char> format, IFormatProvider? provider, DateTimeStyles style, out TimeOnly result) {
  //      if ((style & ~DateTimeStyles.AllowWhiteSpaces) != 0) {
  //        result = default;
  //        return ParseFailureKind.Argument_InvalidDateStyles;
  //      }
  //      if (format.Length == 1) {
  //        switch (format[0] | 0x20) {
  //          case 'o':
  //            format = OFormat;
  //            provider = DateTimeFormat.InvariantFormatInfo;
  //            break;

  //          case 'r':
  //            format = RFormat;
  //            provider = DateTimeFormat.InvariantFormatInfo;
  //            break;
  //        }
  //      }

  //      DateTimeResult dtResult = default;
  //      dtResult.Init(s);

  //      if (!DateTimeParse.TryParseExact(s, format, DateTimeFormatInfo.GetInstance(provider), style, ref dtResult)) {
  //        result = default;
  //        return ParseFailureKind.Format_BadTimeOnly;
  //      }

  //      if ((dtResult.flags & ParseFlagsTimeMask) != 0) {
  //        result = default;
  //        return ParseFailureKind.Format_DateTimeOnlyContainsNoneDateParts;
  //      }

  //      result = FromDateTime(dtResult.parsedDate);
  //      return ParseFailureKind.None;
  //    }

  //    public static bool TryParseExact(ReadOnlySpan<char> s, [NotNullWhen(true), StringSyntax(StringSyntaxAttribute.TimeOnlyFormat)] string?[]? formats, out TimeOnly result) => TryParseExact(s, formats, null, DateTimeStyles.None, out result);
  //    public static bool TryParseExact(ReadOnlySpan<char> s, [NotNullWhen(true), StringSyntax(StringSyntaxAttribute.TimeOnlyFormat)] string?[]? formats, IFormatProvider? provider, DateTimeStyles style, out TimeOnly result) =>
  //        TryParseExactInternal(s, formats, provider, style, out result) == ParseFailureKind.None;

  //    private static ParseFailureKind TryParseExactInternal(ReadOnlySpan<char> s, string?[]? formats, IFormatProvider? provider, DateTimeStyles style, out TimeOnly result) {
  //      if ((style & ~DateTimeStyles.AllowWhiteSpaces) != 0 || formats == null) {
  //        result = default;
  //        return ParseFailureKind.Argument_InvalidDateStyles;
  //      }
  //      DateTimeFormatInfo dtfi = DateTimeFormatInfo.GetInstance(provider);
  //      for (int i = 0; i < formats.Length; i++) {
  //        DateTimeFormatInfo dtfiToUse = dtfi;
  //        string? format = formats[i];
  //        if (string.IsNullOrEmpty(format)) {
  //          result = default;
  //          return ParseFailureKind.Argument_BadFormatSpecifier;
  //        }
  //        if (format.Length == 1) {
  //          switch (format[0] | 0x20) {
  //            case 'o':
  //              format = OFormat;
  //              dtfiToUse = DateTimeFormat.InvariantFormatInfo;
  //              break;

  //            case 'r':
  //              format = RFormat;
  //              dtfiToUse = DateTimeFormat.InvariantFormatInfo;
  //              break;
  //          }
  //        }
  //        DateTimeResult dtResult = default;
  //        dtResult.Init(s);
  //        if (DateTimeParse.TryParseExact(s, format, dtfiToUse, style, ref dtResult) && ((dtResult.flags & ParseFlagsTimeMask) == 0)) {
  //          result = FromDateTime(dtResult.parsedDate);
  //          return ParseFailureKind.None;
  //        }
  //      }
  //      result = default;
  //      return ParseFailureKind.Format_BadTimeOnly;
  //    }
  //    public static bool TryParse([NotNullWhen(true)] string? s, out TimeOnly result) => TryParse(s, null, DateTimeStyles.None, out result);
  //    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, DateTimeStyles style, out TimeOnly result) {
  //      if (s == null) {
  //        result = default;
  //        return false;
  //      }
  //      return TryParse(s.AsSpan(), provider, style, out result);
  //    }
  //    public static bool TryParseExact([NotNullWhen(true)] string? s, [NotNullWhen(true), StringSyntax(StringSyntaxAttribute.TimeOnlyFormat)] string? format, out TimeOnly result) => TryParseExact(s, format, null, DateTimeStyles.None, out result);
  //    public static bool TryParseExact([NotNullWhen(true)] string? s, [NotNullWhen(true), StringSyntax(StringSyntaxAttribute.TimeOnlyFormat)] string? format, IFormatProvider? provider, DateTimeStyles style, out TimeOnly result) {
  //      if (s == null || format == null) {
  //        result = default;
  //        return false;
  //      }
  //      return TryParseExact(s.AsSpan(), format.AsSpan(), provider, style, out result);
  //    }
  //    public static bool TryParseExact([NotNullWhen(true)] string? s, [NotNullWhen(true), StringSyntax(StringSyntaxAttribute.TimeOnlyFormat)] string?[]? formats, out TimeOnly result) => TryParseExact(s, formats, null, DateTimeStyles.None, out result);
  //    public static bool TryParseExact([NotNullWhen(true)] string? s, [NotNullWhen(true), StringSyntax(StringSyntaxAttribute.TimeOnlyFormat)] string?[]? formats, IFormatProvider? provider, DateTimeStyles style, out TimeOnly result) {
  //      if (s == null) {
  //        result = default;
  //        return false;
  //      }

  //      return TryParseExact(s.AsSpan(), formats, provider, style, out result);
  //    }
  //    private static void ThrowOnError(ParseFailureKind result, ReadOnlySpan<char> s) {
  //      Debug.Assert(result != ParseFailureKind.None);
  //      switch (result) {
  //        case ParseFailureKind.Argument_InvalidDateStyles: throw new ArgumentException(SR.Argument_InvalidDateStyles, "style");
  //        case ParseFailureKind.Argument_BadFormatSpecifier: throw new FormatException(SR.Argument_BadFormatSpecifier);
  //        case ParseFailureKind.Format_BadTimeOnly: throw new FormatException(SR.Format(SR.Format_BadTimeOnly, s.ToString()));
  //        default:
  //          Debug.Assert(result == ParseFailureKind.Format_DateTimeOnlyContainsNoneDateParts);
  //          throw new FormatException(SR.Format(SR.Format_DateTimeOnlyContainsNoneDateParts, s.ToString(), nameof(TimeOnly)));
  //      }
  //    }
  //    public string ToLongTimeString() => ToString("T");
  //    public string ToShortTimeString() => ToString();
  //    public override string ToString() => DateTimeFormat.Format(ToDateTime(), "t", null);
  //    public string ToString([StringSyntax(StringSyntaxAttribute.TimeOnlyFormat)] string? format) => ToString(format, null);
  //    public string ToString(IFormatProvider? provider) => DateTimeFormat.Format(ToDateTime(), "t", provider);
  //    public string ToString([StringSyntax(StringSyntaxAttribute.TimeOnlyFormat)] string? format, IFormatProvider? provider) {
  //      if (string.IsNullOrEmpty(format)) {
  //        format = "t";
  //      }
  //      if (format.Length == 1) {
  //        return (format[0] | 0x20) switch {
  //          'o' => string.Create(16, this, (destination, value) => {
  //            DateTimeFormat.TryFormatTimeOnlyO(value, destination, out int charsWritten);
  //            Debug.Assert(charsWritten == destination.Length);
  //          }),
  //          'r' => string.Create(8, this, (destination, value) => {
  //            DateTimeFormat.TryFormatTimeOnlyR(value, destination, out int charsWritten);
  //            Debug.Assert(charsWritten == destination.Length);
  //          }),
  //          't' => DateTimeFormat.Format(ToDateTime(), format, provider),
  //          _ => throw new FormatException(SR.Format_InvalidString),
  //        };
  //      }
  //      DateTimeFormat.IsValidCustomTimeOnlyFormat(format.AsSpan(), throwOnError: true);
  //      return DateTimeFormat.Format(ToDateTime(), format, provider);
  //    }
  //    public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.TimeOnlyFormat)] ReadOnlySpan<char> format = default, IFormatProvider? provider = null) => TryFormatCore(destination, out charsWritten, format, provider);
  //    /// <inheritdoc cref="IUtf8SpanFormattable.TryFormat" />
  //    public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax(StringSyntaxAttribute.TimeOnlyFormat)] ReadOnlySpan<char> format = default, IFormatProvider? provider = null) => TryFormatCore(utf8Destination, out bytesWritten, format, provider);
  //    private bool TryFormatCore<TChar>(Span<TChar> destination, out int written, [StringSyntax(StringSyntaxAttribute.TimeOnlyFormat)] ReadOnlySpan<char> format, IFormatProvider? provider) where TChar : unmanaged, IUtfChar<TChar> {
  //      if (format.Length == 0) {
  //        format = "t";
  //      }
  //      if (format.Length == 1) {
  //        switch (format[0] | 0x20) {
  //          case 'o': return DateTimeFormat.TryFormatTimeOnlyO(this, destination, out written);
  //          case 'r': return DateTimeFormat.TryFormatTimeOnlyR(this, destination, out written);
  //          case 't': return DateTimeFormat.TryFormat(ToDateTime(), destination, out written, format, provider);
  //          default:
  //            ThrowHelper.ThrowFormatException_BadFormatSpecifier();
  //            break;
  //        }
  //      }
  //      if (!DateTimeFormat.IsValidCustomTimeOnlyFormat(format, throwOnError: false)) {
  //        throw new FormatException(SR.Format(SR.Format_DateTimeOnlyContainsNoneDateParts, format.ToString(), nameof(TimeOnly)));
  //      }
  //      return DateTimeFormat.TryFormat(ToDateTime(), destination, out written, format, provider);
  //    }
  //    // IParsable
  //    /// <inheritdoc cref="IParsable{TSelf}.Parse(string, IFormatProvider?)" />
  //    public static TimeOnly Parse(string s, IFormatProvider? provider) => Parse(s, provider, DateTimeStyles.None);

  //    /// <inheritdoc cref="IParsable{TSelf}.TryParse(string?, IFormatProvider?, out TSelf)" />
  //    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out TimeOnly result) => TryParse(s, provider, DateTimeStyles.None, out result);

  //    // ISpanParsable
  //    /// <inheritdoc cref="ISpanParsable{TSelf}.Parse(ReadOnlySpan{char}, IFormatProvider?)" />
  //    public static TimeOnly Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => Parse(s, provider, DateTimeStyles.None);
  //    /// <inheritdoc cref="ISpanParsable{TSelf}.TryParse(ReadOnlySpan{char}, IFormatProvider?, out TSelf)" />
  //    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out TimeOnly result) => TryParse(s, provider, DateTimeStyles.None, out result);
}
