using IBM.Data.DB2.iSeries;
using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Data;
using LinqToDB.DataProvider.DB2iSeries;
using LinqToDB.Mapping;
using System;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Xunit;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
#if !NETSTANDARD1_6 && !NETSTANDARD2_0
#endif
using LinqToDB.Linq;
#if !NETSTANDARD1_6 && !NETSTANDARD2_0
#endif
//[assembly: Parallelizable]
namespace Tests {
  using Model;
  static class DataCache<T>
    where T : class {
    static readonly Dictionary<string, List<T>> _dic = new Dictionary<string, List<T>>();
    public static List<T> Get(string context) {
      lock (_dic) {
        context = context.Replace(".LinqService", "");
        if (!_dic.TryGetValue(context, out var list)) {
          using (new DisableLogging())
          using (var db = new DataConnection(context)) {
            list = db.GetTable<T>().ToList();
            _dic.Add(context, list);
          }
        }
        return list;
      }
    }
    public static void Clear() => _dic.Clear();
  }
  public static class Helpers {
    public static string ToInvariantString<T>(this T data) => string.Format(CultureInfo.InvariantCulture, "{0}", data).Replace(',', '.').Trim(' ', '.', '0');
  }
  public class AllowMultipleQuery : IDisposable {
    public AllowMultipleQuery() {
      Configuration.Linq.AllowMultipleQuery = true;
    }
    public void Dispose() => Configuration.Linq.AllowMultipleQuery = false;
  }
  public class GuardGrouping : IDisposable {
    public GuardGrouping() {
      Configuration.Linq.GuardGrouping = true;
    }
    public void Dispose() => Configuration.Linq.GuardGrouping = false;
  }
  public class DisableLogging : IDisposable {
    private readonly TraceSwitch _traceSwitch;
    public DisableLogging() {
      _traceSwitch = DataConnection.TraceSwitch;
      DataConnection.TurnTraceSwitchOn(TraceLevel.Off);
    }
    public void Dispose() => DataConnection.TraceSwitch = _traceSwitch;
  }
  public class DisableQueryCache : IDisposable {
    public DisableQueryCache() {
      Configuration.Linq.DisableQueryCache = true;
    }
    public void Dispose() => Configuration.Linq.DisableQueryCache = false;
  }
  public class WithoutJoinOptimization : IDisposable {
    public WithoutJoinOptimization() {
      Configuration.Linq.OptimizeJoins = false;
    }
    public void Dispose() => Configuration.Linq.OptimizeJoins = true;
  }
  public class DeletePerson : IDisposable {
    readonly IDataContext _db;
    public DeletePerson(IDataContext db) {
      _db = db;
      Delete(_db);
    }
    public void Dispose() => Delete(_db);
    readonly Func<IDataContext, int> Delete =
      CompiledQuery.Compile<IDataContext, int>(db => db.GetTable<Person>().Delete(_ => _.ID > TestBase.MaxPersonID));
  }
  public class WithoutComparasionNullCheck : IDisposable {
    public WithoutComparasionNullCheck() {
      Configuration.Linq.CompareNullsAsValues = false;
    }
    public void Dispose() {
      Configuration.Linq.CompareNullsAsValues = true;
      Query.ClearCaches();
    }
  }
  public abstract class DataSourcesBaseAttribute : DataAttribute, IParameterDataSource {
    public bool IncludeLinqService { get; }
    public string[] Providers { get; }
    protected DataSourcesBaseAttribute(bool includeLinqService, string[] providers) {
      IncludeLinqService = includeLinqService;
      Providers = providers;
    }
    public IEnumerable GetData(IParameterInfo parameter) {
      if (!IncludeLinqService)
        return GetProviders();
      var providers = GetProviders().ToArray();
      return providers.Concat(providers.Select(p => p + ".LinqService"));
    }
    protected abstract IEnumerable<string> GetProviders();
  }
  [AttributeUsage(AttributeTargets.Parameter)]
  public class DataSourcesAttribute : DataSourcesBaseAttribute {
    public DataSourcesAttribute(params string[] excludeProviders) : base(true, excludeProviders) {
    }
    public DataSourcesAttribute(bool includeLinqService, params string[] excludeProviders) : base(includeLinqService, excludeProviders) {
    }
    protected override IEnumerable<string> GetProviders() => TestBase.UserProviders.Where(p => !Providers.Contains(p) && TestBase.Providers.Contains(p));
  }
  [AttributeUsage(AttributeTargets.Parameter)]
  public class IncludeDataSourcesAttribute : DataSourcesBaseAttribute {
    public IncludeDataSourcesAttribute(params string[] includeProviders) : base(true, includeProviders) {
    }
    public IncludeDataSourcesAttribute(bool includeLinqService, params string[] includeProviders) : base(includeLinqService, includeProviders) {
    }
    protected override IEnumerable<string> GetProviders() => Providers.Where(TestBase.UserProviders.Contains);
  }
}

namespace Tests.Model {
  public enum Gender {
    [MapValue("M")] Male,
    [MapValue("F")] Female,
    [MapValue("U")] Unknown,
    [MapValue("O")] Other,
  }

  public interface IPerson {
    int ID { get; set; }
    Gender Gender { get; set; }
    string FirstName { get; set; }
    string MiddleName { get; set; }
    string LastName { get; set; }
  }

  public class Patient {
    [PrimaryKey] public int PersonID;
    public string Diagnosis;
    [Association(ThisKey = "PersonID", OtherKey = "ID", CanBeNull = false)] public Person Person;
    public override bool Equals(object obj) => Equals(obj as Patient);
    public bool Equals(Patient other) {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return other.PersonID == PersonID && Equals(other.Diagnosis, Diagnosis);
    }
    public override int GetHashCode() {
      unchecked {
        var result = PersonID;
        result = (result * 397) ^ (Diagnosis != null ? Diagnosis.GetHashCode() : 0);
        return result;
      }
    }
  }


  public class Person : IPerson {
    public Person() { }
    public Person(int id) {
      ID = id;
    }
    public Person(int id, string firstName) : this(id) {
      FirstName = firstName;
    }
    // Firebird: it duplicates identity generation trigger job
    //[SequenceName(ProviderName.Firebird, "PersonID")]
    [Column("PersonID"), Identity, PrimaryKey] public int ID;
    [NotNull] public string FirstName { get; set; }
    [NotNull] public string LastName;
    [Nullable] public string MiddleName;
    [Column(DataType = DataType.Char, Length = 1)] public Gender Gender;
    [NotColumn] int IPerson.ID { get => ID; set => ID = value; }
    [NotColumn] string IPerson.FirstName { get => FirstName; set => FirstName = value; }
    [NotColumn] string IPerson.LastName { get => LastName; set => LastName = value; }
    [NotColumn] string IPerson.MiddleName { get => MiddleName; set => MiddleName = value; }
    [NotColumn] Gender IPerson.Gender { get => Gender; set => Gender = value; }
    [NotColumn] public string Name => FirstName + " " + LastName;
    [Association(ThisKey = "ID", OtherKey = "PersonID", CanBeNull = true)]
    public Patient Patient;
    public override bool Equals(object obj) => Equals(obj as Person);
    public bool Equals(Person other) {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return
        other.ID == ID &&
        Equals(other.LastName, LastName) &&
        Equals(other.MiddleName, MiddleName) &&
        other.Gender == Gender &&
        Equals(other.FirstName, FirstName);
    }
    public override int GetHashCode() {
      unchecked {
        var result = ID;
        result = (result * 397) ^ (LastName != null ? LastName.GetHashCode() : 0);
        result = (result * 397) ^ (MiddleName != null ? MiddleName.GetHashCode() : 0);
        result = (result * 397) ^ Gender.GetHashCode();
        result = (result * 397) ^ (FirstName != null ? FirstName.GetHashCode() : 0);
        return result;
      }
    }
  }
}

