using LinqToDB.Mapping;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace LinqToDB.Data;

public static class DataConnectionExtensions {

  public const string TempTableStatementHeaderFormat = "DECLARE GLOBAL TEMPORARY TABLE {0}";

  //public static DataConnection AddAssociation<T1, T2, T1Key, T2Key>(this DataConnection dataConnection
  //  , Expression<Func<T1, T2>> prop1
  //  , Expression<Func<T2, T1>> prop2
  //  , Expression<Func<T1, T1Key>> key1
  //  , Expression<Func<T2, T2Key>> key2
  //  ) {
  //  dataConnection.GetFluentMappingBuilder().AddAssociation(prop1, prop2, key1, key2);
  //  return dataConnection;
  //}


  //public static DataConnection AddAssociation<TOne, TMany, TOneKey, TManyKey>(this DataConnection dataConnection
  //  , Expression<Func<TOne, TMany>> prop1
  //  , Expression<Func<TMany, IEnumerable<TOne>>> prop2
  //  , Expression<Func<TOne, TOneKey>> key1
  //  , Expression<Func<TMany, TManyKey>> key2
  //  ) {
  //  dataConnection.GetFluentMappingBuilder().AddAssociation(prop1, prop2, key1, key2);
  //  return dataConnection;
  //}

  //public static DataConnection AddAssociation<TOne, TMany>(this DataConnection dataConnection
  //  , Expression<Func<TOne, TMany>> prop1
  //  , Expression<Func<TMany, IEnumerable<TOne>>> prop2
  //  , Expression<Func<TOne, TMany, bool>> predicate
  //  ) {
  //  dataConnection.MappingSchema.GetFluentMappingBuilder().AddAssociation(prop1, prop2, predicate);
  //  return dataConnection;
  //}

  //public static DataConnection AddAssociation<TOne1, TOne2>(this DataConnection dataConnection
  //  , Expression<Func<TOne1, TOne2>> prop1
  //  , Expression<Func<TOne2, TOne1>> prop2
  //  , Expression<Func<TOne1, TOne2, bool>> predicate
  //  ) {
  //  dataConnection.MappingSchema.GetFluentMappingBuilder().AddAssociation(prop1, prop2, predicate);
  //  return dataConnection;
  //}

  /// <summary>NotNull to NotNull </summary>
  public static FluentMappingBuilder AddAssociation00<T, TMany>(this FluentMappingBuilder fluentMappingBuilder
    , Expression<Func<T, TMany>> prop1
    , Expression<Func<TMany, IEnumerable<T>>> prop2
    , Expression<Func<T, TMany, bool>> predicate
    ) where T : class where TMany : class => fluentMappingBuilder.AddAssociationNotNullToNotNull(prop1, prop2, predicate);

  /// <summary>NotNull to NotNull </summary>
  public static FluentMappingBuilder AddAssociation00<T1, T2>(this FluentMappingBuilder fluentMappingBuilder
    , Expression<Func<T1, T2>> prop1
    , Expression<Func<T2, T1>> prop2
    , Expression<Func<T1, T2, bool>> predicate
    ) where T1 : class where T2 : class => fluentMappingBuilder.AddAssociationNotNullToNotNull(prop1, prop2, predicate);

  /// <summary>NotNull to Nullable</summary>
  public static FluentMappingBuilder AddAssociation01<T1, T2>(this FluentMappingBuilder fluentMappingBuilder
   , Expression<Func<T1, T2>> prop1
   , Expression<Func<T2, T1>> prop2
   , Expression<Func<T1, T2, bool>> predicate
   ) where T1 : class where T2 : class? => fluentMappingBuilder.AddAssociationNotNullToNullable(prop1, prop2, predicate);

