using LinqToDB.Common;
using LinqToDB.Data;
using LinqToDB.SqlQuery;
using LinqToDB.Tests.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Xunit;

namespace LinqToDB.Tests {
  public static class _Extensions {

    public static string? GetFilePath(string basePath, string findFileName) {
      var fileName = Path.GetFullPath(Path.Combine(basePath, findFileName));

      var path = basePath;
      while (!File.Exists(fileName)) {
        Console.WriteLine($"File not found: {fileName}");

        path = Path.GetDirectoryName(path);

        if (path == null)
          return null;

        fileName = Path.GetFullPath(Path.Combine(path, findFileName));
      }

      Console.WriteLine($"Base path found: {fileName}");

      return fileName;
    }

    public static readonly List<string> Providers = new List<string>    {
      //ProviderName.SqlCe,
      ProviderName.Access,
      ProviderName.AccessOdbc,
      "AS400",
      //ProviderName.DB2,
      //ProviderName.InformixDB2,
      //ProviderName.SQLiteClassic,
      //TestProvName.SQLiteClassicMiniProfilerMapped,
      //TestProvName.SQLiteClassicMiniProfilerUnmapped,
      //ProviderName.SybaseManaged,
      //ProviderName.OracleManaged,
      //TestProvName.Oracle11Managed,
      //ProviderName.Firebird,
      //TestProvName.Firebird3,
      //ProviderName.SqlServer2008,
      //ProviderName.SqlServer2012,
      //ProviderName.SqlServer2014,
      //ProviderName.SqlServer2017,
      //ProviderName.SqlServer2000,
      //ProviderName.SqlServer2005,
      //TestProvName.SqlAzure,
      //ProviderName.PostgreSQL,
      //ProviderName.PostgreSQL92,
      //ProviderName.PostgreSQL93,
      //ProviderName.PostgreSQL95,
      //TestProvName.PostgreSQL10,
      //TestProvName.PostgreSQL11,
      //ProviderName.MySql,
      //ProviderName.MySqlConnector,
      //TestProvName.MySql55,
      //TestProvName.MariaDB,
      //ProviderName.SQLiteMS,
      //ProviderName.SapHanaNative,
      //ProviderName.SapHanaOdbc
    };

    public static Base.Tools.TempTable<T> Create<T>(this IDataContext db, string tableName) => new Base.Tools.TempTable<T>(db, tableName);
    public static Base.Tools.TempTable<T> Create<T>(this IDataContext db, IEnumerable<T> data, string tableName) {
      var table = new Base.Tools.TempTable<T>(db, tableName);
      table.Table.BulkCopy(data);
      return table;
    }

    public static Base.Tools.TempTable<T> CreateTempTable<T>(this IDataContext db, string tableName, string context) => db.Create<T>(GetTempTableName(tableName, context));

    static void TestNumeric<T>(this DataConnection conn, T expectedValue, DataType dataType, string skip, string[] sqlTypes, string sqlFormat, bool isOdbc) {
      var param = isOdbc ? "?" : "@p";

      var skipTypes = skip.Split(' ');
      if (expectedValue != null)
        foreach (var sqlType in sqlTypes.Except(skipTypes)) {
          var sqlValue = expectedValue is bool ? (bool)(object)expectedValue ? 1 : 0 : (object)expectedValue;
          var sql = string.Format(CultureInfo.InvariantCulture, sqlFormat, sqlType, sqlValue ?? "NULL");
          Debug.WriteLine(sql + " -> " + typeof(T));
          Assert.Equal(expectedValue, conn.Execute<T>(sql));
        }

      var querySql = $"SELECT {param}";
      Debug.WriteLine("{0} -> DataType.{1}", typeof(T), dataType);
      Assert.Equal(expectedValue, conn.Execute<T>(querySql, new DataParameter { Name = "p", DataType = dataType, Value = expectedValue }));
      //sert.Equal(expectedValue, conn.Execute<T>("SELECT @p FROM SYSIBM.SYSDUMMY1", new DataParameter { Name = "p", DataType = dataType, Value = expectedValue }));
      Debug.WriteLine("{0} -> auto", typeof(T));
      Assert.Equal(expectedValue, conn.Execute<T>(querySql, new DataParameter { Name = "p", Value = expectedValue }));
      //sert.Equal(expectedValue, conn.Execute<T>("SELECT @p FROM SYSIBM.SYSDUMMY1", new DataParameter { Name = "p", Value = expectedValue }));
      Debug.WriteLine("{0} -> new", typeof(T));
      Assert.Equal(expectedValue, conn.Execute<T>(querySql, new { p = expectedValue }));
      //sert.Equal(expectedValue, conn.Execute<T>("SELECT @p FROM SYSIBM.SYSDUMMY1", new { p = expectedValue }));

      // [IBM][DB2/LINUXX8664] SQL0418N  The statement was not processed because the statement contains an invalid use of one of the following:
      // an untyped parameter marker, the DEFAULT keyword, or a null value.
    }