namespace Tests.DataProvider {
  public class DB2iSeriesTests : TestBase {

    [Fact, DataContextSource(false)]
    public void TestParameters(string context) {
      using (var conn = new DataConnection(context)) {
        Assert.Equal("1", conn.Execute<string>("SELECT cast(@p as varchar(10)) FROM SYSIBM.SYSDUMMY1", new { p = 1 }));
        Assert.Equal("1", conn.Execute<string>("SELECT cast(@p as varchar(10)) FROM SYSIBM.SYSDUMMY1", new { p = "1" }));
        Assert.Equal(1, conn.Execute<int>("SELECT cast(@p as varchar(10)) FROM SYSIBM.SYSDUMMY1", new { p = new DataParameter { Value = 1 } }));
        Assert.Equal("1", conn.Execute<string>("SELECT cast(@p as varchar(10)) FROM SYSIBM.SYSDUMMY1", new { p1 = new DataParameter { Value = "1" } }));
        Assert.Equal(5, conn.Execute<int>("SELECT cast(@p1 as int) + cast(@p2 as int) FROM SYSIBM.SYSDUMMY1", new { p1 = 2, p2 = 3 }));
        Assert.Equal(5, conn.Execute<int>("SELECT cast(@p2 as int) + cast(@p1 as int) FROM SYSIBM.SYSDUMMY1", new { p2 = 2, p1 = 3 }));
      }
    }

    protected string GetNullSql = "SELECT {0} FROM {1} WHERE ID = 1";
    protected string GetValueSql = "SELECT {0} FROM {1} WHERE ID = 2";
    protected string PassNullSql = "SELECT ID FROM {1} WHERE @p IS NULL AND {0} IS NULL OR @p1 IS NOT NULL AND {0} = @p2";
    protected string PassValueSql = "SELECT ID FROM {1} WHERE {0} = @p";
    protected T TestType<T>(DataConnection conn, string fieldName,
      DataType dataType = DataType.Undefined,
      string tableName = "AllTypes",
      bool skipPass = false,
      bool skipNull = false,
      bool skipDefinedNull = false,
      bool skipDefaultNull = false,
      bool skipUndefinedNull = false,
      bool skipNotNull = false,
      bool skipDefined = false,
      bool skipDefault = false,
      bool skipUndefined = false) {
      var type = typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>)
        ? typeof(T).GetGenericArguments()[0]
        : typeof(T);
      // Get NULL value.
      //
      Debug.WriteLine("{0} {1}:{2} -> NULL", fieldName, type.Name, dataType);
      var sql = string.Format(GetNullSql, fieldName, tableName);
      var value = conn.Execute<T>(sql);
      var def = conn.MappingSchema.GetDefaultValue(typeof(T));
      Assert.Equal(def, value);
      int? id;
      if (!skipNull && !skipPass && PassNullSql != null) {
        sql = string.Format(PassNullSql, fieldName, tableName);
        if (!skipDefinedNull && dataType != DataType.Undefined) {
          // Get NULL ID with dataType.
          //
          Debug.WriteLine("{0} {1}:{2} -> NULL ID with dataType", fieldName, type.Name, dataType);
          id = conn.Execute<int?>(sql, new DataParameter("p", value, dataType), new DataParameter("p1", value, dataType),
            new DataParameter("p2", value, dataType));
          Assert.Equal(1, id);
        }
        if (!skipDefaultNull) {
          // Get NULL ID with default dataType.
          //
          Debug.WriteLine("{0} {1}:{2} -> NULL ID with default dataType", fieldName, type.Name, dataType);
          id = conn.Execute<int?>(sql, new { p = value, p1 = value, p2 = value });
          Assert.Equal(1, id);
        }
        if (!skipUndefinedNull) {
          // Get NULL ID without dataType.
          //
          Debug.WriteLine("{0} {1}:{2} -> NULL ID without dataType", fieldName, type.Name, dataType);
          id = conn.Execute<int?>(sql, new DataParameter("p", value, dataType), new DataParameter("p1", value, dataType),
            new DataParameter("p2", value, dataType));
          Assert.Equal(1, id);
        }
      }
      // Get value.
      //
      Debug.WriteLine("{0} {1}:{2} -> value", fieldName, type.Name, dataType);
      sql = string.Format(GetValueSql, fieldName, tableName);
      value = conn.Execute<T>(sql);
      if (!skipNotNull && !skipPass) {
        sql = string.Format(PassValueSql, fieldName, tableName);
        if (!skipDefined && dataType != DataType.Undefined) {
          // Get value ID with dataType.
          Debug.WriteLine("{0} {1}:{2} -> value ID with dataType", fieldName, type.Name, dataType);
          id = conn.Execute<int?>(sql, new DataParameter("p", value, dataType));
          Assert.Equal(2, id);
        }
        if (!skipDefault) {
          // Get value ID with default dataType.
          Debug.WriteLine("{0} {1}:{2} -> value ID with default dataType", fieldName, type.Name, dataType);
          id = conn.Execute<int?>(sql, new { p = value });
          Assert.Equal(2, id);
        }
        if (!skipUndefined) {
          // Get value ID without dataType.
          Debug.WriteLine("{0} {1}:{2} -> value ID without dataType", fieldName, type.Name, dataType);
          id = conn.Execute<int?>(sql, new DataParameter("p", value));
          Assert.Equal(2, id);
        }
      }
      return value;
    }

