using System;
using System.Data;

namespace Common.Structures {
  public interface IiSeriesVarChar : IiSeriesChar {
    int Allocate { get; }
  }

  public struct iSeriesVarChar : IiSeriesVarChar {

    public iSeriesVarChar(string value) : this(value.Length, value.Length, value) { }
    public iSeriesVarChar(int length) : this(length, length) { }
    public iSeriesVarChar(int length, int allocate) : this(length, allocate, null) { }
    public iSeriesVarChar(int length, int allocate, string value) : this(length, allocate, value ?? string.Empty, false) { }

    public iSeriesVarChar(int length, int allocate, string value, bool isNullable) {
      Length = length;
      Value = value;
      IsNullable = isNullable;
      Allocate = allocate > length ? allocate : length;
    }

    public static implicit operator iSeriesVarChar(string value) => new iSeriesVarChar(value);

    #region IiSeriesChar
    public string Value { get; set; }
    public int Length { get; }
    public bool IsNullable { get; }
    public DbType DbType => DbType.String;
    public string SqlQualifiedValue => DbType.GetSqlQualifiedValue(Value, true);
    #endregion

    #region IiSeriesChar
    public int Allocate { get; }
    #endregion 

    #region IFormattable
    public override string ToString() => Value;
    public string ToString(string format, IFormatProvider formatProvider) => ToString(ToString(), formatProvider);
    #endregion

    //public string SqlValue() => String_Extensions_Sql.Sql(Value).ToSqlExpression();


  }
}