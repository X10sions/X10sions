using System;

namespace Common.Structures {
  public struct IntHHMMSS : IFormattable {

    public IntHHMMSS(int hhmmss) : this() {
      HHMMSS = hhmmss;
    }

    public IntHHMMSS(DateTime d) : this() {
      Date = d;
    }

    public IntHHMMSS(int hour, int minute, int second) : this() {
      HHMMSS = hour * 10000 + minute * 100 + second;
    }

    public static readonly IntHHMMSS _Max = new IntHHMMSS(235959); // 23:59:00
    public static readonly IntHHMMSS _Min = new IntHHMMSS(0); // 00:00:00

    public const string Format = "hhmmss";

    public DateTime Date {
      get => new DateTime(1, 1, 1, Hour, Minute, Second, Millisecond);
      set {
        Hour = value.Hour;
        Minute = value.Minute;
        Second = value.Second;
        Millisecond = value.Millisecond;
      }
    }

    public int HHMMSS {
      get => (Hour * 10000) + (Minute * 100) + Second;
      set {
        Hour = value / 10000;
        Minute = (value / 100) % 100;
        Second = value % 100;
      }
    }

    public int Hour { get; set; }
    public int Minute { get; set; }
    public int Second { get; set; }
    public int Millisecond { get; set; }

    public int HHMM => Hour * 100 + Minute;

    public TimeSpan AsTimeSpan() => new TimeSpan(0, Hour, Minute, Second, Millisecond);

    public static implicit operator IntHHMMSS(decimal value) => new IntHHMMSS((int)value);
    public static implicit operator IntHHMMSS(int value) => new IntHHMMSS(value);
    public static implicit operator IntHHMMSS(DateTime value) => new IntHHMMSS(value);
    public static implicit operator IntHHMMSS(double value) => new IntHHMMSS((int)value);
    public static implicit operator IntHHMMSS(string value) => new IntHHMMSS(value.As(0));

    public static implicit operator DateTime(IntHHMMSS value) => value.Date;
    public static implicit operator decimal(IntHHMMSS value) => value.HHMMSS;
    public static implicit operator double(IntHHMMSS value) => value.HHMMSS;
    public static implicit operator int(IntHHMMSS value) => value.HHMMSS;
    public static implicit operator string(IntHHMMSS value) => value.ToString();

    #region IFormattable
    public override string ToString() => HHMMSS.ToString();
    public string ToString(string format, IFormatProvider formatProvider) => ToString(ToString(), formatProvider);
    #endregion

    //public static int ToHHMM(DateTime d) => ToHHMM(d.Hour, d.Minute);
    //public static int ToHHMM(int hour, int minute) => (hour * 100) + minute;
    //public static int ToHHMMSS(DateTime d) => ToHHMMSS(d.Hour, d.Millisecond, d.Second);
    //public static int ToHHMMSS(this DateTime d) => ToHHMMSS(d.Hour, d.Millisecond, d.Second);
    //public static int ToHHMMSS(int hour, int minute, int second) => (hour * 10000 + minute * 100 + second);
    //public static int? ToHHMMSS(this DateTime? d, int? defaultIfNull = null) => d.HasValue ? d.Value.ToHHMMSS() : defaultIfNull;


    //public string SqlValue() => Date.ToSqlTime();
    //public string SqlExpression() => Date.ToSqlTimeExpression();

  }
}