    [Fact, DataContextSource(false)]
    //DecFloatTests break on AccessClient with cultures that have a different decimal point than period.
    [SetCulture("en-US")]
    public void TestDataTypes(string context) {
      using (var conn = new DataConnection(context)) {
        Assert.Equal(1000000L, TestType<long?>(conn, "bigintDataType", DataType.Int64));
        Assert.Equal(new iDB2BigInt(1000000L), TestType<iDB2BigInt?>(conn, "bigintDataType", DataType.Int64));
        Assert.Equal(444444, TestType<int?>(conn, "intDataType", DataType.Int32));
        Assert.Equal(new iDB2Integer(444444), TestType<iDB2Integer?>(conn, "intDataType", DataType.Int32));
        Assert.Equal(100, TestType<short?>(conn, "smallintDataType", DataType.Int16));
        Assert.Equal(new iDB2SmallInt(100), TestType<iDB2SmallInt?>(conn, "smallintDataType", DataType.Int16));
        Assert.Equal(666m, TestType<decimal?>(conn, "decimalDataType", DataType.Decimal));
        Assert.Equal(888.456m, TestType<decimal?>(conn, "decfloat16DataType", DataType.Decimal));
        Assert.Equal(777.987m, TestType<decimal?>(conn, "decfloat34DataType", DataType.Decimal));
        Assert.Equal(222.987f, TestType<float?>(conn, "realDataType", DataType.Single));
        Assert.Equal(new iDB2Real(222.987f), TestType<iDB2Real?>(conn, "realDataType", DataType.Single));
        //Assert.Equal(TestType<double?>(conn, "doubleDataType", DataType.Double), Is.EqualTo(16.2d));
        //Assert.Equal(TestType<iDB2Double?>(conn, "doubleDataType", DataType.Double), Is.EqualTo(new iDB2Double(16.2d)));
        Assert.Equal("Y", TestType<string>(conn, "charDataType", DataType.Char));
        Assert.Equal("Y", TestType<string>(conn, "charDataType", DataType.NChar));
        Assert.Equal(new iDB2VarChar("Y"), TestType<iDB2VarChar?>(conn, "charDataType", DataType.Char));
        Assert.Equal("var-char", TestType<string>(conn, "varcharDataType", DataType.VarChar));
        Assert.Equal("var-char", TestType<string>(conn, "varcharDataType", DataType.NVarChar));
        Assert.Equal("567", TestType<string>(conn, "clobDataType", DataType.Text));
        Assert.Equal("890", TestType<string>(conn, "dbclobDataType", DataType.NText));
        Assert.Equal(new byte[] { 0xF1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, TestType<byte[]>(conn, "binaryDataType", DataType.Binary));
        Assert.Equal(new byte[] { 0xF4 }, TestType<byte[]>(conn, "varbinaryDataType", DataType.VarBinary));
        Assert.Equal(new byte[] { 0xF2, 0xF3, 0xF4 }, TestType<byte[]>(conn, "blobDataType", DataType.Blob, skipDefaultNull: true, skipUndefinedNull: true, skipDefault: true, skipUndefined: true));
        Assert.Equal(new byte[] { 0xF2, 0xF3, 0xF4 }, TestType<byte[]>(conn, "blobDataType", DataType.VarBinary, skipDefaultNull: true, skipUndefinedNull: true, skipDefault: true, skipUndefined: true));
        Assert.Equal("graphic   ", TestType<string>(conn, "graphicDataType", DataType.VarChar));
        Assert.Equal("vargraphic", TestType<string>(conn, "vargraphicDataType", DataType.VarChar));
        Assert.Equal(new DateTime(2012, 12, 12), TestType<DateTime?>(conn, "dateDataType", DataType.Date, skipDefault: true, skipUndefined: true));
        Assert.Equal(new iDB2Date(new DateTime(2012, 12, 12)), TestType<iDB2Date?>(conn, "dateDataType", DataType.Date));
        Assert.Equal(new TimeSpan(12, 12, 12), TestType<TimeSpan?>(conn, "timeDataType", DataType.Time));
        Assert.Equal(new iDB2Time(new DateTime(1, 1, 1, 12, 12, 12)), TestType<iDB2Time?>(conn, "timeDataType", DataType.Time));
        Assert.Equal(new DateTime(2012, 12, 12, 12, 12, 12, 0), TestType<DateTime?>(conn, "timestampDataType", DataType.DateTime2, skipDefault: true, skipUndefined: true));
        Assert.Equal(new iDB2TimeStamp(new DateTime(2012, 12, 12, 12, 12, 12, 0)), TestType<iDB2TimeStamp?>(conn, "timestampDataType", DataType.DateTime2));
        Assert.Equal("<root><element strattr=\"strvalue\" intattr=\"12345\"/></root>", TestType<string>(conn, "xmlDataType", DataType.Xml, skipPass: true));
        Assert.NotEmpty(conn.Execute<byte[]>("SELECT rowidDataType FROM AllTypes WHERE ID = 2"));
        Assert.NotEmpty(conn.Execute<iDB2Rowid>("SELECT rowidDataType FROM AllTypes WHERE ID = 2").Value);
        TestType<iDB2Clob>(conn, "clobDataType", DataType.Text, skipNotNull: true);
        TestType<iDB2Blob>(conn, "blobDataType", DataType.VarBinary, skipNotNull: true);
        TestType<iDB2Xml>(conn, "xmlDataType", DataType.Xml, skipPass: true);
        Assert.Equal(new iDB2Decimal(666m).ToString(), TestType<iDB2Decimal?>(conn, "decimalDataType", DataType.Decimal).ToString());
        Assert.Equal(new iDB2Binary(new byte[] { 0xF4 }).ToString(), TestType<iDB2Binary>(conn, "varbinaryDataType", DataType.VarBinary).ToString());
        Assert.Equal(new iDB2DecFloat16(888.456m), TestType<iDB2DecFloat16?>(conn, "decfloat16DataType", DataType.Decimal));
        Assert.Equal(new iDB2DecFloat34(777.987m).ToString(), TestType<iDB2DecFloat34?>(conn, "decfloat34DataType", DataType.Decimal).ToString());
      }
    }
    static void TestNumeric<T>(DataConnection conn, T expectedValue, DataType dataType, string skip = "") {
      var skipTypes = skip.Split(' ');
      foreach (var sqlType in new[] {
        "bigint",
        "int",
        "smallint",
        "decimal(31)",
        "decfloat",
        "double",
        "real"
      }.Except(skipTypes)) {
        var sqlValue = expectedValue is bool ? (bool)(object)expectedValue ? 1 : 0 : (object)expectedValue;
        var sql = string.Format("SELECT Cast({0} as {1}) FROM SYSIBM.SYSDUMMY1", sqlValue ?? "NULL", sqlType);
        Debug.WriteLine(sql + " -> " + typeof(T));
        Assert.Equal(expectedValue, conn.Execute<T>(sql));
      }
      var castType = "real";
      switch (dataType) {
        case DataType.Boolean:
        case DataType.Int16:
        case DataType.Int32:
        case DataType.Int64:
        case DataType.UInt32:
          castType = "bigint";
          break;
        case DataType.UInt16:
          castType = "int";
          break;
        case DataType.UInt64:
          castType = "decimal(20,0)";
          break;
        case DataType.Single:
          castType = "real";
          break;
        case DataType.Double:
          castType = "float(34)";
          break;
        case DataType.VarNumeric:
        case DataType.Decimal:
          if (expectedValue != null) {
            var val = expectedValue.ToString();
            var precision = val.Replace("-", "").Replace(".", "").Length;
            var point = val.IndexOf(".");
            var scale = point < 0 ? 0 : val.Length - point;
            castType = string.Format("decimal({0},{1})", precision, scale);
          } else
            castType = "decimal";
          break;
        case DataType.Money:
          castType = "decfloat";
          break;
      }
      Debug.WriteLine("{0} -> DataType.{1}", typeof(T), dataType);
      var sql1 = $"SELECT cast(@p as {castType}) FROM SYSIBM.SYSDUMMY1";
      Assert.Equal(expectedValue, conn.Execute<T>(sql1, new DataParameter { Name = "p", DataType = dataType, Value = expectedValue }));
      Debug.WriteLine("{0} -> auto", typeof(T));
      Assert.Equal(expectedValue,
        conn.Execute<T>($"SELECT cast(@p as {castType}) FROM SYSIBM.SYSDUMMY1",
          new DataParameter { Name = "p", Value = expectedValue }));
      Debug.WriteLine("{0} -> new", typeof(T));
      Assert.Equal(expectedValue, conn.Execute<T>($"SELECT cast(@p as {castType}) FROM SYSIBM.SYSDUMMY1", new { p = expectedValue }));
    }
    static void TestSimple<T>(DataConnection conn, T expectedValue, DataType dataType)
      where T : struct {
      TestNumeric<T>(conn, expectedValue, dataType);
      TestNumeric<T?>(conn, expectedValue, dataType);
      TestNumeric<T?>(conn, null, dataType);
    }
    [Fact, DataContextSource(false)]
    //Test uses string format to build sql values, invariant culture is needed
    [SetCulture("en-US")]
    public void TestNumerics(string context) {
      using (var conn = new DataConnection(context)) {
        TestSimple<sbyte>(conn, 1, DataType.SByte);
        TestSimple<short>(conn, 1, DataType.Int16);
        TestSimple<int>(conn, 1, DataType.Int32);
        TestSimple<long>(conn, 1L, DataType.Int64);
        TestSimple<byte>(conn, 1, DataType.Byte);
        TestSimple<ushort>(conn, 1, DataType.UInt16);
        TestSimple<uint>(conn, 1u, DataType.UInt32);
        TestSimple<ulong>(conn, 1ul, DataType.UInt64);
        TestSimple<float>(conn, 1, DataType.Single);
        TestSimple<double>(conn, 1d, DataType.Double);
        TestSimple<decimal>(conn, 1m, DataType.Decimal);
        TestSimple<decimal>(conn, 1m, DataType.VarNumeric);
        TestSimple<decimal>(conn, 1m, DataType.Money);
        TestSimple<decimal>(conn, 1m, DataType.SmallMoney);
        TestNumeric(conn, sbyte.MinValue, DataType.SByte);
        TestNumeric(conn, sbyte.MaxValue, DataType.SByte);
        TestNumeric(conn, short.MinValue, DataType.Int16);
        TestNumeric(conn, short.MaxValue, DataType.Int16);
        TestNumeric(conn, int.MinValue, DataType.Int32, "smallint");
        TestNumeric(conn, int.MaxValue, DataType.Int32, "smallint real");
        TestNumeric(conn, long.MinValue, DataType.Int64, "smallint int double");
        TestNumeric(conn, long.MaxValue, DataType.Int64, "smallint int double real");
        TestNumeric(conn, byte.MaxValue, DataType.Byte);
        TestNumeric(conn, ushort.MaxValue, DataType.UInt16, "smallint");
        TestNumeric(conn, uint.MaxValue, DataType.UInt32, "smallint int real");
        TestNumeric(conn, ulong.MaxValue, DataType.UInt64, "smallint int real bigint double");
        TestNumeric(conn, -3.40282306E+38f, DataType.Single, "bigint int smallint decimal(31) decfloat");
        TestNumeric(conn, 3.40282306E+38f, DataType.Single, "bigint int smallint decimal(31) decfloat");
        TestNumeric(conn, -1.79E+308d, DataType.Double, "bigint int smallint decimal(31) decfloat real");
        TestNumeric(conn, 1.79E+308d, DataType.Double, "bigint int smallint decimal(31) decfloat real");
        TestNumeric(conn, decimal.MinValue, DataType.Decimal, "bigint int smallint double real");
        TestNumeric(conn, decimal.MaxValue, DataType.Decimal, "bigint int smallint double real");
        TestNumeric(conn, decimal.MinValue, DataType.VarNumeric, "bigint int smallint double real");
        TestNumeric(conn, decimal.MaxValue, DataType.VarNumeric, "bigint int smallint double real");
        TestNumeric(conn, -922337203685477m, DataType.Money, "int smallint real");
        TestNumeric(conn, +922337203685477m, DataType.Money, "int smallint real");
        TestNumeric(conn, -214748m, DataType.SmallMoney, "smallint");
        TestNumeric(conn, +214748m, DataType.SmallMoney, "smallint");
      }
    }

    [Fact, DataContextSource(false)]
    public void TestDate(string context) {
      using (var conn = new DataConnection(context)) {
        var dateTime = new DateTime(2012, 12, 12);
        Assert.Equal(dateTime, conn.Execute<DateTime>("SELECT Cast('2012-12-12' as date) FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal(dateTime, conn.Execute<DateTime?>("SELECT Cast('2012-12-12' as date) FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal(dateTime, conn.Execute<DateTime>("SELECT cast(@p as date) FROM SYSIBM.SYSDUMMY1", DataParameter.Date("p", dateTime)));
        Assert.Equal(dateTime, conn.Execute<DateTime?>("SELECT cast(@p as date) FROM SYSIBM.SYSDUMMY1", new DataParameter("p", dateTime, DataType.Date)));
      }
    }

    [Fact, DataContextSource(false)]
    public void TestDateTime(string context) {
      using (var conn = new DataConnection(context)) {
        var dateTime = new DateTime(2012, 12, 12, 12, 12, 12);
        Assert.Equal(dateTime, conn.Execute<DateTime>("SELECT Cast('2012-12-12 12:12:12' as timestamp) FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal(dateTime, conn.Execute<DateTime?>("SELECT Cast('2012-12-12 12:12:12' as timestamp) FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal(dateTime, conn.Execute<DateTime>("SELECT cast(@p as timestamp) FROM SYSIBM.SYSDUMMY1", DataParameter.DateTime("p", dateTime)));
        Assert.Equal(dateTime, conn.Execute<DateTime?>("SELECT cast(@p as timestamp) FROM SYSIBM.SYSDUMMY1", new DataParameter("p", dateTime)));
        Assert.Equal(dateTime, conn.Execute<DateTime?>("SELECT cast(@p as timestamp) FROM SYSIBM.SYSDUMMY1", new DataParameter("p", dateTime, DataType.DateTime)));
      }
    }
    [Fact, DataContextSource(false)]
    public void TestTimeSpan(string context) {
      using (var conn = new DataConnection(context)) {
        var time = new TimeSpan(12, 12, 12);
        Assert.Equal(time, conn.Execute<TimeSpan>("SELECT Cast('12:12:12' as time) FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal(time, conn.Execute<TimeSpan?>("SELECT Cast('12:12:12' as time) FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal(time, conn.Execute<TimeSpan>("SELECT cast(@p as time) FROM SYSIBM.SYSDUMMY1", DataParameter.Time("p", time)));
        Assert.Equal(time, conn.Execute<TimeSpan>("SELECT cast(@p as time) FROM SYSIBM.SYSDUMMY1", DataParameter.Create("p", time)));
        Assert.Equal(time, conn.Execute<TimeSpan?>("SELECT cast(@p as time) FROM SYSIBM.SYSDUMMY1", new DataParameter("p", time, DataType.Time)));
        Assert.Equal(time, conn.Execute<TimeSpan?>("SELECT cast(@p as time) FROM SYSIBM.SYSDUMMY1", new DataParameter("p", time)));
      }
    }
    [Fact, DataContextSource(false)]
    public void TestChar(string context) {
      using (var conn = new DataConnection(context)) {
        Assert.Equal('1', conn.Execute<char>("SELECT Cast('1' as char) FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal('1', conn.Execute<char?>("SELECT Cast('1' as char) FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal('1', conn.Execute<char>("SELECT Cast('1' as char(1)) FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal('1', conn.Execute<char?>("SELECT Cast('1' as char(1)) FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal('1', conn.Execute<char>("SELECT Cast('1' as varchar(1)) FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal('1', conn.Execute<char?>("SELECT Cast('1' as varchar(1)) FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal('1', conn.Execute<char>("SELECT Cast('1' as varchar(20)) FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal('1', conn.Execute<char?>("SELECT Cast('1' as varchar(20)) FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal('1', conn.Execute<char>("SELECT Cast(@p as char) FROM SYSIBM.SYSDUMMY1", DataParameter.Char("p", '1')));
        Assert.Equal('1', conn.Execute<char?>("SELECT Cast(@p as char) FROM SYSIBM.SYSDUMMY1", DataParameter.Char("p", '1')));
        Assert.Equal('1', conn.Execute<char>("SELECT Cast(@p as char(1)) FROM SYSIBM.SYSDUMMY1", DataParameter.Char("p", '1')));
        Assert.Equal('1', conn.Execute<char?>("SELECT Cast(@p as char(1)) FROM SYSIBM.SYSDUMMY1", DataParameter.Char("p", '1')));
        Assert.Equal('1', conn.Execute<char>("SELECT CAST(@p as varchar(1)) FROM SYSIBM.SYSDUMMY1", DataParameter.VarChar("p", '1')));
        Assert.Equal('1', conn.Execute<char?>("SELECT CAST(@p as varchar(1)) FROM SYSIBM.SYSDUMMY1", DataParameter.VarChar("p", '1')));
        Assert.Equal('1', conn.Execute<char>("SELECT CAST(@p as nchar(1)) FROM SYSIBM.SYSDUMMY1", DataParameter.NChar("p", '1')));
        Assert.Equal('1', conn.Execute<char?>("SELECT CAST(@p as nchar(1)) FROM SYSIBM.SYSDUMMY1", DataParameter.NChar("p", '1')));
        Assert.Equal('1', conn.Execute<char>("SELECT CAST(@p as nvarchar(1)) FROM SYSIBM.SYSDUMMY1", DataParameter.NVarChar("p", '1')));
        Assert.Equal('1', conn.Execute<char?>("SELECT CAST(@p as nvarchar(1)) FROM SYSIBM.SYSDUMMY1", DataParameter.NVarChar("p", '1')));
        Assert.Equal('1', conn.Execute<char>("SELECT CAST(@p as nvarchar(10)) FROM SYSIBM.SYSDUMMY1", DataParameter.Create("p", '1')));
        Assert.Equal('1', conn.Execute<char?>("SELECT CAST(@p as nvarchar(10)) FROM SYSIBM.SYSDUMMY1", DataParameter.Create("p", '1')));
        Assert.Equal('1', conn.Execute<char>("SELECT CAST(@p as nvarchar(10)) FROM SYSIBM.SYSDUMMY1", new DataParameter { Name = "p", Value = '1' }));
        Assert.Equal('1', conn.Execute<char?>("SELECT CAST(@p as nvarchar(10)) FROM SYSIBM.SYSDUMMY1", new DataParameter { Name = "p", Value = '1' }));
      }
    }
    [Fact, DataContextSource(false)]
    public void TestString(string context) {
      using (var conn = new DataConnection(context)) {
        Assert.Equal("12345", conn.Execute<string>("SELECT Cast('12345' as char(5)) FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal("12345", conn.Execute<string>("SELECT Cast('12345' as char(20)) FROM SYSIBM.SYSDUMMY1"));
        Assert.Null(conn.Execute<string>("SELECT Cast(NULL    as char(20)) FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal("12345", conn.Execute<string>("SELECT Cast('12345' as varchar(5)) FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal("12345", conn.Execute<string>("SELECT Cast('12345' as varchar(20)) FROM SYSIBM.SYSDUMMY1"));
        Assert.Null(conn.Execute<string>("SELECT Cast(NULL    as varchar(20)) FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal("12345", conn.Execute<string>("SELECT Cast('12345' as clob) FROM SYSIBM.SYSDUMMY1"));
        Assert.Null(conn.Execute<string>("SELECT Cast(NULL    as clob) FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal("123", conn.Execute<string>("SELECT cast(@p as char(5)) FROM SYSIBM.SYSDUMMY1", DataParameter.Char("p", "123")));
        Assert.Equal("123", conn.Execute<string>("SELECT cast(@p as varchar(10)) FROM SYSIBM.SYSDUMMY1", DataParameter.VarChar("p", "123")));
        Assert.Equal("123", conn.Execute<string>("SELECT cast(@p as varchar(10)) FROM SYSIBM.SYSDUMMY1", DataParameter.Text("p", "123")));
        Assert.Equal("123  ", conn.Execute<string>("SELECT cast(@p as nchar(5)) FROM SYSIBM.SYSDUMMY1", DataParameter.NChar("p", "123")));
        Assert.Equal("123", conn.Execute<string>("SELECT cast(@p as nvarchar(10)) FROM SYSIBM.SYSDUMMY1", DataParameter.NVarChar("p", "123")));
        Assert.Equal("123", conn.Execute<string>("SELECT cast(@p as nvarchar(10)) FROM SYSIBM.SYSDUMMY1", DataParameter.NText("p", "123")));
        Assert.Equal("123", conn.Execute<string>("SELECT cast(@p as nvarchar(10)) FROM SYSIBM.SYSDUMMY1", DataParameter.Create("p", "123")));
        Assert.Null(conn.Execute<string>("SELECT cast(@p as nvarchar(10)) FROM SYSIBM.SYSDUMMY1", DataParameter.Create("p", (string)null)));
        Assert.Equal("1", conn.Execute<string>("SELECT cast(@p as nvarchar(10)) FROM SYSIBM.SYSDUMMY1", new DataParameter { Name = "p", Value = "1" }));
      }
    }
    [Fact, DataContextSource(false)]
    public void TestBinary(string context) {
      // results are going to be bytes from EDCIDC character set
      var arr1 = new byte[] { 241, 242 };
      var arr2 = new byte[] { 241, 242, 243, 244 };
      using (var conn = new DataConnection(context)) {
        var res1 = conn.Execute<byte[]>("SELECT Cast('12' as char(2) for bit data) FROM SYSIBM.SYSDUMMY1");
        Assert.Equal(arr1, res1, Is.EqualTo(arr1));
        Assert.Equal(new Binary(arr2), conn.Execute<Binary>("SELECT Cast('1234' as char(4) for bit data) FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal(arr1, conn.Execute<byte[]>("SELECT Cast('12' as varchar(2) for bit data) FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal(new Binary(arr2), conn.Execute<Binary>("SELECT Cast('1234' as varchar(4) for bit data) FROM SYSIBM.SYSDUMMY1"));
      }
    }
    [Fact, IncludeDataContextSource(DB2iSeriesProviderName.DB2_73, DB2iSeriesProviderName.DB2)]
    public void TestGuidBlob(string context) {
      using (var conn = new DataConnection(context)) {
        Assert.Equal(new Guid("6F9619FF-8B86-D011-B42D-00C04FC964FF"), conn.Execute<Guid>("SELECT Cast('6F9619FF-8B86-D011-B42D-00C04FC964FF' as varchar(38))  FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal(new Guid("6F9619FF-8B86-D011-B42D-00C04FC964FF"), conn.Execute<Guid?>("SELECT Cast('6F9619FF-8B86-D011-B42D-00C04FC964FF' as varchar(38)) FROM SYSIBM.SYSDUMMY1"));
        var guid = Guid.NewGuid();
        Assert.Equal(guid, conn.Execute<Guid>("SELECT Cast(@p as char(16) for bit data) FROM SYSIBM.SYSDUMMY1", DataParameter.Create("p", guid)), Is.EqualTo(guid));
        Assert.Equal(guid, conn.Execute<Guid>("SELECT Cast(@p as char(16) for bit data) FROM SYSIBM.SYSDUMMY1", new DataParameter { Name = "p", Value = guid }), Is.EqualTo(guid));
      }
    }
    [Fact, IncludeDataContextSource(DB2iSeriesProviderName.DB2_73_GAS, DB2iSeriesProviderName.DB2_GAS)]
    public void TestGuidAsString(string context) {
      using (var conn = new DataConnection(context)) {
        Assert.Equal(new Guid("6F9619FF-8B86-D011-B42D-00C04FC964FF"), conn.Execute<Guid>("SELECT Cast('6F9619FF-8B86-D011-B42D-00C04FC964FF' as varchar(38))  FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal(new Guid("6F9619FF-8B86-D011-B42D-00C04FC964FF"), conn.Execute<Guid?>("SELECT Cast('6F9619FF-8B86-D011-B42D-00C04FC964FF' as varchar(38)) FROM SYSIBM.SYSDUMMY1"));
      }
    }
    [Fact, DataContextSource(false)]
    public void TestXml(string context) {
      using (var conn = new DataConnection(context)) {
        Assert.Equal("<xml/>", conn.Execute<string>("SELECT '<xml/>' FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal("<xml />", conn.Execute<XDocument>("SELECT '<xml/>' FROM SYSIBM.SYSDUMMY1").ToString());
        Assert.Equal("<xml />", conn.Execute<XmlDocument>("SELECT '<xml/>' FROM SYSIBM.SYSDUMMY1").InnerXml);
        var xdoc = XDocument.Parse("<xml/>");
        var xml = Convert<string, XmlDocument>.Lambda("<xml/>");
        Assert.Equal("<xml/>", conn.Execute<string>("SELECT cast(@p as nvarchar(8000)) FROM SYSIBM.SYSDUMMY1", DataParameter.Xml("p", "<xml/>")));
        Assert.Equal("<xml />", conn.Execute<XDocument>("SELECT cast(@p as nvarchar(8000)) FROM SYSIBM.SYSDUMMY1", DataParameter.Xml("p", xdoc)).ToString());
        Assert.Equal("<xml />", conn.Execute<XmlDocument>("SELECT cast(@p as nvarchar(8000)) FROM SYSIBM.SYSDUMMY1", DataParameter.Xml("p", xml)).InnerXml);
        Assert.Equal("<xml />", conn.Execute<XDocument>("SELECT cast(@p as nvarchar(8000)) FROM SYSIBM.SYSDUMMY1", new DataParameter("p", xdoc)).ToString());
        Assert.Equal("<xml />", conn.Execute<XDocument>("SELECT cast(@p as nvarchar(8000)) FROM SYSIBM.SYSDUMMY1", new DataParameter("p", xml)).ToString());
      }
    }

    enum TestEnum {
      [MapValue("A")] AA,
      [MapValue("B")] BB,
    }

    [Fact, DataContextSource(false)]
    public void TestEnum1(string context) {
      using (var conn = new DataConnection(context)) {
        Assert.Equal(TestEnum.AA, conn.Execute<TestEnum>("SELECT 'A' FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal(TestEnum.AA, conn.Execute<TestEnum?>("SELECT 'A' FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal(TestEnum.BB, conn.Execute<TestEnum>("SELECT 'B' FROM SYSIBM.SYSDUMMY1"));
        Assert.Equal(TestEnum.BB, conn.Execute<TestEnum?>("SELECT 'B' FROM SYSIBM.SYSDUMMY1"));
      }
    }

    [Fact, DataContextSource(false)]
    public void TestEnum2(string context) {
      using (var conn = new DataConnection(context)) {
        Assert.Equal("A", conn.Execute<string>("SELECT cast(@p as nvarchar(10)) FROM SYSIBM.SYSDUMMY1", new { p = TestEnum.AA }));
        Assert.Equal("B", conn.Execute<string>("SELECT cast(@p as nvarchar(10)) FROM SYSIBM.SYSDUMMY1", new { p = (TestEnum?)TestEnum.BB }));
        Assert.Equal("A", conn.Execute<string>("SELECT cast(@p as nvarchar(10)) FROM SYSIBM.SYSDUMMY1", new { p = ConvertTo<string>.From((TestEnum?)TestEnum.AA) }));
        Assert.Equal("A", conn.Execute<string>("SELECT cast(@p as nvarchar(10)) FROM SYSIBM.SYSDUMMY1", new { p = ConvertTo<string>.From(TestEnum.AA) }));
        Assert.Equal("A", conn.Execute<string>("SELECT cast(@p as nvarchar(10)) FROM SYSIBM.SYSDUMMY1", new { p = conn.MappingSchema.GetConverter<TestEnum?, string>()(TestEnum.AA) }));
      }
    }
    [Table(Name = "ALLTYPES")]
    public class ALLTYPE {
      [PrimaryKey, Identity] public int ID { get; set; } // INTEGER
      [Column(DbType = "bigint"), Nullable] public long? BIGINTDATATYPE { get; set; } // BIGINT
      [Column(DbType = "int"), Nullable] public int? INTDATATYPE { get; set; } // INTEGER
      [Column(DbType = "smallint"), Nullable]
      public short? SMALLINTDATATYPE { get; set; } // SMALLINT
      [Column(DbType = "decimal(30)"), Nullable]
      public decimal? DECIMALDATATYPE { get; set; } // DECIMAL
      [Column(DbType = "decfloat(16)"), Nullable]
      public decimal? DECFLOAT16DATATYPE { get; set; } // DECFLOAT16
      [Column(DbType = "decfloat(34)"), Nullable]
      public decimal? DECFLOAT34DATATYPE { get; set; } // DECFLOAT34
      [Column(DbType = "real"), Nullable] public float? REALDATATYPE { get; set; } // REAL
      [Column(DbType = "double"), Nullable] public double? DOUBLEDATATYPE { get; set; } // DOUBLE
      [Column(DbType = "char(1)"), Nullable] public char CHARDATATYPE { get; set; } // CHARACTER
      [Column(DbType = "varchar(20)"), Nullable]
      public string VARCHARDATATYPE { get; set; } // VARCHAR(20)
      [Column(DbType = "clob"), Nullable] public string CLOBDATATYPE { get; set; } // CLOB(1048576)
      [Column(DbType = "dclob(100)"), Nullable]
      public string DBCLOBDATATYPE { get; set; } // DBCLOB(100)
      [Column(DbType = "binary(20)"), Nullable]
      public object BINARYDATATYPE { get; set; } // CHARACTER
      [Column(DbType = "varbinary(20)"), Nullable]
      public object VARBINARYDATATYPE { get; set; } // VARCHAR(5)
      [Column, Nullable] public byte[] BLOBDATATYPE { get; set; } // BLOB(10)
      [Column(DbType = "graphic(10)"), Nullable]
      public string GRAPHICDATATYPE { get; set; } // GRAPHIC(10)
      [Column(DbType = "vargraphic(10)"), Nullable]
      public string VARGRAPHICDATATYPE { get; set; } // GRAPHIC(10)
      [Column(DbType = "date"), Nullable] public DateTime? DATEDATATYPE { get; set; } // DATE
      [Column(DbType = "time"), Nullable] public TimeSpan? TIMEDATATYPE { get; set; } // TIME
      [Column(DbType = "timestamp"), Nullable]
      public DateTime? TIMESTAMPDATATYPE { get; set; } // TIMESTAMP
      [Column, Nullable] public string XMLDATATYPE { get; set; } // XML
    }
    void BulkCopyTest(string context, BulkCopyType bulkCopyType, int maxSize, int batchSize) {
      using (var conn = new DataConnection(context)) {
        //conn.BeginTransaction();
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
              DECFLOAT16DATATYPE = 7000 + n,
              DECFLOAT34DATATYPE = 7000 + n,
              REALDATATYPE = 8000 + n,
              DOUBLEDATATYPE = 9000 + n,
              CHARDATATYPE = 'A',
              VARCHARDATATYPE = "",
              CLOBDATATYPE = null,
              DBCLOBDATATYPE = null,
              BINARYDATATYPE = null,
              VARBINARYDATATYPE = null,
              BLOBDATATYPE = new byte[] { 1, 2, 3 },
              GRAPHICDATATYPE = "abc",
              VARGRAPHICDATATYPE = "xyz",
              DATEDATATYPE = DateTime.Now.Date,
              TIMEDATATYPE = null,
              TIMESTAMPDATATYPE = null,
              XMLDATATYPE = "<root><element strattr=\"strvalue\" intattr=\"12345\"/></root>"
            }));
        conn.GetTable<ALLTYPE>().Delete(p => p.SMALLINTDATATYPE >= 5000);
      }
    }
    [Fact, DataContextSource(false)]
    public void BulkCopyMultipleRows(string context) => BulkCopyTest(context, BulkCopyType.MultipleRows, 5000, 10001);
    [Fact, DataContextSource(false)]
    public void BulkCopyProviderSpecific(string context) => Assert.Throws<System.NotImplementedException>(delegate {
      BulkCopyTest(context, BulkCopyType.ProviderSpecific, 50000, 100001);
    });

    [Fact, DataContextSource(false)]
    public void BulkCopyLinqTypesMultipleRows(string context) {
      using (var db = new DataConnection(context)) {
        db.BulkCopy(
          new BulkCopyOptions { BulkCopyType = BulkCopyType.MultipleRows },
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
        db.GetTable<LinqDataTypes>().Delete(p => p.ID >= 4000);
      }
    }

    [Fact, DataContextSource(false)]
    public void TestBinarySize(string context) {
      using (var conn = new DataConnection(context)) {
        try {
          var data = new byte[500000];
          for (var i = 0; i < data.Length; i++)
            data[i] = (byte)(i % byte.MaxValue);
          conn.GetTable<ALLTYPE>().Insert(() => new ALLTYPE {
            INTDATATYPE = 2000,
            BLOBDATATYPE = data,
          });
          var blob = conn.GetTable<ALLTYPE>().First(t => t.INTDATATYPE == 2000).BLOBDATATYPE;
          Assert.AreEqual(data, blob);
        } finally {
          conn.GetTable<ALLTYPE>().Delete(p => p.INTDATATYPE == 2000);
        }
      }
    }

    [Fact, DataContextSource(false)]
    public void TestClobSize(string context) {
      using (var conn = new DataConnection(context)) {
        try {
          var sb = new StringBuilder();
          for (var i = 0; i < 100000; i++)
            sb.Append(((char)((i % (byte.MaxValue - 31)) + 32)).ToString());
          var data = sb.ToString();
          conn.GetTable<ALLTYPE>().Insert(() => new ALLTYPE {
            INTDATATYPE = 2000,
            CLOBDATATYPE = data,
          });
          var blob = conn.GetTable<ALLTYPE>()
            .Where(t => t.INTDATATYPE == 2000)
            .Select(t => t.CLOBDATATYPE)
            .First();
          Assert.AreEqual(data, blob);
        } finally {
          conn.GetTable<ALLTYPE>().Delete(p => p.INTDATATYPE == 2000);
        }
      }
    }

    [Fact, DataContextSource(false)]
    public void TestTypes(string context) {
      dynamic int64Value = null;
      dynamic int32Value = null;
      dynamic int16Value = null;
      DB2iSeriesTools.AfterInitialized(() => {
        int64Value = DB2iSeriesTypes.BigInt.CreateInstance(1);
        int32Value = DB2iSeriesTypes.Integer.CreateInstance(2);
        int16Value = DB2iSeriesTypes.SmallInt.CreateInstance(3);
      });
      using (var conn = new DataConnection(context)) {
        conn.Select(() => 1);
        Assert.True(DB2iSeriesTypes.Clob.CreateInstance(conn) == null);
        Assert.True(DB2iSeriesTypes.Blob.CreateInstance(conn) == null);
      }
      Assert.Equal<long>(1, int64Value.Value);
      Assert.Equal<int>(2, int32Value.Value);
      Assert.Equal<short>(3, int16Value.Value);
      var decimalValue = DB2iSeriesTypes.Decimal.CreateInstance(4);
      var decimalValueAsDecimal = DB2iSeriesTypes.DecFloat16.CreateInstance(5m);
      var decimalValueAsDouble = DB2iSeriesTypes.DecFloat34.CreateInstance(6.0);
      var decimalValueAsLong = DB2iSeriesTypes.DecFloat34.CreateInstance(7);
      var realValue = DB2iSeriesTypes.Real.CreateInstance(8);
      var stringValue = DB2iSeriesTypes.VarChar.CreateInstance("1");
      var clobValue = DB2iSeriesTypes.Clob.CreateInstance("2");
      var binaryValue = DB2iSeriesTypes.Binary.CreateInstance(new byte[] { 1 });
      var blobValue = DB2iSeriesTypes.Blob.CreateInstance(new byte[] { 2 });
      var dateValue = DB2iSeriesTypes.Date.CreateInstance(new DateTime(2000, 1, 1));
      var timeValue = DB2iSeriesTypes.Time.CreateInstance(new DateTime(1, 1, 1, 1, 1, 1));
      var timeStampValue = DB2iSeriesTypes.TimeStamp.CreateInstance(new DateTime(2000, 1, 4));
      Assert.Equal<decimal>(4, decimalValue.Value);
      Assert.Equal<decimal>(5, decimalValueAsDecimal.Value);
      Assert.Equal<decimal>(6, decimalValueAsDouble.Value);
      Assert.Equal<decimal>(7, decimalValueAsLong.Value);
      Assert.Equal<float>(8, realValue.Value);
      Assert.Equal<string>("1", stringValue.Value);
      Assert.Equal<string>("2", clobValue.Value);
      Assert.Equal<byte[]>(new byte[] { 1 }, binaryValue.Value);
      Assert.Equal<byte[]>(new byte[] { 2 }, blobValue.Value);
      Assert.Equal<DateTime>(new DateTime(2000, 1, 1), dateValue.Value);
      Assert.Equal<DateTime>(new DateTime(1, 1, 1, 1, 1, 1), timeValue.Value);
      Assert.Equal<DateTime>(new DateTime(2000, 1, 4), timeStampValue.Value);
      DB2iSeriesTools.AfterInitialized(() => {
        int64Value = DB2iSeriesTypes.BigInt.CreateInstance();
        int32Value = DB2iSeriesTypes.Integer.CreateInstance();
        int16Value = DB2iSeriesTypes.SmallInt.CreateInstance();
      });
      Assert.True(int64Value.IsNull);
      Assert.True(int32Value.IsNull);
      Assert.True(int16Value.IsNull);
      Assert.True(DB2iSeriesTypes.Decimal.CreateInstance().IsNull);
      Assert.True(DB2iSeriesTypes.DecFloat16.CreateInstance().IsNull);
      Assert.True(DB2iSeriesTypes.DecFloat34.CreateInstance().IsNull);
      Assert.True(DB2iSeriesTypes.Real.CreateInstance().IsNull);
      Assert.True(DB2iSeriesTypes.VarChar.CreateInstance().IsNull);
      Assert.True(DB2iSeriesTypes.Binary.CreateInstance().IsNull);
      Assert.True(DB2iSeriesTypes.Date.CreateInstance().IsNull);
      Assert.True(DB2iSeriesTypes.Time.CreateInstance().IsNull);
      Assert.True(DB2iSeriesTypes.TimeStamp.CreateInstance().IsNull);
      Assert.True(DB2iSeriesTypes.RowId.CreateInstance().IsNull);
    }
    [Fact, DataContextSource(false)]
    public void TestAny(string context) {
      using (var conn = new DataConnection(context)) {
        var person = conn.GetTable<Person>();
        Assert.True(person.Any(p => p.ID == 2));
        Assert.False(person.Any(p => p.ID == 23));
        Assert.True(person.Any(p => !(p.ID == 23)));
      }
    }
    [Fact, DataContextSource(false)]
    public void TestOrderBySkipTake(string context) {
      using (var conn = new DataConnection(context)) {
        var person = conn.GetTable<Person>().OrderBy(p => p.LastName).Skip(2).Take(2);
        var results = person.ToArray();
        Assert.Equal(2, results.Count());
        Assert.Equal("Peacock", results.First().LastName);
        Assert.Equal("Plum", results.Last().LastName);
      }
    }
    [Fact, DataContextSource(false)]
    public void TestOrderByDescendingSkipTake(string context) {
      using (var conn = new DataConnection(context)) {
        var person = conn.GetTable<Person>().OrderByDescending(p => p.LastName).Skip(2).Take(2);
        var results = person.ToArray();
        Assert.Equal(2, results.Count());
        Assert.Equal("Scarlet", results.First().LastName);
        Assert.Equal("Pupkin", results.Last().LastName);
      }
    }
    [Fact, DataContextSource(false)]
    public void CompareDate1(string context) {
      using (var db = GetDataContext(context)) {
        var expected = Types.Where(t => t.ID == 1 && t.DateTimeValue <= DateTime.Today);
        var actual = db.Types.Where(t => t.ID == 1 && t.DateTimeValue <= DateTime.Today);
        Assert.AreEqual(expected, actual);
      }
    }
    [Fact, DataContextSource(false)]
    public void CompareDate2(string context) {
      var dt = Types2[3].DateTimeValue;
      using (var db = GetDataContext(context)) {
        var expected = Types2.Where(t => t.DateTimeValue.Value.Date > dt.Value.Date);
        var actual = db.Types2.Where(t => t.DateTimeValue.Value.Date > dt.Value.Date);
        Assert.Equal(expected, actual);
      }
    }
    [Table("InsertOrUpdateByte")]
    class MergeTypesByte {
      [Column("Id", IsIdentity = true)] [PrimaryKey] public int Id { get; set; }
      [Column("FieldByteAsDecimal", DataType = DataType.Decimal, Length = 2, Precision = 0)] public byte FieldByte { get; set; }
      [Column("FieldULongAsDecimal", DataType = DataType.Decimal, Length = 20, Precision = 0)] public ulong FieldULong { get; set; }
    }
    [Fact, DataContextSource(false)]
    public void InsertOrUpdateWithIntegers(string context) {
      using (var db = new TestDataConnection(context)) {
        LinqToDB.ITable<MergeTypesByte> table;
        using (new DisableLogging()) {
          db.DropTable<MergeTypesByte>(throwExceptionIfNotExists: false);
          table = db.CreateTable<MergeTypesByte>();
        }
        ulong val = long.MaxValue;
        table.InsertOrUpdate(
          () => new MergeTypesByte { FieldByte = 27, FieldULong = val },
          s => new MergeTypesByte { FieldByte = 27, FieldULong = val },
          () => new MergeTypesByte { FieldByte = 22, FieldULong = val }
      );
        Assert.Equal(1, table.Count());
      }
    }
  }
}