    public static void TestNumeric_Access<T>(this DataConnection conn, T expectedValue, DataType dataType, string skip, bool isOdbc) => TestNumeric(conn, expectedValue, dataType, skip, SqlTypes_Access, SqlFormat_Access, isOdbc);

    public static void TestNumeric_AS400<T>(this DataConnection conn, T expectedValue, DataType dataType, string skip, bool isOdbc) => TestNumeric(conn, expectedValue, dataType, skip, SqlTypes_AS400, SqlFormat_AS400, isOdbc);

    static string[] SqlTypes_Access = new[] { "cbool", "cbyte", "clng", "cint", "ccur", "cdbl", "csng" };

    static string SqlFormat_Access = "SELECT {0}({1})";

    static string[] SqlTypes_AS400 = new[] { "bigint", "int", "smallint", "decimal(31)", "decfloat", "double", "real" };
    static string SqlFormat_AS400 = "SELECT Cast({0} as {1}) FROM SYSIBM.SYSDUMMY1";

    static void TestSimple<T>(this DataConnection conn, T expectedValue, DataType dataType, string[] sqlTypes, string sqlFormat, bool isOdbc) where T : struct {
      TestNumeric(conn, expectedValue, dataType, "", sqlTypes, sqlFormat, isOdbc);
      TestNumeric<T?>(conn, expectedValue, dataType, "", sqlTypes, sqlFormat, isOdbc);
      TestNumeric<T?>(conn, null, dataType, "", sqlTypes, sqlFormat, isOdbc);
    }

    public static void TestSimple_Access<T>(this DataConnection conn, T expectedValue, DataType dataType, bool isOdbc) where T : struct => TestSimple(conn, expectedValue, dataType, SqlTypes_Access, SqlFormat_Access, isOdbc);

    public static void TestSimple_AS400<T>(this DataConnection conn, T expectedValue, DataType dataType, bool isOdbc) where T : struct => TestSimple(conn, expectedValue, dataType, SqlTypes_AS400, SqlFormat_AS400, isOdbc);

    public static string GetTempTableName(string tableName, string context) {
      var finalTableName = tableName;
      switch (GetProviderName(context, out var _)) {
        case TestProvName.SqlAzure:
        case ProviderName.SqlServer:
        case ProviderName.SqlServer2000:
        case ProviderName.SqlServer2005:
        case ProviderName.SqlServer2008:
        case ProviderName.SqlServer2012:
        case ProviderName.SqlServer2014:
        case ProviderName.SqlServer2017: {
            if (!tableName.StartsWith("#"))
              finalTableName = "#" + tableName;
            break;
          }
        default:
          throw new NotImplementedException();
      }
      return finalTableName;
    }

    public static string GetProviderName(string context, out bool isLinqService) {
      isLinqService = context.EndsWith(".LinqService");
      return context.Replace(".LinqService", "");
    }

    static string? NormalizeParameterName(string? name) {
      if (string.IsNullOrEmpty(name))
        return name;
      name = name!.Replace(' ', '_');
      return name;
    }

    static HashSet<string>? _aliases;

