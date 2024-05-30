using System;

namespace Common.Attributes {
  [AttributeUsage(AttributeTargets.Field)]
  public class SqlComparisonAttribute : Attribute {
    public string SqlFormat { get; set; }
    public int? MaxValues { get; set; }

    public SqlComparisonAttribute(string code, string sqlFormat) {
      Code = code;
      SqlFormat = sqlFormat;
    }

    public SqlComparisonAttribute(string code, string sqlFormat, int maxValues) : this(code, sqlFormat) {
      MaxValues = maxValues;
    }

    public string Code { get; set; }

    //public string ISeriesCode { get; set; }
    //public string ValueSeparator { get; set; } = string.Empty;

    //public SqlComparisonAttribute(string sqlFormat, string iSeriesCode) {
    //  SqlFormat = sqlFormat;
    //  ISeriesCode = iSeriesCode;
    //}

    //public SqlComparisonAttribute(string sqlFormat, string iSeriesCode, int maxValues) : this(sqlFormat, iSeriesCode) {
    //  MaxValues = maxValues;
    //      ValueSeparator = valueSeparator;
    //}

  }
}