  /// <summary>NotNull to Nullable</summary>
  public static FluentMappingBuilder AddAssociation01<T, TMany>(this FluentMappingBuilder fluentMappingBuilder
   , Expression<Func<T, TMany>> prop1
   , Expression<Func<TMany, IEnumerable<T>>> prop2
   , Expression<Func<T, TMany, bool>> predicate
   ) where T : class where TMany : class? => fluentMappingBuilder.AddAssociationNotNullToNullable(prop1, prop2, predicate);

  /// <summary>NUllable to NotNull </summary>
  public static FluentMappingBuilder AddAssociation10<T1, T2>(this FluentMappingBuilder fluentMappingBuilder
    , Expression<Func<T1, T2>> prop1
    , Expression<Func<T2, T1>> prop2
    , Expression<Func<T1, T2, bool>> predicate
    ) where T1 : class? where T2 : class => fluentMappingBuilder.AddAssociationNullableToNotNull(prop1, prop2, predicate);

  /// <summary>NUllable to NotNull </summary>
  public static FluentMappingBuilder AddAssociation10<T, TMany>(this FluentMappingBuilder fluentMappingBuilder
    , Expression<Func<T, TMany>> prop1
    , Expression<Func<TMany, IEnumerable<T>>> prop2
    , Expression<Func<T, TMany, bool>> predicate
    ) where T : class? where TMany : class => fluentMappingBuilder.AddAssociationNullableToNotNull(prop1, prop2, predicate);


  public static ITable<T> CreateTempTable<T>(this DataConnection dc, string tableName, bool withReplace, IEnumerable<T> data) where T : notnull    => dc.DeclareTempTable(tableName, withReplace, data);

  public static ITable<T> DeclareTempTable<T>(this DataConnection dc, string tableName, bool withReplace, IEnumerable<T> data) where T : notnull {
    var footer = (withReplace ? " WITH REPLACE" : "");
    var tempTable = dc.CreateTable<T>(tableName, null, null, TempTableStatementHeaderFormat, footer);
    if (data != null) {
      foreach (var row in data) {
        dc.Insert(row, tableName);
      }
      return tempTable;
    }
    return tempTable;
  }

  public static void DeclareTempTableFromSql(this DataConnection dc, string tableName, string selectSql, bool withData, bool withReplace) {
    var sql = string.Format("{0} As(\r\n{1}\r\n) {2} {3}", "DECLARE GLOBAL TEMPORARY TABLE " + tableName, selectSql, withData ? " With Data " : "", withReplace ? " With Replace " : "");
    dc.Execute(sql);
  }

  public static void DeclareTempNumbersTable(this DataConnection dc, string tableName, int min, int max, bool withData, bool withReplace) {
    var selectSql = $"Select A.Id + {min} AS Id From Table(Numbers({max} - {min} + 1)) As A";
    dc.DeclareTempTableFromSql(tableName, selectSql, withData, withReplace);
  }

  public static FluentMappingBuilder GetFluentMappingBuilder(this DataConnection dc) => new FluentMappingBuilder(dc.MappingSchema);

  public static ITable<T> GetTableWithPrimaryKey<T>(this DataConnection dataConnection, Expression<Func<T, object>> primaryKey, bool isPrimaryKeyIdentity)
    where T : class {
    var fmb = new FluentMappingBuilder(dataConnection.MappingSchema).Entity<T>().HasPrimaryKey(primaryKey);
    if (isPrimaryKeyIdentity) {
      fmb.HasIdentity(primaryKey);
    }
    fmb.Build();
    return dataConnection.GetTable<T>();
  }

  public static DataTable LoadDataTable(this DataConnection dc, string sql) => dc.OpenDbConnection().LoadDataTable(sql);

  public static void WriteToLog(this DataConnection dc, string logText, string sqlLogPath) {
    using var w = File.AppendText(sqlLogPath);
    try {
      //w.WriteLine(string.Format("-- {0} jobname:{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), iDb2Connection().JobName));
      w.WriteLine(logText);
    } catch (Exception ex2) {
      var ex = ex2;
      throw new Exception(logText + "\r\n" + ex.Message);
    }
  }

}