    public static void PrepareQueryAndAliases(this SqlSelectStatement sqlSelectStatement) {
      _aliases = null;

      var allAliases = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
      var paramsVisited = new HashSet<SqlParameter>();
      var tablesVisited = new HashSet<SqlTableSource>();

      new QueryVisitor().VisitAll(sqlSelectStatement, expr => {
        switch (expr.ElementType) {
          case QueryElementType.MergeSourceTable: {
              var source = (SqlMergeSourceTable)expr;

              Utils.MakeUniqueNames(
                source.SourceFields,
                null,
                (n, a) => !ReservedWords.IsReserved(n),
                f => f.PhysicalName,
                (f, n, a) => { f.PhysicalName = n; },
                f => {
                  var a = f.PhysicalName;
                  return a.IsNullOrEmpty()
                    ? "c1"
                    : a + (a!.EndsWith("_") ? string.Empty : "_") + "1";
                },
                StringComparer.OrdinalIgnoreCase);

              // copy aliases to source query fields
              if (source.SourceQuery != null)
                for (var i = 0; i < source.SourceFields.Count; i++)
                  source.SourceQuery.Select.Columns[i].Alias = source.SourceFields[i].PhysicalName;

              break;
            }
          case QueryElementType.SqlQuery: {
              var query = (SelectQuery)expr;
              if (query.IsParameterDependent)
                sqlSelectStatement.IsParameterDependent = true;
              if (query.Select.Columns.Count > 0) {
                var isRootQuery = query.ParentSelect == null;
                Utils.MakeUniqueNames(
                  query.Select.Columns.Where(c => c.Alias != "*"),
                  isRootQuery ? allAliases : null,
                  (n, a) => !ReservedWords.IsReserved(n),
                  c => c.Alias,
                  (c, n, a) => {
                    a?.Add(n);
                    c.Alias = n;
                  },
                  c => {
                    var a = c.Alias;
                    return a.IsNullOrEmpty()
                      ? "c1"
                      : a + (a!.EndsWith("_") ? string.Empty : "_") + "1";
                  },
                  StringComparer.OrdinalIgnoreCase);

                if (query.HasSetOperators) {
                  for (var i = 0; i < query.Select.Columns.Count; i++) {
                    var col = query.Select.Columns[i];

                    foreach (var t in query.SetOperators) {
                      var union = t.SelectQuery.Select;
                      union.Columns[i].Alias = col.Alias;
                    }
                  }
                }
              }

              break;
            }
          case QueryElementType.SqlParameter: {
              var p = (SqlParameter)expr;

              if (paramsVisited.Add(p) && !p.IsQueryParameter)
                sqlSelectStatement.IsParameterDependent = true;

              p.Name = NormalizeParameterName(p.Name);

              break;
            }
          case QueryElementType.TableSource: {
              var table = (SqlTableSource)expr;
              if (tablesVisited.Add(table)) {
                if (table.Source is SqlTable sqlTable)
                  allAliases.Add(sqlTable.PhysicalName!);
              }
              break;
            }
        }
      });

      Utils.MakeUniqueNames(tablesVisited,
        allAliases,
        (n, a) => !a!.Contains(n) && !ReservedWords.IsReserved(n), ts => ts.Alias, (ts, n, a) => {
          a!.Add(n);
          ts.Alias = n;
        },
        ts => {
          var a = ts.Alias;
          return a.IsNullOrEmpty() ? "t1" : a + (a!.EndsWith("_") ? string.Empty : "_") + "1";
        },
        StringComparer.OrdinalIgnoreCase);

      Utils.MakeUniqueNames(
        paramsVisited,
        allAliases,
        (n, a) => !a!.Contains(n) && !ReservedWords.IsReserved(n), p => p.Name, (p, n, a) => {
          a!.Add(n);
          p.Name = n;
        },
        p => p.Name.IsNullOrEmpty() ? "p1" : p.Name + "_1",
        StringComparer.OrdinalIgnoreCase);

      _aliases = allAliases;
    }



  }
}