using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Data;
using LinqToDB.DataProvider.Access;
using LinqToDB.Tests.Base;
using LinqToDB.Tests.Base.DataProvider;
using LinqToDB.Tests.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LinqToDB.Tests.Linq.DataProvider {
  //[TestFixture]
  public class AccessTests : DataProviderTestBase {
    public AccessTests(ITestOutputHelper output) {
      this.output = output;
      output.WriteLine($"{nameof(AS400DataSource.System)}: {AS400DataSource.System}");
      output.WriteLine($"{nameof(AS400DataSource.UserId)}: {AS400DataSource.UserId}");
      //output.WriteLine($"{nameof(AS400DataSource.Password)}: {AS400DataSource.Password}");
    }
    private readonly ITestOutputHelper output;

    public static TheoryData<DataSource> DataSourceTheoryData => new TheoryData<DataSource> {
      new DataSource(new AccessODBCDataProvider(),  true , AppSettings.RemoteConfiguration.GetConnectionString(  "Access.Odbc")),
      new DataSource  (new AccessOleDbDataProvider() ,false, AppSettings.RemoteConfiguration.GetConnectionString("Access.OleDb") )
    };

    protected override string? PassNullSql(DataConnection dc, out int paramCount) {
      paramCount = dc.DataProvider.Name == ProviderName.AccessOdbc ? 3 : 1;
      return dc.DataProvider.Name == ProviderName.AccessOdbc
        ? "SELECT ID FROM {1} WHERE ? IS NULL AND {0} IS NULL OR ? IS NOT NULL AND {0} = ?"
        : base.PassNullSql(dc, out paramCount);
    }
    protected override string PassValueSql(DataConnection dc) => dc.DataProvider.Name == ProviderName.AccessOdbc ? "SELECT ID FROM {1} WHERE {0} = ?" : base.PassValueSql(dc);

    [Theory, MemberData(nameof(DataSourceTheoryData))]
    public void TestConnectionString(DataSource dataSource) => Assert.True(dataSource.ConnectionString != null, dataSource.ConnectionString);

    [Theory, MemberData(nameof(DataSourceTheoryData))]
    public void TestParameters(DataSource dataSource) {
      var param = dataSource.IsOdbc ? "?" : "@p";
      var param1 = dataSource.IsOdbc ? "?" : "@p1";
      var param2 = dataSource.IsOdbc ? "?" : "@p2";

      using (var conn = dataSource.GetDataConnection(output)) {
        Assert.Equal("1", conn.Execute<string>($"SELECT {param}", new { p = 1 }));
        Assert.Equal("1", conn.Execute<string>($"SELECT {param}", new { p = "1" }));
        Assert.Equal(1, conn.Execute<int>($"SELECT {param}", new { p = new DataParameter { Value = 1 } }));
        Assert.Equal("1", conn.Execute<string>($"SELECT {param1}", new { p1 = new DataParameter { Value = "1" } }));
        // doesn't really test ODBC parameters
        Assert.Equal(5, conn.Execute<int>($"SELECT {param1} + {param2}", new { p1 = 2, p2 = 3 }));
        Assert.Equal(5, conn.Execute<int>($"SELECT {param2} + {param1}", new { p2 = 2, p1 = 3 }));
      }
    }

    [Theory, MemberData(nameof(DataSourceTheoryData))]
    public void TestDataTypes(DataSource dataSource) {
      using (var conn = dataSource.GetDataConnection(output)) {
        var isODBC = dataSource.IsOdbc;
        Assert.True(TestType<bool>(conn, "bitDataType", DataType.Boolean, skipDefaultNull: isODBC));
        Assert.Equal((short)25555, TestType<short?>(conn, "smallintDataType", DataType.Int16, skipDefaultNull: isODBC));
        Assert.Equal(2222222m, TestType<decimal?>(conn, "decimalDataType", DataType.Decimal, skipDefaultNull: isODBC));
        Assert.Equal(7777777, TestType<int?>(conn, "intDataType", DataType.Int32, skipDefaultNull: isODBC));
        Assert.Equal((sbyte)100, TestType<sbyte?>(conn, "tinyintDataType", DataType.SByte, skipDefaultNull: isODBC));
        Assert.Equal(100000m, TestType<decimal?>(conn, "moneyDataType", DataType.Money, skipDefaultNull: isODBC));
        Assert.Equal(20.31d, TestType<double?>(conn, "floatDataType", DataType.Double, skipDefaultNull: isODBC));
        Assert.Equal(16.2f, TestType<float?>(conn, "realDataType", DataType.Single, skipDefaultNull: isODBC));

        Assert.Equal(new DateTime(2012, 12, 12, 12, 12, 12), TestType<DateTime?>(conn, "datetimeDataType", DataType.DateTime, skipDefaultNull: isODBC));

        Assert.Equal('1', TestType<char?>(conn, "charDataType", DataType.Char, skipDefaultNull: isODBC));
        Assert.Equal("234", TestType<string>(conn, "varcharDataType", DataType.VarChar, skipDefaultNull: isODBC));
        Assert.Equal("567", TestType<string>(conn, "textDataType", DataType.Text, skipDefaultNull: isODBC));
        Assert.Equal("23233", TestType<string>(conn, "ncharDataType", DataType.NChar, skipDefaultNull: isODBC));
        Assert.Equal("3323", TestType<string>(conn, "nvarcharDataType", DataType.NVarChar, skipDefaultNull: isODBC));
        Assert.Equal("111", TestType<string>(conn, "ntextDataType", DataType.NText, skipDefaultNull: isODBC));

        Assert.Equal(new byte[] { 1, 2, 3, 4, 0, 0, 0, 0, 0, 0 }, TestType<byte[]>(conn, "binaryDataType", DataType.Binary, skipDefaultNull: isODBC));
        Assert.Equal(new byte[] { 1, 2, 3, 5 }, TestType<byte[]>(conn, "varbinaryDataType", DataType.VarBinary, skipDefaultNull: isODBC));
        Assert.Equal(new byte[] { 3, 4, 5, 6 }, TestType<byte[]>(conn, "imageDataType", DataType.Image, skipDefaultNull: isODBC));
        Assert.Equal(new byte[] { 5, 6, 7, 8 }, TestType<byte[]>(conn, "oleobjectDataType", DataType.Variant, skipDefined: true, skipDefaultNull: isODBC));

        Assert.Equal(new Guid("{6F9619FF-8B86-D011-B42D-00C04FC964FF}"), TestType<Guid?>(conn, "uniqueidentifierDataType", DataType.Guid, skipDefaultNull: isODBC));
      }
    }

    [Theory, MemberData(nameof(DataSourceTheoryData))]
    public void TestNumerics(DataSource dataSource) {
      using (var conn = dataSource.GetDataConnection(output)) {
        // ODBC driver doesn't support null parameter in select
        var isODBC = dataSource.IsOdbc;
        conn.TestSimple_Access<bool>(true, DataType.Boolean, isODBC);
        conn.TestSimple_Access<sbyte>(1, DataType.SByte, isODBC);
        conn.TestSimple_Access<short>(1, DataType.Int16, isODBC);
        conn.TestSimple_Access<int>(1, DataType.Int32, isODBC);
        conn.TestSimple_Access<long>(1L, DataType.Int64, isODBC);
        conn.TestSimple_Access<byte>(1, DataType.Byte, isODBC);
        conn.TestSimple_Access<ushort>(1, DataType.UInt16, isODBC);
        conn.TestSimple_Access<uint>(1u, DataType.UInt32, isODBC);
        conn.TestSimple_Access<ulong>(1ul, DataType.UInt64, isODBC);
        conn.TestSimple_Access<float>(1, DataType.Single, isODBC);
        conn.TestSimple_Access<double>(1d, DataType.Double, isODBC);
        conn.TestSimple_Access<decimal>(1m, DataType.Decimal, isODBC);
        conn.TestSimple_Access<decimal>(1m, DataType.VarNumeric, isODBC);
        conn.TestSimple_Access<decimal>(1m, DataType.Money, isODBC);
        conn.TestSimple_Access<decimal>(1m, DataType.SmallMoney, isODBC);
        conn.TestNumeric_Access(sbyte.MinValue, DataType.SByte, "cbool cbyte", isODBC);
        conn.TestNumeric_Access(sbyte.MaxValue, DataType.SByte, "cbool", isODBC);
        conn.TestNumeric_Access(short.MinValue, DataType.Int16, "cbool cbyte", isODBC);
        conn.TestNumeric_Access(short.MaxValue, DataType.Int16, "cbool cbyte", isODBC);
        conn.TestNumeric_Access(int.MinValue, DataType.Int32, "cbool cbyte cint", isODBC);
        conn.TestNumeric_Access(int.MaxValue, DataType.Int32, "cbool cbyte cint csng", isODBC);
        if (!isODBC) {
          // TODO: it is not clear if ODBC driver doesn't support 64-numbers at all or we just need
          // ACE 16 database
          conn.TestNumeric_Access(long.MinValue, DataType.Int64, "cbool cbyte cint clng ccur", isODBC);
          conn.TestNumeric_Access(long.MaxValue, DataType.Int64, "cbool cbyte cint clng ccur cdbl csng", isODBC);
          conn.TestNumeric_Access(ulong.MaxValue, DataType.UInt64, "cbool cbyte cint clng csng ccur cdbl", isODBC);
        }
        conn.TestNumeric_Access(int.MinValue, DataType.Int64, "cbool cbyte cint clng ccur", isODBC);
        conn.TestNumeric_Access(int.MaxValue, DataType.Int64, "cbool cbyte cint clng ccur cdbl csng", isODBC);
        conn.TestNumeric_Access(uint.MaxValue, DataType.UInt64, "cbool cbyte cint clng csng ccur cdbl", isODBC);
        conn.TestNumeric_Access(byte.MaxValue, DataType.Byte, "cbool", isODBC);
        conn.TestNumeric_Access(ushort.MaxValue, DataType.UInt16, "cbool cbyte cint", isODBC);
        conn.TestNumeric_Access(uint.MaxValue, DataType.UInt32, "cbool cbyte cint clng csng", isODBC);
        conn.TestNumeric_Access(-3.40282306E+38f, DataType.Single, "cbool cbyte clng cint ccur", isODBC);
        conn.TestNumeric_Access(3.40282306E+38f, DataType.Single, "cbool cbyte clng cint ccur", isODBC);
        conn.TestNumeric_Access(-1.79E+308d, DataType.Double, "cbool cbyte clng cint ccur csng", isODBC);
        conn.TestNumeric_Access(1.79E+308d, DataType.Double, "cbool cbyte clng cint ccur csng", isODBC);
        conn.TestNumeric_Access(decimal.MinValue, DataType.Decimal, "cbool cbyte clng cint ccur cdbl csng", isODBC);
        conn.TestNumeric_Access(decimal.MaxValue, DataType.Decimal, "cbool cbyte clng cint ccur cdbl csng", isODBC);
        conn.TestNumeric_Access(1.123456789m, DataType.Decimal, "cbool cbyte clng cint ccur cdbl csng", isODBC);
        conn.TestNumeric_Access(-1.123456789m, DataType.Decimal, "cbool cbyte clng cint ccur cdbl csng", isODBC);
        conn.TestNumeric_Access(-922337203685477m, DataType.Money, "cbool cbyte clng cint csng", isODBC);
        conn.TestNumeric_Access(+922337203685477m, DataType.Money, "cbool cbyte clng cint csng", isODBC);
        conn.TestNumeric_Access(-214748m, DataType.SmallMoney, "cbool cbyte cint", isODBC);
        conn.TestNumeric_Access(+214748m, DataType.SmallMoney, "cbool cbyte cint", isODBC);
      }
    }

    [Theory, MemberData(nameof(DataSourceTheoryData))]
    public void TestDateTime(DataSource dataSource) {
      var param = dataSource.IsOdbc ? "?" : "@p";
      using (var conn = dataSource.GetDataConnection(output)) {
        var dateTime = new DateTime(2012, 12, 12, 12, 12, 12);
        Assert.Equal(dateTime, conn.Execute<DateTime>("SELECT cdate('2012-12-12 12:12:12')"));
        Assert.Equal(dateTime, conn.Execute<DateTime?>("SELECT CDate('2012-12-12 12:12:12')"));
        Assert.Equal(dateTime, conn.Execute<DateTime>($"SELECT {param}", DataParameter.DateTime("p", dateTime)));
        Assert.Equal(dateTime, conn.Execute<DateTime?>($"SELECT {param}", new DataParameter("p", dateTime)));
        Assert.Equal(dateTime, conn.Execute<DateTime?>($"SELECT {param}", new DataParameter("p", dateTime, DataType.DateTime)));
      }
    }

    [Theory, MemberData(nameof(DataSourceTheoryData))]
    public void TestChar(DataSource dataSource) {
      var param = dataSource.IsOdbc ? "?" : "@p";

      using (var conn = dataSource.GetDataConnection(output)) {
        Assert.Equal('1', conn.Execute<char>("SELECT CStr('1')"));
        Assert.Equal('1', conn.Execute<char?>("SELECT CStr('1')"));

        Assert.Equal('1', conn.Execute<char>($"SELECT {param}", DataParameter.Char("p", '1')));
        Assert.Equal('1', conn.Execute<char?>($"SELECT {param}", DataParameter.Char("p", '1')));
        Assert.Equal('1', conn.Execute<char>($"SELECT CStr({param})", DataParameter.Char("p", '1')));

        Assert.Equal('1', conn.Execute<char>($"SELECT {param}", DataParameter.VarChar("p", '1')));
        Assert.Equal('1', conn.Execute<char?>($"SELECT {param}", DataParameter.VarChar("p", '1')));
        Assert.Equal('1', conn.Execute<char>($"SELECT {param}", DataParameter.NChar("p", '1')));
        Assert.Equal('1', conn.Execute<char?>($"SELECT {param}", DataParameter.NChar("p", '1')));
        Assert.Equal('1', conn.Execute<char>($"SELECT {param}", DataParameter.NVarChar("p", '1')));
        Assert.Equal('1', conn.Execute<char?>($"SELECT {param}", DataParameter.NVarChar("p", '1')));
        Assert.Equal('1', conn.Execute<char>($"SELECT {param}", DataParameter.Create("p", '1')));
        Assert.Equal('1', conn.Execute<char?>($"SELECT {param}", DataParameter.Create("p", '1')));

        Assert.Equal('1', conn.Execute<char>($"SELECT {param}", new DataParameter { Name = "p", Value = '1' }));
        Assert.Equal('1', conn.Execute<char?>($"SELECT {param}", new DataParameter { Name = "p", Value = '1' }));
      }
    }

    [Theory, MemberData(nameof(DataSourceTheoryData))]
    public void TestString(DataSource dataSource) {
      // ODBC driver doesn't support null parameter in select
      var isODBC = dataSource.IsOdbc;
      var param = isODBC ? "?" : "@p";

      using (var conn = dataSource.GetDataConnection(output)) {
        Assert.Equal("12345", conn.Execute<string>("SELECT CStr('12345')"));
        Assert.Null(conn.Execute<string>("SELECT NULL"));

        Assert.Equal("1231", conn.Execute<string>($"SELECT {param} & 1", DataParameter.Char("p", "123")));
        Assert.Equal("123", conn.Execute<string>($"SELECT {param}", DataParameter.VarChar("p", "123")));
        Assert.Equal("123", conn.Execute<string>($"SELECT {param}", DataParameter.Text("p", "123")));
        Assert.Equal("123", conn.Execute<string>($"SELECT {param}", DataParameter.NChar("p", "123")));
        Assert.Equal("123", conn.Execute<string>($"SELECT {param}", DataParameter.NVarChar("p", "123")));
        Assert.Equal("123", conn.Execute<string>($"SELECT {param}", DataParameter.NText("p", "123")));
        Assert.Equal("123", conn.Execute<string>($"SELECT {param}", DataParameter.Create("p", "123")));

        if (isODBC) // ODBC provider doesn't return type for NULL value
          Assert.Null(conn.Execute<string>($"SELECT CVar({param})", DataParameter.Create("p", (string?)null)));
        else
          Assert.Null(conn.Execute<string>($"SELECT {param}", DataParameter.Create("p", (string?)null)));

        Assert.Equal("1", conn.Execute<string>($"SELECT {param}", new DataParameter { Name = "p", Value = "1" }));
      }
    }

    [Theory, MemberData(nameof(DataSourceTheoryData))]
    public void TestBinary(DataSource dataSource) {
      var isODBC = dataSource.IsOdbc;
      var param = isODBC ? "?" : "@p";

      var arr1 = new byte[] { 48, 57 };
      using (var conn = dataSource.GetDataConnection(output)) {
        Assert.Equal(arr1, conn.Execute<byte[]>($"SELECT {param}", DataParameter.Binary("p", arr1)));
        Assert.Equal(arr1, conn.Execute<byte[]>($"SELECT {param}", DataParameter.VarBinary("p", arr1)));
        Assert.Equal(arr1, conn.Execute<byte[]>($"SELECT {param}", DataParameter.Create("p", arr1)));

        if (isODBC) // ODBC provider doesn't return type for NULL value
          Assert.Null(conn.Execute<byte[]>($"SELECT CVar({param})", DataParameter.VarBinary("p", null)));
        else
          Assert.Null(conn.Execute<byte[]>($"SELECT {param}", DataParameter.VarBinary("p", null)));

        Assert.Equal(new byte[0], conn.Execute<byte[]>($"SELECT {param}", DataParameter.VarBinary("p", new byte[0])));
        Assert.Equal(new byte[0], conn.Execute<byte[]>($"SELECT {param}", DataParameter.Image("p", new byte[0])));
        Assert.Equal(arr1, conn.Execute<byte[]>($"SELECT {param}", new DataParameter { Name = "p", Value = arr1 }));
        Assert.Equal(arr1, conn.Execute<byte[]>($"SELECT {param}", DataParameter.Create("p", new Binary(arr1))));
        Assert.Equal(arr1, conn.Execute<byte[]>($"SELECT {param}", new DataParameter("p", new Binary(arr1))));
      }
    }

    [Theory, MemberData(nameof(DataSourceTheoryData))]
    public void TestGuid(DataSource dataSource) {
      var param = dataSource.IsOdbc ? "?" : "@p";

      using (var conn = dataSource.GetDataConnection(output)) {
        var guid = Guid.NewGuid();

        Assert.Equal(guid, conn.Execute<Guid>($"SELECT {param}", DataParameter.Create("p", guid)));
        Assert.Equal(guid, conn.Execute<Guid>($"SELECT {param}", new DataParameter { Name = "p", Value = guid }));
      }
    }

    [Theory, MemberData(nameof(DataSourceTheoryData))]
    public void TestSqlVariant(DataSource dataSource) {
      var isODBC = dataSource.IsOdbc;
      var param = isODBC ? "?" : "@p";

      using (var conn = dataSource.GetDataConnection(output)) {
        Assert.Equal("1", conn.Execute<object>("SELECT CVar(1)"));
        Assert.Equal(1, conn.Execute<int>("SELECT CVar(1)"));
        Assert.Equal(1, conn.Execute<int?>("SELECT CVar(1)"));
        Assert.Equal("1", conn.Execute<string>("SELECT CVar(1)"));

        // ODBC doesn't have variant type and maps it to Binary
        if (!isODBC)
          Assert.Equal("1", conn.Execute<string>($"SELECT {param}", DataParameter.Variant("p", 1)));
      }
    }

    [Theory, MemberData(nameof(DataSourceTheoryData))]
    public void TestXml(DataSource dataSource) {
      var param = dataSource.IsOdbc ? "?" : "@p";

      using (var conn = dataSource.GetDataConnection(output)) {
        Assert.Equal("<xml/>", conn.Execute<string>("SELECT '<xml/>'"));
        Assert.Equal("<xml />", conn.Execute<XDocument>("SELECT '<xml/>'").ToString());
        Assert.Equal("<xml />", conn.Execute<XmlDocument>("SELECT '<xml/>'").InnerXml);

        var xdoc = XDocument.Parse("<xml/>");
        var xml = Convert<string, XmlDocument>.Lambda("<xml/>");

        Assert.Equal("<xml/>", conn.Execute<string>($"SELECT {param}", DataParameter.Xml("p", "<xml/>")));
        Assert.Equal("<xml />", conn.Execute<XDocument>($"SELECT {param}", DataParameter.Xml("p", xdoc)).ToString());
        Assert.Equal("<xml />", conn.Execute<XmlDocument>($"SELECT {param}", DataParameter.Xml("p", xml)).InnerXml);
        Assert.Equal("<xml />", conn.Execute<XDocument>($"SELECT {param}", new DataParameter("p", xdoc)).ToString());
        Assert.Equal("<xml />", conn.Execute<XDocument>($"SELECT {param}", new DataParameter("p", xml)).ToString());
      }
    }

    [Theory, MemberData(nameof(DataSourceTheoryData))]
    public void TestEnum1(DataSource dataSource) {
      using (var conn = dataSource.GetDataConnection(output)) {
        Assert.Equal(TestEnum.AA, conn.Execute<TestEnum>("SELECT 'A'"));
        Assert.Equal(TestEnum.AA, conn.Execute<TestEnum?>("SELECT 'A'"));
        Assert.Equal(TestEnum.BB, conn.Execute<TestEnum>("SELECT 'B'"));
        Assert.Equal(TestEnum.BB, conn.Execute<TestEnum?>("SELECT 'B'"));
      }
    }

    [Theory, MemberData(nameof(DataSourceTheoryData))]
    public void TestEnum2(DataSource dataSource) {
      var param = dataSource.IsOdbc ? "?" : "@p";

      using (var conn = dataSource.GetDataConnection(output)) {
        Assert.Equal("A", conn.Query<string>($"SELECT {param}", new { p = TestEnum.AA }).First());
        Assert.Equal("B", conn.Query<string>($"SELECT {param}", new { p = (TestEnum?)TestEnum.BB }).First());
        Assert.Equal("A", conn.Query<string>($"SELECT {param}", new { p = ConvertTo<string>.From((TestEnum?)TestEnum.AA) }).First());
        Assert.Equal("A", conn.Query<string>($"SELECT {param}", new { p = ConvertTo<string>.From(TestEnum.AA) }).First());
        Assert.Equal("A", conn.Query<string>($"SELECT {param}", new { p = conn.MappingSchema.GetConverter<TestEnum?, string>()!(TestEnum.AA) }).First());
      }
    }

    [Theory(Skip = "Test requires JET provider"), MemberData(nameof(DataSourceTheoryData))]
    public void TestCreateDatabase(DataSource dataSource) {
      //var cs = DataConnection.GetConnectionString(context);
      var cs = dataSource.ConnectionString;
      //if (!cs.Contains("Microsoft.Jet.OLEDB"))
      //  Assert.Inconclusive("Test requires JET provider");

      AccessTools.CreateDatabase("TestDatabase", deleteIfExists: true);
      Assert.True(File.Exists("TestDatabase.mdb"));

      //using (var db = new DataConnection(AccessTools.GetDataProvider(), "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=TestDatabase.mdb;Locale Identifier=1033;Jet OLEDB:Engine Type=5;Persist Security Info=True")) {
      //  db.CreateTable<SqlCeTests.CreateTableTest>();
      //  db.DropTable<SqlCeTests.CreateTableTest>();
      //}

      AccessTools.DropDatabase("TestDatabase");
      Assert.False(File.Exists("TestDatabase.mdb"));
    }

    [Theory, MemberData(nameof(DataSourceTheoryData))]
    public void TestBulkCopyLinqTypes(DataSource dataSource) {
      foreach (var bulkCopyType in new[] { BulkCopyType.MultipleRows, BulkCopyType.ProviderSpecific }) {
        using (var db = dataSource.GetDataConnection(output)) {
          try {
            db.BulkCopy(
              new BulkCopyOptions { BulkCopyType = bulkCopyType },
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

    [Theory, MemberData(nameof(DataSourceTheoryData))]
    public async Task TestBulkCopyLinqTypesAsync(DataSource dataSource) {
      foreach (var bulkCopyType in new[] { BulkCopyType.MultipleRows, BulkCopyType.ProviderSpecific }) {
        using (var db = dataSource.GetDataConnection(output)) {
          try {
            await db.BulkCopyAsync(
              new BulkCopyOptions { BulkCopyType = bulkCopyType },
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

    [Theory(Skip = "Long running test. Run explicitly."), MemberData(nameof(DataSourceTheoryData))]
    //		[Timeout(60000)]
    public void TestDataConnection(DataSource dataSource) {
      //var cs = DataConnection.GetConnectionString(context);
      var cs = dataSource.ConnectionString;
      for (var i = 0; i < 1000; i++) {
        using (var db = AccessTools.CreateDataConnection(cs, dataSource.DataProvider.Name)) {
          var list = db.GetTable<Person>().Where(p => p.ID > 0).ToList();
        }
      }
    }

    [Theory, MemberData(nameof(DataSourceTheoryData))]
    public void TestZeroDate(DataSource dataSource) {
      using (var db = dataSource.GetDataConnection(output))
      using (var table = db.CreateLocalTable<DateTable>()) {
        table.Insert(() => new DateTable() { ID = 1, Date = new DateTime(1899, 12, 29) });
        table.Insert(() => new DateTable() { ID = 2, Date = new DateTime(1899, 12, 30) });
        table.Insert(() => new DateTable() { ID = 3, Date = new DateTime(1899, 12, 31) });
        table.Insert(() => new DateTable() { ID = 4, Date = new DateTime(1900, 1, 1) });

        var res = table.OrderBy(_ => _.ID).ToArray();

        Assert.Equal(4, res.Length);
        Assert.Equal(1, res[0].ID);
        Assert.Equal(new DateTime(1899, 12, 29), res[0].Date);
        Assert.Equal(2, res[1].ID);
        Assert.Equal(new DateTime(1899, 12, 30), res[1].Date);
        Assert.Equal(3, res[2].ID);
        Assert.Equal(new DateTime(1899, 12, 31), res[2].Date);
        Assert.Equal(4, res[3].ID);
        Assert.Equal(new DateTime(1900, 1, 1), res[3].Date);
      }
    }

    [Theory, MemberData(nameof(DataSourceTheoryData))]
    public void TestIssue1906(DataSource dataSource) {
      using (var db = dataSource.GetDataConnection(output))
      using (db.CreateLocalTable<CtqResultModel>())
      using (db.CreateLocalTable<CtqDefinitionModel>())
      using (db.CreateLocalTable<CtqSetModel>())
      using (db.CreateLocalTable<FtqSectorModel>()) {
        db.GetTable<CtqResultModel>()
          .LoadWith(f => f.Definition.Set!.Sector)
          .ToList();
      }
    }

  }
}
