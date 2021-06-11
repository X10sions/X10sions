namespace System {

  public class SqlBinaryOptions : SqlOptions {
    public const string DefaultLiteralPrefix = "0x";
    public override string LiteralPrefix { get; set; } = DefaultLiteralPrefix;
    public override bool UseLiteral { get; set; } = true;
  }

  public class SqlBoolean01Options : SqlBooleanOptions {
    public override string FalseValue { get; set; } = "0";
    public override string TrueValue { get; set; } = "1";
  }

  public class SqlBooleanOptions : SqlOptions {
    public virtual string FalseValue { get; set; } = "false";
    public virtual string TrueValue { get; set; } = "true";
  }

  public class SqlBooleanYNOptions : SqlBooleanOptions {
    public override string FalseValue { get; set; } = "Y";
    public override string TrueValue { get; set; } = "N";
  }

  public class SqlDateOptions : SqlDateTimeOptions {
    public virtual Formats Format => Formats.Date;
    //public string LiteralValueformat = DateTimeConstants.SqlDateFormat;
    //public string LiteralFormat => LiteralPrefix + LiteralFormat + LiteralSuffix;
  }

  public class SqlDateTimeOptions : SqlStringOptions {

    public enum Formats {
      Date,
      Time,
      Timestamp
    }

    public override string LiteralPrefix { get; set; } = DefaultLiteralPrefix;
    public override string LiteralSuffix { get; set; } = DefaultLiteralSuffix;
    public override bool UseLiteral { get; set; } = true;
    //public string LiteralValueformat = DateTimeConstants.SqlDateFormat;
    //public string LiteralFormat => LiteralPrefix + LiteralFormat + LiteralSuffix;
  }

  public class SqlDecimalOptions : SqlOptions {
  }

  public class SqlOptions {
    public const string SqlNullString = "null";

    //public string LiteralValueformat = DateTimeConstants.SqlDateFormat;
    //public string LiteralFormat => LiteralPrefix + LiteralFormat + LiteralSuffix;
    public virtual string LiteralPrefix { get; set; } = string.Empty;

    public virtual string LiteralSuffix { get; set; } = string.Empty;
    public virtual bool UseLiteral { get; set; } = false;
  }

  public class SqlStringOptions : SqlOptions {
    public const string DefaultLiteralPrefix = "'";
    public const string DefaultLiteralSuffix = "'";

    public override string LiteralPrefix { get; set; } = DefaultLiteralPrefix;
    public override string LiteralSuffix { get; set; } = DefaultLiteralSuffix;
    public override bool UseLiteral { get; set; } = true;
  }

  public class SqlTimeOptions : SqlDateOptions {

    //public string LiteralValueformat = DateTimeConstants.SqlTimeFormat;
    public int MilliSecondsPrecision;
     
    public override Formats Format => Formats.Time;
  }

  public class SqlTimestampOptions : SqlTimeOptions {
    public override Formats Format => Formats.Timestamp;
  }

}