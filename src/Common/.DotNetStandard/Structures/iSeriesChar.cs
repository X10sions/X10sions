using System;
using System.Data;

namespace Common.Structures {

  public interface IiSeriesChar : IFormattable {
    string Value { get; set; }
    int Length { get; }
    bool IsNullable { get; }
    DbType DbType { get; }
    string SqlQualifiedValue { get; }
  }

  public struct iSeriesChar : IiSeriesChar {

    public iSeriesChar(string value) : this(value.Length, value) { }
    public iSeriesChar(int length) : this(length, null) { }
    public iSeriesChar(int length, string value) : this(length, value ?? string.Empty, false) { }
    public iSeriesChar(int length, string value, bool isNullable) {
      Length = length;
      Value = value;
      IsNullable = isNullable;
    }

    public static implicit operator iSeriesChar(string value) => new iSeriesChar(value);

    #region IiSeriesChar
    public string Value { get; set; }
    public int Length { get; }
    public bool IsNullable { get; }
    public DbType DbType => DbType.StringFixedLength;
    public string SqlQualifiedValue => DbType.GetSqlQualifiedValue(Value, true);
    #endregion

    #region IFormattable
    public override string ToString() => Value;
    public string ToString(string format, IFormatProvider formatProvider) => ToString().ToString(formatProvider);
    #endregion

  }
}
