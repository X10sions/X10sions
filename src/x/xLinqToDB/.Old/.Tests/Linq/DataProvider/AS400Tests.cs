using IBM.Data.DB2.iSeries;
using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.DataProvider.AS400;
using LinqToDB.DataProvider.DB2iSeries;
using LinqToDB.Mapping;
using LinqToDB.Tests.Base;
using LinqToDB.Tests.Base.DataProvider;
using LinqToDB.Tests.Model;
using LinqToDB.Tools.Comparers;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.Common;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LinqToDB.Tests.Linq.DataProvider {
  public class AS400DataSource<T> : AS400DataSource where T : IDB2iSeriesConnectionProviderType, IDB2iSeriesDataProviderOptions {
    public AS400DataSource(T dataProvider) : base(dataProvider, dataProvider.ConnectionProviderType, dataProvider.DataProviderOptions) {
    }
  }


  public class AS400DataSource : DataSource {
    public AS400DataSource(IDataProvider dataProvider, DB2iSeriesConnectionProviderType connectionProviderType, DB2iSeriesDataProviderOptions dataProviderOptions)
      : base(dataProvider, connectionProviderType.GetIsOdbcAS400(), GetConnectionStringAS400(connectionProviderType, dataProviderOptions.NamingConvention)) {
      DummyTable = $"SYSIBM{dataProviderOptions.NamingConvention.Separator()}SYSDUMMY1";
    }

    public string DummyTable { get; }

    //public static DataConnection Odbc = new DataConnection(new AS400DataProvider_Odbc(sqlOptions));

    public static T AS400AppSetting<T>(string key, T defaultValue) => AppSettings.RemoteConfiguration.GetValue($"DataSources:AS400:{key}", defaultValue);
    public static string Schema => AS400AppSetting(nameof(Schema), "LINQ2DB");
    public static string UserId => AS400AppSetting(nameof(UserId), "-UID-");
    public static string Password => AS400AppSetting(nameof(Password), "-PWD-");
    public static string System => AS400AppSetting(nameof(System), "-SYS-");

    public static string GetConnectionStringAS400(DB2iSeriesConnectionProviderType connectionProviderType, DB2iSeriesNamingConvention naming) => connectionProviderType.GetConnectionStringAS400(naming, System, UserId, Password, Schema);

  }

  //public class AS400DataSourceTests {
  //  public AS400DataSourceTests(ITestOutputHelper output) {
  //    this.output = output;
  //  }
  //  private readonly ITestOutputHelper output;
  //}


  //[TestFixture]
  public class AS400Tests : DataProviderTestBase {
    public AS400Tests(ITestOutputHelper output) {
      this.output = output;
      output.WriteLine($"{nameof(AS400DataSource.System)}: {AS400DataSource.System}");
      output.WriteLine($"{nameof(AS400DataSource.UserId)}: {AS400DataSource.UserId}");
      //output.WriteLine($"{nameof(AS400DataSource.Password)}: {AS400DataSource.Password}");
    }
    private readonly ITestOutputHelper output;

    protected override string? PassNullSql(DataConnection dc, out int paramCount) {
      var isOdbc = dc.DataProvider.Name.Contains("odbc", StringComparison.OrdinalIgnoreCase);
      paramCount = isOdbc ? 3 : 1;
      return isOdbc ? "SELECT ID FROM {1} WHERE ? IS NULL AND {0} IS NULL OR ? IS NOT NULL AND {0} = ?" : base.PassNullSql(dc, out paramCount);
    }
    protected override string PassValueSql(DataConnection dc) => dc.DataProvider.Name.Contains("odbc", StringComparison.OrdinalIgnoreCase) ? "SELECT ID FROM {1} WHERE {0} = ?" : base.PassValueSql(dc);

    static DB2iSeriesDataProviderOptions sqlOptions = new DB2iSeriesDataProviderOptions { NamingConvention = DB2iSeriesNamingConvention.Sql };
    static DB2iSeriesDataProviderOptions systemOptions = new DB2iSeriesDataProviderOptions { NamingConvention = DB2iSeriesNamingConvention.System };

    public static TheoryData<AS400DataSource> AS400DataSourceTheoryData() => new TheoryData<AS400DataSource> {
      new AS400DataSource<AS400DataProvider_Odbc>(new AS400DataProvider_Odbc(sqlOptions) ),
      new AS400DataSource<AS400DataProvider_Odbc>(new AS400DataProvider_Odbc(systemOptions  )),

      new AS400DataSource<DB2iSeriesDataProvider_Odbc>( new DB2iSeriesDataProvider_Odbc(sqlOptions)),
      new AS400DataSource<DB2iSeriesDataProvider_Odbc>(new DB2iSeriesDataProvider_Odbc(systemOptions)),

      new AS400DataSource<DB2iSeriesDataProvider_NetStandard>( new DB2iSeriesDataProvider_NetStandard(sqlOptions)),
      new AS400DataSource<DB2iSeriesDataProvider_NetStandard>(new  DB2iSeriesDataProvider_NetStandard(systemOptions))
    };

    [Fact] public void TestUserId() => Assert.NotEqual("-UID-", AS400DataSource.UserId);
    [Fact] public void TestPassword() => Assert.NotEqual("-PWD-", AS400DataSource.Password);
    [Fact] public void TestSystem() => Assert.NotEqual("-SYS-", AS400DataSource.System);

    [Theory, MemberData(nameof(AS400DataSourceTheoryData))]
    public void TestConnectionString(AS400DataSource dataSource) => Assert.True(dataSource.ConnectionString != null, dataSource.ConnectionString);

    [Theory, MemberData(nameof(AS400DataSourceTheoryData))]
    public void TestParameters(AS400DataSource ds) {
      using (var conn = ds.GetDataConnection(output)) {
        var dsInfo = ((DbConnection)conn.Connection).GetSchemaHelper().DataSourceInformationRow();
        var p = dsInfo.ParameterName("p");
        var p1 = dsInfo.ParameterName("p1");
        var p2 = dsInfo.ParameterName("p2");

        Assert.Equal("1", conn.Execute<string>($"SELECT Cast({p} as int)  FROM {ds.DummyTable}", new { p = 1 }));
        Assert.Equal("1", conn.Execute<string>($"SELECT Cast({p} as char) FROM {ds.DummyTable}", new { p = "1" }));
        Assert.Equal(1, conn.Execute<int>($"SELECT Cast({p} as int)  FROM {ds.DummyTable}", new { p = new DataParameter { Value = 1 } }));
        Assert.Equal("1", conn.Execute<string>($"SELECT Cast({p1} as char) FROM {ds.DummyTable}", new { p1 = new DataParameter { Value = "1" } }));
        Assert.Equal(5, conn.Execute<int>($"SELECT Cast({p1} as int) + Cast({p2} as int) FROM {ds.DummyTable}", new { p1 = 2, p2 = 3 }));
        Assert.Equal(5, conn.Execute<int>($"SELECT Cast({p2} as int) + Cast({p1} as int) FROM {ds.DummyTable}", new { p2 = 2, p1 = 3 }));
      }
    }

    [Theory, MemberData(nameof(AS400DataSourceTheoryData))]
    public void TestDataTypes(AS400DataSource ds) {
      using (var conn = ds.GetDataConnection(output)) {
        Assert.Equal(1000000L, TestType<long?>(conn, "bigintDataType", DataType.Int64, "ALLTYPES"));
        Assert.Equal(new iDB2BigInt(1000000L), TestType<iDB2BigInt?>(conn, "bigintDataType", DataType.Int64, "ALLTYPES"));
        Assert.Equal(7777777, TestType<int?>(conn, "intDataType", DataType.Int32, "ALLTYPES"));
        Assert.Equal(new iDB2Integer(7777777), TestType<iDB2Integer?>(conn, "intDataType", DataType.Int32, "ALLTYPES"));
        Assert.Equal((short)100, TestType<short?>(conn, "smallintDataType", DataType.Int16, "ALLTYPES"));
        Assert.Equal(new iDB2SmallInt(100), TestType<iDB2SmallInt?>(conn, "smallintDataType", DataType.Int16, "ALLTYPES"));
        Assert.Equal(9999999m, TestType<decimal?>(conn, "decimalDataType", DataType.Decimal, "ALLTYPES"));
        Assert.Equal(8888888m, TestType<decimal?>(conn, "decfloatDataType", DataType.Decimal, "ALLTYPES"));
        Assert.Equal(20.31f, TestType<float?>(conn, "realDataType", DataType.Single, "ALLTYPES"));
        Assert.Equal(new iDB2Real(20.31f), TestType<iDB2Real?>(conn, "realDataType", DataType.Single, "ALLTYPES"));
        Assert.Equal(16.2d, TestType<double?>(conn, "doubleDataType", DataType.Double, "ALLTYPES"));
        Assert.Equal(new iDB2Double(16.2d), TestType<iDB2Double?>(conn, "doubleDataType", DataType.Double, "ALLTYPES"));

        Assert.Equal("1", TestType<string>(conn, "charDataType", DataType.Char, "ALLTYPES"));
        Assert.Equal("1", TestType<string>(conn, "charDataType", DataType.NChar, "ALLTYPES"));
        Assert.Equal(new iDB2Char("1"), TestType<iDB2Char?>(conn, "charDataType", DataType.Char, "ALLTYPES"));
        Assert.Equal("234", TestType<string>(conn, "varcharDataType", DataType.VarChar, "ALLTYPES"));
        Assert.Equal("234", TestType<string>(conn, "varcharDataType", DataType.NVarChar, "ALLTYPES"));
        Assert.Equal("55645", TestType<string>(conn, "clobDataType", DataType.Text, "ALLTYPES"));
        Assert.Equal("6687", TestType<string>(conn, "dbclobDataType", DataType.NText, "ALLTYPES"));

        Assert.Equal(new byte[] { 49, 50, 51, 32, 32 }, TestType<byte[]>(conn, "binaryDataType", DataType.Binary, "ALLTYPES"));
        Assert.Equal(new byte[] { 49, 50, 51, 52 }, TestType<byte[]>(conn, "varbinaryDataType", DataType.VarBinary, "ALLTYPES"));
        Assert.Equal(new byte[] { 50, 51, 52 }, TestType<byte[]>(conn, "blobDataType", DataType.Blob, "ALLTYPES", skipDefaultNull: true, skipUndefinedNull: true, skipDefault: true, skipUndefined: true));
        Assert.Equal(new byte[] { 50, 51, 52 }, TestType<byte[]>(conn, "blobDataType", DataType.VarBinary, "ALLTYPES", skipDefaultNull: true, skipUndefinedNull: true, skipDefault: true, skipUndefined: true));
        Assert.Equal("23        ", TestType<string>(conn, "graphicDataType", DataType.VarChar, "ALLTYPES"));

        Assert.Equal(new DateTime(2012, 12, 12), TestType<DateTime?>(conn, "dateDataType", DataType.Date, "ALLTYPES"));
        Assert.Equal(new iDB2Date(new DateTime(2012, 12, 12)), TestType<iDB2Date?>(conn, "dateDataType", DataType.Date, "ALLTYPES"));
        Assert.Equal(new TimeSpan(12, 12, 12), TestType<TimeSpan?>(conn, "timeDataType", DataType.Time, "ALLTYPES"));
        Assert.Equal(new iDB2Time(12, 12, 12), TestType<iDB2Time?>(conn, "timeDataType", DataType.Time, "ALLTYPES"));
        Assert.Equal(new DateTime(2012, 12, 12, 12, 12, 12, 12), TestType<DateTime?>(conn, "timestampDataType", DataType.DateTime2, "ALLTYPES"));
        Assert.Equal(new iDB2TimeStamp(new DateTime(2012, 12, 12, 12, 12, 12, 12)), TestType<iDB2TimeStamp?>(conn, "timestampDataType", DataType.DateTime2, "ALLTYPES"));

        Assert.Equal("<root><element strattr=\"strvalue\" intattr=\"12345\"/></root>", TestType<string>(conn, "xmlDataType", DataType.Xml, "ALLTYPES", skipPass: true));

        Assert.NotEmpty(conn.Execute<byte[]>($"SELECT rowid FROM AllTypes WHERE ID = 2"));
        Assert.NotEmpty(conn.Execute<iDB2Rowid>($"SELECT rowid FROM AllTypes WHERE ID = 2").Value);

        TestType<iDB2Clob>(conn, "clobDataType", DataType.Text, "ALLTYPES", skipNotNull: true);
        TestType<iDB2Blob>(conn, "blobDataType", DataType.VarBinary, "ALLTYPES", skipNotNull: true);
        TestType<iDB2Xml>(conn, "xmlDataType", DataType.Xml, "ALLTYPES", skipPass: true);

        Assert.Equal(new iDB2Decimal(9999999m).ToString(), TestType<iDB2Decimal?>(conn, "decimalDataType", DataType.Decimal, "ALLTYPES").ToString());
        Assert.Equal(new iDB2Binary(new byte[] { 49, 50, 51, 52 }).ToString(), TestType<iDB2Binary>(conn, "varbinaryDataType", DataType.VarBinary, "ALLTYPES").ToString());
        Assert.Equal(new iDB2DecFloat16(8888888m).ToString(), TestType<iDB2DecFloat16?>(conn, "decfloatDataType", DataType.Decimal, "ALLTYPES").ToString());
        Assert.Equal(new iDB2DecFloat34(8888888m).ToString(), TestType<iDB2DecFloat34?>(conn, "decfloatDataType", DataType.Decimal, "ALLTYPES").ToString());
      }
    }

    [Theory, MemberData(nameof(AS400DataSourceTheoryData))]
    public void TestNumerics(AS400DataSource ds) {
      var isOdbc = ds.IsOdbc;
      using (var conn = ds.GetDataConnection(output)) {
        conn.TestSimple_AS400<sbyte>(1, DataType.SByte, isOdbc);
        conn.TestSimple_AS400<short>(1, DataType.Int16, isOdbc);
        conn.TestSimple_AS400<int>(1, DataType.Int32, isOdbc);
        conn.TestSimple_AS400<long>(1L, DataType.Int64, isOdbc);
        conn.TestSimple_AS400<byte>(1, DataType.Byte, isOdbc);
        conn.TestSimple_AS400<ushort>(1, DataType.UInt16, isOdbc);
        conn.TestSimple_AS400<uint>(1u, DataType.UInt32, isOdbc);
        conn.TestSimple_AS400<ulong>(1ul, DataType.UInt64, isOdbc);
        conn.TestSimple_AS400<float>(1, DataType.Single, isOdbc);
        conn.TestSimple_AS400<double>(1d, DataType.Double, isOdbc);
        conn.TestSimple_AS400<decimal>(1m, DataType.Decimal, isOdbc);
        conn.TestSimple_AS400<decimal>(1m, DataType.VarNumeric, isOdbc);
        conn.TestSimple_AS400<decimal>(1m, DataType.Money, isOdbc);
        conn.TestSimple_AS400<decimal>(1m, DataType.SmallMoney, isOdbc);

        conn.TestNumeric_AS400(sbyte.MinValue, DataType.SByte, "", isOdbc);
        conn.TestNumeric_AS400(sbyte.MaxValue, DataType.SByte, "", isOdbc);
        conn.TestNumeric_AS400(short.MinValue, DataType.Int16, "", isOdbc);
        conn.TestNumeric_AS400(short.MaxValue, DataType.Int16, "", isOdbc);
        conn.TestNumeric_AS400(int.MinValue, DataType.Int32, "smallint", isOdbc);
        conn.TestNumeric_AS400(int.MaxValue, DataType.Int32, "smallint real", isOdbc);
        conn.TestNumeric_AS400(long.MinValue, DataType.Int64, "smallint int double", isOdbc);
        conn.TestNumeric_AS400(long.MaxValue, DataType.Int64, "smallint int double real", isOdbc);

        conn.TestNumeric_AS400(byte.MaxValue, DataType.Byte, "", isOdbc);
        conn.TestNumeric_AS400(ushort.MaxValue, DataType.UInt16, "smallint", isOdbc);
        conn.TestNumeric_AS400(uint.MaxValue, DataType.UInt32, "smallint int real", isOdbc);
        conn.TestNumeric_AS400(ulong.MaxValue, DataType.UInt64, "smallint int real bigint double", isOdbc);

        conn.TestNumeric_AS400(-3.40282306E+38f, DataType.Single, "bigint int smallint decimal(31) decfloat", isOdbc);
        conn.TestNumeric_AS400(3.40282306E+38f, DataType.Single, "bigint int smallint decimal(31) decfloat", isOdbc);
        conn.TestNumeric_AS400(-1.79E+308d, DataType.Double, "bigint int smallint decimal(31) decfloat real", isOdbc);
        conn.TestNumeric_AS400(1.79E+308d, DataType.Double, "bigint int smallint decimal(31) decfloat real", isOdbc);
        conn.TestNumeric_AS400(decimal.MinValue, DataType.Decimal, "bigint int smallint double real", isOdbc);
        conn.TestNumeric_AS400(decimal.MaxValue, DataType.Decimal, "bigint int smallint double real", isOdbc);
        conn.TestNumeric_AS400(decimal.MinValue, DataType.VarNumeric, "bigint int smallint double real", isOdbc);
        conn.TestNumeric_AS400(decimal.MaxValue, DataType.VarNumeric, "bigint int smallint double real", isOdbc);
        conn.TestNumeric_AS400(-922337203685477m, DataType.Money, "int smallint real", isOdbc);
        conn.TestNumeric_AS400(+922337203685477m, DataType.Money, "int smallint real", isOdbc);
        conn.TestNumeric_AS400(-214748m, DataType.SmallMoney, "smallint", isOdbc);
        conn.TestNumeric_AS400(+214748m, DataType.SmallMoney, "smallint", isOdbc);
      }
    }

    [Theory, MemberData(nameof(AS400DataSourceTheoryData))]
    public void TestDate(AS400DataSource ds) {
      using (var conn = ds.GetDataConnection(output)) {
        var dateTime = new DateTime(2012, 12, 12);
        var p = ds.IsOdbc ? "?" : "@p";
        Assert.Equal(dateTime, conn.Execute<DateTime>($"SELECT Cast('2012-12-12' as date) FROM {ds.DummyTable}"));
        Assert.Equal(dateTime, conn.Execute<DateTime?>($"SELECT Cast('2012-12-12' as date) FROM {ds.DummyTable}"));
        Assert.Equal(dateTime, conn.Execute<DateTime>($"SELECT Cast({p} as date) FROM {ds.DummyTable}", DataParameter.Date("p", dateTime)));
        Assert.Equal(dateTime, conn.Execute<DateTime?>($"SELECT Cast({p} as date) FROM {ds.DummyTable}", new DataParameter("p", dateTime, DataType.Date)));
      }
    }

    [Theory, MemberData(nameof(AS400DataSourceTheoryData))]
    public void TestDateTime(AS400DataSource ds) {
      using (var conn = ds.GetDataConnection(output)) {
        var dateTime = new DateTime(2012, 12, 12, 12, 12, 12);

        Assert.Equal(dateTime, conn.Execute<DateTime>($"SELECT Cast('2012-12-12 12:12:12' as timestamp) FROM {ds.DummyTable}"));
        Assert.Equal(dateTime, conn.Execute<DateTime?>($"SELECT Cast('2012-12-12 12:12:12' as timestamp) FROM {ds.DummyTable}"));

        var p = ds.IsOdbc ? "?" : "@p";

        Assert.Equal(dateTime, conn.Execute<DateTime>($"SELECT Cast({p} as timestamp) FROM {ds.DummyTable}", DataParameter.DateTime("p", dateTime)));
        Assert.Equal(dateTime, conn.Execute<DateTime?>($"SELECT Cast({p} as timestamp) FROM {ds.DummyTable}", new DataParameter("p", dateTime)));
        Assert.Equal(dateTime, conn.Execute<DateTime?>($"SELECT Cast({p} as timestamp) FROM {ds.DummyTable}", new DataParameter("p", dateTime, DataType.DateTime)));
      }
    }

    [Theory, MemberData(nameof(AS400DataSourceTheoryData))]
    public void TestTimeSpan(AS400DataSource ds) {
      using (var conn = ds.GetDataConnection(output)) {
        var time = new TimeSpan(12, 12, 12);
        var p = ds.IsOdbc ? "?" : "@p";

        Assert.Equal(time, conn.Execute<TimeSpan>($"SELECT Cast('12:12:12' as time) FROM {ds.DummyTable}"));
        Assert.Equal(time, conn.Execute<TimeSpan?>($"SELECT Cast('12:12:12' as time) FROM {ds.DummyTable}"));

        Assert.Equal(time, conn.Execute<TimeSpan>($"SELECT Cast({p} as time) FROM {ds.DummyTable}", DataParameter.Time("p", time)));
        Assert.Equal(time, conn.Execute<TimeSpan>($"SELECT Cast({p} as time) FROM {ds.DummyTable}", DataParameter.Create("p", time)));
        Assert.Equal(time, conn.Execute<TimeSpan?>($"SELECT Cast({p} as time) FROM {ds.DummyTable}", new DataParameter("p", time, DataType.Time)));
        Assert.Equal(time, conn.Execute<TimeSpan?>($"SELECT Cast({p} as time) FROM {ds.DummyTable}", new DataParameter("p", time)));
      }
    }

    [Theory, MemberData(nameof(AS400DataSourceTheoryData))]
    public void TestChar(AS400DataSource ds) {
      using (var conn = ds.GetDataConnection(output)) {
        Assert.Equal('1', conn.Execute<char>($"SELECT Cast('1' as char) FROM {ds.DummyTable}"));
        Assert.Equal('1', conn.Execute<char?>($"SELECT Cast('1' as char) FROM {ds.DummyTable}"));
        Assert.Equal('1', conn.Execute<char>($"SELECT Cast('1' as char(1)) FROM {ds.DummyTable}"));
        Assert.Equal('1', conn.Execute<char?>($"SELECT Cast('1' as char(1)) FROM {ds.DummyTable}"));

        Assert.Equal('1', conn.Execute<char>($"SELECT Cast('1' as varchar(1)) FROM {ds.DummyTable}"));
        Assert.Equal('1', conn.Execute<char?>($"SELECT Cast('1' as varchar(1)) FROM {ds.DummyTable}"));
        Assert.Equal('1', conn.Execute<char>($"SELECT Cast('1' as varchar(20)) FROM {ds.DummyTable}"));
        Assert.Equal('1', conn.Execute<char?>($"SELECT Cast('1' as varchar(20)) FROM {ds.DummyTable}"));

        var p = ds.IsOdbc ? "?" : "@p";
        // [IBM][DB2/LINUXX8664] SQL0418N  The statement was not processed because the statement contains an invalid use of one of the following: an untyped parameter marker, the DEFAULT keyword, or a null value.
        //Assert.Equal('1', conn.Execute<char> ($"SELECT {p} FROM {ds.DummyTable}",                  DataParameter.Char("p", '1')));
        //Assert.Equal('1', conn.Execute<char?>($"SELECT {p} FROM {ds.DummyTable}",                  DataParameter.Char("p", '1')));
        Assert.Equal('1', conn.Execute<char>($"SELECT Cast({p} as char) FROM {ds.DummyTable}", DataParameter.Char("p", '1')));
        Assert.Equal('1', conn.Execute<char?>($"SELECT Cast({p} as char) FROM {ds.DummyTable}", DataParameter.Char("p", '1')));
        Assert.Equal('1', conn.Execute<char>($"SELECT Cast({p} as char(1)) FROM {ds.DummyTable}", DataParameter.Char("p", '1')));
        Assert.Equal('1', conn.Execute<char?>($"SELECT Cast({p} as char(1)) FROM {ds.DummyTable}", DataParameter.Char("p", '1')));

        Assert.Equal('1', conn.Execute<char>($"SELECT Cast({p} as varchar) FROM {ds.DummyTable}", DataParameter.VarChar("p", '1')));
        Assert.Equal('1', conn.Execute<char?>($"SELECT Cast({p} as varchar) FROM {ds.DummyTable}", DataParameter.VarChar("p", '1')));
        Assert.Equal('1', conn.Execute<char>($"SELECT Cast({p} as nchar) FROM {ds.DummyTable}", DataParameter.NChar("p", '1')));
        Assert.Equal('1', conn.Execute<char?>($"SELECT Cast({p} as nchar) FROM {ds.DummyTable}", DataParameter.NChar("p", '1')));
        Assert.Equal('1', conn.Execute<char>($"SELECT Cast({p} as nvarchar) FROM {ds.DummyTable}", DataParameter.NVarChar("p", '1')));
        Assert.Equal('1', conn.Execute<char?>($"SELECT Cast({p} as nvarchar) FROM {ds.DummyTable}", DataParameter.NVarChar("p", '1')));
        Assert.Equal('1', conn.Execute<char>($"SELECT Cast({p} as char) FROM {ds.DummyTable}", DataParameter.Create("p", '1')));
        Assert.Equal('1', conn.Execute<char?>($"SELECT Cast({p} as char) FROM {ds.DummyTable}", DataParameter.Create("p", '1')));

        Assert.Equal('1', conn.Execute<char>($"SELECT Cast({p} as char) FROM {ds.DummyTable}", new DataParameter { Name = "p", Value = '1' }));
        Assert.Equal('1', conn.Execute<char?>($"SELECT Cast({p} as char) FROM {ds.DummyTable}", new DataParameter { Name = "p", Value = '1' }));
      }
    }

    [Theory, MemberData(nameof(AS400DataSourceTheoryData))]
    public void TestString(AS400DataSource ds) {
      using (var conn = ds.GetDataConnection(output)) {
        Assert.Equal("12345", conn.Execute<string>($"SELECT Cast('12345' as char(5)) FROM {ds.DummyTable}"));
        Assert.Equal("12345", conn.Execute<string>($"SELECT Cast('12345' as char(20)) FROM {ds.DummyTable}"));
        Assert.Null(conn.Execute<string>($"SELECT Cast(NULL    as char(20)) FROM {ds.DummyTable}"));

        Assert.Equal("12345", conn.Execute<string>($"SELECT Cast('12345' as varchar(5)) FROM {ds.DummyTable}"));
        Assert.Equal("12345", conn.Execute<string>($"SELECT Cast('12345' as varchar(20)) FROM {ds.DummyTable}"));
        Assert.Null(conn.Execute<string>($"SELECT Cast(NULL    as varchar(20)) FROM {ds.DummyTable}"));

        Assert.Equal("12345", conn.Execute<string>($"SELECT Cast('12345' as clob) FROM {ds.DummyTable}"));
        Assert.Null(conn.Execute<string>($"SELECT Cast(NULL    as clob) FROM {ds.DummyTable}"));

        var p = ds.IsOdbc ? "?" : "@p";

        Assert.Equal("123", conn.Execute<string>($"SELECT Cast({p} as char(3))     FROM {ds.DummyTable}", DataParameter.Char("p", "123")));
        Assert.Equal("123", conn.Execute<string>($"SELECT Cast({p} as varchar(3))  FROM {ds.DummyTable}", DataParameter.VarChar("p", "123")));
        Assert.Equal("123", conn.Execute<string>($"SELECT Cast({p} as char(3))     FROM {ds.DummyTable}", DataParameter.Text("p", "123")));
        Assert.Equal("123", conn.Execute<string>($"SELECT Cast({p} as nchar(3))    FROM {ds.DummyTable}", DataParameter.NChar("p", "123")));
        Assert.Equal("123", conn.Execute<string>($"SELECT Cast({p} as nvarchar(3)) FROM {ds.DummyTable}", DataParameter.NVarChar("p", "123")));
        Assert.Equal("123", conn.Execute<string>($"SELECT Cast({p} as nchar(3))    FROM {ds.DummyTable}", DataParameter.NText("p", "123")));
        Assert.Equal("123", conn.Execute<string>($"SELECT Cast({p} as char(3))     FROM {ds.DummyTable}", DataParameter.Create("p", "123")));

        Assert.Null(conn.Execute<string>($"SELECT Cast({p} as char) FROM {ds.DummyTable}", DataParameter.Create("p", (string?)null)));
        Assert.Equal("1", conn.Execute<string>($"SELECT Cast({p} as char) FROM {ds.DummyTable}", new DataParameter { Name = "p", Value = "1" }));
      }
    }

    [Theory, MemberData(nameof(AS400DataSourceTheoryData))]
    public void TestBinary(AS400DataSource ds) {
      var arr1 = new byte[] { 49, 50 };
      var arr2 = new byte[] { 49, 50, 51, 52 };

      using (var conn = ds.GetDataConnection(output)) {
        Assert.Equal(arr1, conn.Execute<byte[]>($"SELECT Cast('12' as char(2) for bit data) FROM {ds.DummyTable}"));
        Assert.Equal(new Binary(arr2), conn.Execute<Binary>($"SELECT Cast('1234' as char(4) for bit data) FROM {ds.DummyTable}"));

        Assert.Equal(arr1, conn.Execute<byte[]>($"SELECT Cast('12' as varchar(2) for bit data) FROM {ds.DummyTable}"));
        Assert.Equal(new Binary(arr2), conn.Execute<Binary>($"SELECT Cast('1234' as varchar(4) for bit data) FROM {ds.DummyTable}"));
      }
    }

    [Theory, MemberData(nameof(AS400DataSourceTheoryData))]
    public void TestGuid(AS400DataSource ds) {
      using (var conn = ds.GetDataConnection(output)) {
        Assert.Equal(new Guid("6F9619FF-8B86-D011-B42D-00C04FC964FF"), conn.Execute<Guid>($"SELECT Cast('6F9619FF-8B86-D011-B42D-00C04FC964FF' as varchar(38))  FROM {ds.DummyTable}"));
        Assert.Equal(new Guid("6F9619FF-8B86-D011-B42D-00C04FC964FF"), conn.Execute<Guid?>($"SELECT Cast('6F9619FF-8B86-D011-B42D-00C04FC964FF' as varchar(38)) FROM {ds.DummyTable}"));
        var guid = Guid.NewGuid();
        var p = ds.IsOdbc ? "?" : "@p";

        Assert.Equal(guid, conn.Execute<Guid>($"SELECT Cast({p} as char(16) for bit data) FROM {ds.DummyTable}", DataParameter.Create("p", guid)));
        Assert.Equal(guid, conn.Execute<Guid>($"SELECT Cast({p} as char(16) for bit data) FROM {ds.DummyTable}", new DataParameter { Name = "p", Value = guid }));
      }
    }

    [Theory, MemberData(nameof(AS400DataSourceTheoryData))]
    public void TestXml(AS400DataSource ds) {
      using (var conn = ds.GetDataConnection(output)) {
        Assert.Equal("<xml/>", conn.Execute<string>($"SELECT Cast('<xml/>' as char(10)) FROM {ds.DummyTable}"));
        Assert.Equal("<xml />", conn.Execute<XDocument>($"SELECT Cast('<xml/>' as char(10)) FROM {ds.DummyTable}").ToString());
        Assert.Equal("<xml />", conn.Execute<XmlDocument>($"SELECT Cast('<xml/>' as char(10)) FROM {ds.DummyTable}").InnerXml);

        var xdoc = XDocument.Parse("<xml/>");
        var xml = Convert<string, XmlDocument>.Lambda("<xml/>");
        var p = ds.IsOdbc ? "?" : "@p";

        Assert.Equal("<xml/>", conn.Execute<string>($"SELECT Cast({p} as char(10)) FROM {ds.DummyTable}", DataParameter.Xml("p", "<xml/>")));
        Assert.Equal("<xml />", conn.Execute<XDocument>($"SELECT Cast({p} as char(10)) FROM {ds.DummyTable}", DataParameter.Xml("p", xdoc)).ToString());
        Assert.Equal("<xml />", conn.Execute<XmlDocument>($"SELECT Cast({p} as char(10)) FROM {ds.DummyTable}", DataParameter.Xml("p", xml)).InnerXml);
        Assert.Equal("<xml />", conn.Execute<XDocument>($"SELECT Cast({p} as char(10)) FROM {ds.DummyTable}", new DataParameter("p", xdoc)).ToString());
        Assert.Equal("<xml />", conn.Execute<XDocument>($"SELECT Cast({p} as char(10)) FROM {ds.DummyTable}", new DataParameter("p", xml)).ToString());
      }
    }


    [Theory, MemberData(nameof(AS400DataSourceTheoryData))]
    public void TestEnum1(AS400DataSource ds) {
      using (var conn = ds.GetDataConnection(output)) {
        Assert.Equal(TestEnum.AA, conn.Execute<TestEnum>($"SELECT 'A' FROM {ds.DummyTable}"));
        Assert.Equal(TestEnum.AA, conn.Execute<TestEnum?>($"SELECT 'A' FROM {ds.DummyTable}"));
        Assert.Equal(TestEnum.BB, conn.Execute<TestEnum>($"SELECT 'B' FROM {ds.DummyTable}"));
        Assert.Equal(TestEnum.BB, conn.Execute<TestEnum?>($"SELECT 'B' FROM {ds.DummyTable}"));
      }
    }

    [Theory, MemberData(nameof(AS400DataSourceTheoryData))]
    public void TestEnum2(AS400DataSource ds) {
      using (var conn = ds.GetDataConnection(output)) {
        var p = ds.IsOdbc ? "?" : "@p";

        Assert.Equal("A", conn.Execute<string>($"SELECT Cast({p} as char) FROM {ds.DummyTable}", new { p = TestEnum.AA }));
        Assert.Equal("B", conn.Execute<string>($"SELECT Cast({p} as char) FROM {ds.DummyTable}", new { p = (TestEnum?)TestEnum.BB }));
        Assert.Equal("A", conn.Execute<string>($"SELECT Cast({p} as char) FROM {ds.DummyTable}", new { p = ConvertTo<string>.From((TestEnum?)TestEnum.AA) }));
        Assert.Equal("A", conn.Execute<string>($"SELECT Cast({p} as char) FROM {ds.DummyTable}", new { p = ConvertTo<string>.From(TestEnum.AA) }));
        Assert.Equal("A", conn.Execute<string>($"SELECT Cast({p} as char) FROM {ds.DummyTable}", new { p = conn.MappingSchema.GetConverter<TestEnum?, string>()!(TestEnum.AA) }));
      }
    }

    void BulkCopyTest(AS400DataSource ds, BulkCopyType bulkCopyType, int maxSize, int batchSize) {
      using (var conn = ds.GetDataConnection(output)) {
        try {
          conn.BulkCopy(
            new BulkCopyOptions {
              MaxBatchSize = maxSize,
              BulkCopyType = bulkCopyType,
              NotifyAfter = 10000,
              RowsCopiedCallback = copied => Debug.WriteLine(copied.RowsCopied)
            },
            Enumerable.Range(0, batchSize).Select(n =>
              new ALLTYPE {
                ID = 2000 + n,
                BIGINTDATATYPE = 3000 + n,
                INTDATATYPE = 4000 + n,
                SMALLINTDATATYPE = (short)(5000 + n),
                DECIMALDATATYPE = 6000 + n,
                DECFLOATDATATYPE = 7000 + n,
                REALDATATYPE = 8000 + n,
                DOUBLEDATATYPE = 9000 + n,
                CHARDATATYPE = 'A',
                VARCHARDATATYPE = "",
                CLOBDATATYPE = null,
                DBCLOBDATATYPE = null,
                BINARYDATATYPE = null,
                VARBINARYDATATYPE = null,
                BLOBDATATYPE = new byte[] { 1, 2, 3 },
                GRAPHICDATATYPE = null,
                DATEDATATYPE = DateTime.Now,
                TIMEDATATYPE = null,
                TIMESTAMPDATATYPE = null,
                XMLDATATYPE = null
              }));

        } finally {
          conn.GetTable<ALLTYPE>().Delete(p => p.SMALLINTDATATYPE >= 5000);
        }
      }
    }

    async Task BulkCopyTestAsync(AS400DataSource ds, BulkCopyType bulkCopyType, int maxSize, int batchSize) {
      using (var conn = ds.GetDataConnection(output)) {
        try {
          await conn.BulkCopyAsync(
            new BulkCopyOptions {
              MaxBatchSize = maxSize,
              BulkCopyType = bulkCopyType,
              NotifyAfter = 10000,
              RowsCopiedCallback = copied => Debug.WriteLine(copied.RowsCopied)
            },
            Enumerable.Range(0, batchSize).Select(n =>
              new ALLTYPE {
                ID = 2000 + n,
                BIGINTDATATYPE = 3000 + n,
                INTDATATYPE = 4000 + n,
                SMALLINTDATATYPE = (short)(5000 + n),
                DECIMALDATATYPE = 6000 + n,
                DECFLOATDATATYPE = 7000 + n,
                REALDATATYPE = 8000 + n,
                DOUBLEDATATYPE = 9000 + n,
                CHARDATATYPE = 'A',
                VARCHARDATATYPE = "",
                CLOBDATATYPE = null,
                DBCLOBDATATYPE = null,
                BINARYDATATYPE = null,
                VARBINARYDATATYPE = null,
                BLOBDATATYPE = new byte[] { 1, 2, 3 },
                GRAPHICDATATYPE = null,
                DATEDATATYPE = DateTime.Now,
                TIMEDATATYPE = null,
                TIMESTAMPDATATYPE = null,
                XMLDATATYPE = null,
              }));
        } finally {
          await conn.GetTable<ALLTYPE>().DeleteAsync(p => p.SMALLINTDATATYPE >= 5000);
        }
      }
    }

    [Theory, MemberData(nameof(AS400DataSourceTheoryData))] public void BulkCopyMultipleRows(AS400DataSource ds) => BulkCopyTest(ds, BulkCopyType.MultipleRows, 5000, 10001);

    [Theory, MemberData(nameof(AS400DataSourceTheoryData))] public void BulkCopyProviderSpecific(AS400DataSource ds) => BulkCopyTest(ds, BulkCopyType.ProviderSpecific, 50000, 100001);

    [Theory, MemberData(nameof(AS400DataSourceTheoryData))] public async Task BulkCopyMultipleRowsAsync(AS400DataSource ds) => await BulkCopyTestAsync(ds, BulkCopyType.MultipleRows, 5000, 10001);

    [Theory, MemberData(nameof(AS400DataSourceTheoryData))] public async Task BulkCopyProviderSpecificAsync(AS400DataSource ds) => await BulkCopyTestAsync(ds, BulkCopyType.ProviderSpecific, 50000, 100001);

    [Theory, MemberData(nameof(AS400DataSourceTheoryData))]
    public void BulkCopyLinqTypes(AS400DataSource ds) {
      foreach (var bulkCopyType in new[] { BulkCopyType.MultipleRows, BulkCopyType.ProviderSpecific }) {
        using (var db = ds.GetDataConnection(output)) {
          try {
            db.BulkCopy(
              new BulkCopyOptions { BulkCopyType = bulkCopyType, },
              Enumerable.Range(0, 10).Select(n =>
                new LinqDataTypes {
                  ID = 4000 + n,
                  MoneyValue = 1000m + n,
                  DateTimeValue = new DateTime(2001, 1, 11, 1, 11, 21, 100),
                  BoolValue = true,
                  GuidValue = Guid.NewGuid(),
                  SmallIntValue = (short)n
                }
              ));
          } finally {
            db.GetTable<LinqDataTypes>().Delete(p => p.ID >= 4000);
          }
        }
      }
    }

    [Theory, MemberData(nameof(AS400DataSourceTheoryData))]
    public async Task BulkCopyLinqTypesAsync(AS400DataSource ds) {
      foreach (var bulkCopyType in new[] { BulkCopyType.MultipleRows, BulkCopyType.ProviderSpecific }) {
        using (var db = ds.GetDataConnection(output)) {
          try {
            await db.BulkCopyAsync(
              new BulkCopyOptions { BulkCopyType = bulkCopyType, },
              Enumerable.Range(0, 10).Select(n =>
                new LinqDataTypes {
                  ID = 4000 + n,
                  MoneyValue = 1000m + n,
                  DateTimeValue = new DateTime(2001, 1, 11, 1, 11, 21, 100),
                  BoolValue = true,
                  GuidValue = Guid.NewGuid(),
                  SmallIntValue = (short)n
                }
              ));
          } finally {
            await db.GetTable<LinqDataTypes>().DeleteAsync(p => p.ID >= 4000);
          }
        }
      }
    }

    [Theory, MemberData(nameof(AS400DataSourceTheoryData))]
    public void TestBinarySize(AS400DataSource ds) {
      using (var conn = ds.GetDataConnection(output)) {
        try {
          var data = new byte[500000];

          for (var i = 0; i < data.Length; i++)
            data[i] = (byte)(i % byte.MaxValue);

          conn.GetTable<ALLTYPE>().Insert(() => new ALLTYPE {
            INTDATATYPE = 2000,
            BLOBDATATYPE = data,
          });
          var blob = conn.GetTable<ALLTYPE>().First(t => t.INTDATATYPE == 2000).BLOBDATATYPE;
          Assert.Equal(data, blob);
        } finally {
          conn.GetTable<ALLTYPE>().Delete(p => p.INTDATATYPE == 2000);
        }
      }
    }

    [Theory, MemberData(nameof(AS400DataSourceTheoryData))]
    public void TestClobSize(AS400DataSource ds) {
      using (var conn = ds.GetDataConnection(output)) {
        try {
          var sb = new StringBuilder();
          for (var i = 0; i < 100000; i++)
            sb.Append(((char)((i % byte.MaxValue) + 32)).ToString());
          var data = sb.ToString();
          conn.GetTable<ALLTYPE>().Insert(() => new ALLTYPE {
            INTDATATYPE = 2000,
            CLOBDATATYPE = data,
          });
          var blob = conn.GetTable<ALLTYPE>()
  .Where(t => t.INTDATATYPE == 2000)
  .Select(t => t.CLOBDATATYPE)
  .First();
          Assert.Equal(data, blob);
        } finally {
          conn.GetTable<ALLTYPE>().Delete(p => p.INTDATATYPE == 2000);
        }
      }
    }

    [Table]
    class TestTimeTypes {
      [Column] public int Id { get; set; }
      [Column(DataType = DataType.Date)] public DateTime Date1 { get; set; }
      [Column(DbType = "Date")] public DateTime Date2 { get; set; }
      [Column] public TimeSpan Time { get; set; }
      [Column(Precision = 0)] public DateTime TimeStamp0 { get; set; }
      [Column(DbType = "timestamp(1)")] public DateTime TimeStamp1 { get; set; }
      [Column(Precision = 2)] public DateTime TimeStamp2 { get; set; }
      //[Column(DbType = "timestamp(3)")]
      [Column(Precision = 3)] public DateTime TimeStamp3 { get; set; }
      [Column(Precision = 4)] public DateTime TimeStamp4 { get; set; }
      //[Column(DbType = "TimeStamp(5)")]
      [Column(Precision = 5)] public DateTime TimeStamp5 { get; set; }
      [Column(Precision = 6)] public DateTime TimeStamp6 { get; set; }
      //[Column(DbType = "timestamp(7)")]
      [Column(Precision = 7)] public DateTime TimeStamp7 { get; set; }
      [Column(Precision = 8)] public iDB2TimeStamp TimeStamp8 { get; set; }
      //[Column(DbType = "timestamp(9)")]
      [Column(Precision = 9)] public iDB2TimeStamp TimeStamp9 { get; set; }
      [Column(Precision = 10)] public iDB2TimeStamp TimeStamp10 { get; set; }
      //[Column(DbType = "timestamp(11)")]
      [Column(Precision = 11)] public iDB2TimeStamp TimeStamp11 { get; set; }
      [Column(Precision = 12)] public iDB2TimeStamp TimeStamp12 { get; set; }

      static TestTimeTypes() {
        Data = new[]
        {
          new TestTimeTypes() { Id = 1, Date1 = new DateTime(1234, 5, 6), Date2 = new DateTime(1234, 5, 7), Time = new TimeSpan(21, 2, 3) },
          new TestTimeTypes() { Id = 2, Date1 = new DateTime(6543, 2, 1), Date2 = new DateTime(1234, 5, 8), Time = new TimeSpan(23, 2, 1) }
        };

        for (var i = 1; i <= Data.Length; i++) {
          var idx = i - 1;
          Data[idx].TimeStamp0 = new DateTime(1000, 1, 10, 2, 20, 30 + i, 0);
          Data[idx].TimeStamp1 = new DateTime(1000, 1, 10, 2, 20, 30, i * 100);
          Data[idx].TimeStamp2 = new DateTime(1000, 1, 10, 2, 20, 30, i * 10);
          Data[idx].TimeStamp3 = new DateTime(1000, 1, 10, 2, 20, 30, i);
          Data[idx].TimeStamp4 = new DateTime(1000, 1, 10, 2, 20, 30, 1).AddTicks(1000 * i);
          Data[idx].TimeStamp5 = new DateTime(1000, 1, 10, 2, 20, 30, 1).AddTicks(100 * i);
          Data[idx].TimeStamp6 = new DateTime(1000, 1, 10, 2, 20, 30, 1).AddTicks(10 * i);
          Data[idx].TimeStamp7 = new DateTime(1000, 1, 10, 2, 20, 30, 1).AddTicks(1 * i);
          Data[idx].TimeStamp8 = new iDB2TimeStamp(1000, 1, 10, 2, 20, 30, 10000 * i);//, 8);
          Data[idx].TimeStamp9 = new iDB2TimeStamp(1000, 1, 10, 2, 20, 30, 1000 * i);//, 9);
          Data[idx].TimeStamp10 = new iDB2TimeStamp(1000, 1, 10, 2, 20, 30, 100 * i);//, 10);
          Data[idx].TimeStamp11 = new iDB2TimeStamp(1000, 1, 10, 2, 20, 30, 10 * i);//, 11);
          Data[idx].TimeStamp12 = new iDB2TimeStamp(1000, 1, 10, 2, 20, 30, i);//, 12);
        }
      }

      public static TestTimeTypes[] Data;

      public static Func<TestTimeTypes, TestTimeTypes, bool> Comparer = ComparerBuilder.GetEqualsFunc<TestTimeTypes>();
    }

    //[ActiveIssue(SkipForNonLinqService = true, Details = "RemoteContext miss provider-specific types mappings. Could be workarounded by explicit column mappings")]
    [Theory, MemberData(nameof(AS400DataSourceTheoryData))]
    public void TestTimespanAndTimeValues(AS400DataSource ds, bool useParameters) {
      using (var db = ds.GetDataConnection(output))
      using (var table = db.CreateLocalTable(TestTimeTypes.Data)) {
        db.InlineParameters = !useParameters;

        var record = table.Where(_ => _.Id == 1).Single();
        Assert.True(TestTimeTypes.Comparer(record, TestTimeTypes.Data[0]));

        record = table.Where(_ => _.Date1 == TestTimeTypes.Data[0].Date1).Single();
        Assert.True(TestTimeTypes.Comparer(record, TestTimeTypes.Data[0]));

        record = table.Where(_ => _.Date2 == TestTimeTypes.Data[0].Date2).Single();
        Assert.True(TestTimeTypes.Comparer(record, TestTimeTypes.Data[0]));

        record = table.Where(_ => _.Time == TestTimeTypes.Data[0].Time).Single();
        Assert.True(TestTimeTypes.Comparer(record, TestTimeTypes.Data[0]));

        record = table.Where(_ => _.TimeStamp0 == TestTimeTypes.Data[0].TimeStamp0).Single();
        Assert.True(TestTimeTypes.Comparer(record, TestTimeTypes.Data[0]));

        record = table.Where(_ => _.TimeStamp1 == TestTimeTypes.Data[0].TimeStamp1).Single();
        Assert.True(TestTimeTypes.Comparer(record, TestTimeTypes.Data[0]));

        record = table.Where(_ => _.TimeStamp2 == TestTimeTypes.Data[0].TimeStamp2).Single();
        Assert.True(TestTimeTypes.Comparer(record, TestTimeTypes.Data[0]));

        record = table.Where(_ => _.TimeStamp3 == TestTimeTypes.Data[0].TimeStamp3).Single();
        Assert.True(TestTimeTypes.Comparer(record, TestTimeTypes.Data[0]));

        record = table.Where(_ => _.TimeStamp4 == TestTimeTypes.Data[0].TimeStamp4).Single();
        Assert.True(TestTimeTypes.Comparer(record, TestTimeTypes.Data[0]));

        record = table.Where(_ => _.TimeStamp5 == TestTimeTypes.Data[0].TimeStamp5).Single();
        Assert.True(TestTimeTypes.Comparer(record, TestTimeTypes.Data[0]));

        record = table.Where(_ => _.TimeStamp6 == TestTimeTypes.Data[0].TimeStamp6).Single();
        Assert.True(TestTimeTypes.Comparer(record, TestTimeTypes.Data[0]));

        record = table.Where(_ => _.TimeStamp7 == TestTimeTypes.Data[0].TimeStamp7).Single();
        Assert.True(TestTimeTypes.Comparer(record, TestTimeTypes.Data[0]));

        record = table.Where(_ => Compare(_.TimeStamp8, TestTimeTypes.Data[0].TimeStamp8)).Single();
        Assert.True(TestTimeTypes.Comparer(record, TestTimeTypes.Data[0]));

        record = table.Where(_ => Compare(_.TimeStamp9, TestTimeTypes.Data[0].TimeStamp9)).Single();
        Assert.True(TestTimeTypes.Comparer(record, TestTimeTypes.Data[0]));

        record = table.Where(_ => Compare(_.TimeStamp10, TestTimeTypes.Data[0].TimeStamp10)).Single();
        Assert.True(TestTimeTypes.Comparer(record, TestTimeTypes.Data[0]));

        record = table.Where(_ => Compare(_.TimeStamp11, TestTimeTypes.Data[0].TimeStamp11)).Single();
        Assert.True(TestTimeTypes.Comparer(record, TestTimeTypes.Data[0]));

        record = table.Where(_ => Compare(_.TimeStamp12, TestTimeTypes.Data[0].TimeStamp12)).Single();
        Assert.True(TestTimeTypes.Comparer(record, TestTimeTypes.Data[0]));
      }
    }

    [Sql.Expression("{0} = {1}", IsPredicate = true, ServerSideOnly = true, PreferServerSide = true)]
    public static bool Compare(iDB2TimeStamp left, iDB2TimeStamp right) => throw new InvalidOperationException();

    [Table]
    class TestParametersTable {
      [Column] public int Id { get; set; }
      [Column] public string? Text { get; set; }
    }

    // https://github.com/linq2db/linq2db/issues/2091
    [Theory, MemberData(nameof(AS400DataSourceTheoryData))]
    public void TestParametersUsed(AS400DataSource ds) {
      using (var db = ds.GetDataConnection(output))
      using (var table = db.CreateLocalTable<TestParametersTable>()) {
        var newText = new TestParametersTable() { Id = 12, Text = "Hallo Welt!" };
        db.Insert(newText);

        var text = "bla";
        var query = from f in table where f.Text == text select f;
        var result = query.ToArray();

        Assert.Contains("@", db.LastQuery!);
      }
    }

  }
}