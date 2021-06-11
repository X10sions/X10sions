using IBM.Data.DB2.iSeries;
using System;

namespace xLinqToDB.DataProvider.DB2iSeries.V2_9.RoyChase{
  public static class DB2iSeriesTypes {

    public static void InitDB2i(Type connectionType) => ConnectionType = connectionType;

    // https://secure.pamtransport.com/bin/IBM.Data.DB2.iSeries.xml
    public static readonly TypeCreator<long> BigInt = new TypeCreator<long> { Type = typeof(iDB2BigInt) };
    public static readonly TypeCreator<byte[]> Binary = new TypeCreator<byte[]> { Type = typeof(iDB2Binary) };
    public static readonly DB2iSeriesTypeCreator<byte[]> Blob = new DB2iSeriesTypeCreator<byte[]> { Type = typeof(iDB2Blob) };
    public static readonly TypeCreator<string> Char = new TypeCreator<string> { Type = typeof(iDB2Char) };
    public static readonly TypeCreator<byte[]> CharBitData = new TypeCreator<byte[]> { Type = typeof(iDB2CharBitData) };
    public static readonly DB2iSeriesTypeCreator<string> Clob = new DB2iSeriesTypeCreator<string> { Type = typeof(iDB2Clob) };
    public static readonly TypeCreator<string> DataLink = new TypeCreator<string> { Type = typeof(iDB2DataLink) };
    public static readonly TypeCreator<DateTime> Date = new TypeCreator<DateTime> { Type = typeof(iDB2Date) };
    public static readonly DB2iSeriesTypeCreator<string> DbClob = new DB2iSeriesTypeCreator<string> { Type = typeof(iDB2DbClob) };
    public static readonly TypeCreator<decimal, double, long> DecFloat16 = new TypeCreator<decimal, double, long> { Type = typeof(iDB2DecFloat16) };
    public static readonly TypeCreator<decimal, double, long> DecFloat34 = new TypeCreator<decimal, double, long> { Type = typeof(iDB2DecFloat34) };
    public static readonly TypeCreator<decimal> Decimal = new TypeCreator<decimal> { Type = typeof(iDB2Decimal) };
    public static readonly TypeCreator<double> Double = new TypeCreator<double> { Type = typeof(iDB2Double) };
    public static readonly TypeCreator<string> Graphic = new TypeCreator<string> { Type = typeof(iDB2Graphic) };
    public static readonly TypeCreator<int> Integer = new TypeCreator<int> { Type = typeof(iDB2Integer) };
    public static readonly TypeCreator<decimal> Numeric = new TypeCreator<decimal> { Type = typeof(iDB2Numeric) };
    public static readonly TypeCreator<float> Real = new TypeCreator<float> { Type = typeof(iDB2Real) };
    public static readonly TypeCreator<byte[]> RowId = new TypeCreator<byte[]> { Type = typeof(iDB2Rowid) };
    public static readonly TypeCreator<short> SmallInt = new TypeCreator<short> { Type = typeof(iDB2SmallInt) };
    public static readonly TypeCreator<DateTime> Time = new TypeCreator<DateTime> { Type = typeof(iDB2Time) };
    public static readonly TypeCreator<DateTime> TimeStamp = new TypeCreator<DateTime> { Type = typeof(iDB2TimeStamp) };
    public static readonly TypeCreator<byte[]> VarBinary = new TypeCreator<byte[]> { Type = typeof(iDB2VarBinary) };
    public static readonly TypeCreator<string> VarChar = new TypeCreator<string> { Type = typeof(iDB2VarChar) };
    public static readonly TypeCreator<byte[]> VarCharBitData = new TypeCreator<byte[]> { Type = typeof(iDB2VarCharBitData) };
    public static readonly TypeCreator<string> VarGraphic = new TypeCreator<string> { Type = typeof(iDB2VarGraphic) };
    public static readonly TypeCreator<string> Xml = new TypeCreator<string> { Type = typeof(iDB2Xml) };
    public static Type ConnectionType { get; set; }
  }
}