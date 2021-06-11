using Common.Data.GetSchemaTyped.DataRows;
using System;
//using System.Data.Odbc;
using System.Collections.Generic;
//using System.Data.OleDb;
using System.Data.Common;
using System.Data;
using System.Linq;

namespace LinqToDB.DataProvider {

  public class xGenericDbInfoList : HashSet<GenericDbInfo> {

    public static xGenericDbInfoList Instance = new xGenericDbInfoList();

    //public static GenericDatabaseInformation? GetInstance(Type dbConnectionType, string dataSourceProductName) => Instance.FirstOrDefault(x => x.DbConnectionType == dbConnectionType && x.DataSourceProductName == dataSourceProductName);
    //public static GenericDatabaseInformation? GetInstance<TConnection>(DataSourceInformationRow? dataSourceInformationRow) where TConnection : DbConnection => GetInstance(typeof(TConnection), dataSourceInformationRow?.DataSourceProductName ?? string.Empty);
    //public static GenericDatabaseInformation? GetInstance<TConnection>(TConnection dbConnection) where TConnection : DbConnection => GetInstance<TConnection>(dbConnection?.GetSchemaHelper().DataSourceInformationRow());
    //public static GenericDatabaseInformation? GetInstance<TConnection>(string connectionString) where TConnection : DbConnection, new() => GetInstance(new TConnection { ConnectionString = connectionString });

    // https://linq2db.github.io/articles/general/databases.html

    xGenericDbInfoList() {
      //Add<OleDbConnection, OleDbDataReader>("DB2iSeries", "DB2 for IBM i");
      //Add<OdbcConnection, OdbcDataReader>("DB2iSeries", "DB2/400 SQL");
      //Add<IBM.Data.DB2.iSeries.iDB2Connection, IBM.Data.DB2.iSeries.iDB2DataReader>("DB2iSeries", "IBM DB2 for i");
      //Add<OleDbConnection, OleDbDataReader>("MsAccess", "MS Jet");

      /*
    public const string Access = "Access";
    public const string AccessOdbc = "Access.Odbc";
    public const string DB2 = "DB2";
    public const string DB2LUW = "DB2.LUW";
    public const string DB2zOS = "DB2.z/OS";
    public const string Firebird = "Firebird";
    public const string Informix = "Informix";
    public const string InformixDB2 = "Informix.DB2";
    public const string SqlServer = "SqlServer";
    public const string SqlServer2000 = "SqlServer.2000";
    public const string SqlServer2005 = "SqlServer.2005";
    public const string SqlServer2008 = "SqlServer.2008";
    public const string SqlServer2012 = "SqlServer.2012";
    public const string SqlServer2014 = "SqlServer.2014";
    public const string SqlServer2016 = "SqlServer.2016";
    public const string SqlServer2017 = "SqlServer.2017";
    public const string MySql = "MySql";
    public const string MySqlOfficial = "MySql.Official";
    public const string MySqlConnector = "MySqlConnector";
    public const string Oracle = "Oracle";
    public const string OracleNative = "Oracle.Native";
    public const string OracleManaged = "Oracle.Managed";
    public const string PostgreSQL = "PostgreSQL";
    public const string PostgreSQL92 = "PostgreSQL.9.2";
    public const string PostgreSQL93 = "PostgreSQL.9.3";
    public const string PostgreSQL95 = "PostgreSQL.9.5";
    public const string SqlCe = "SqlCe";
    public const string SQLite = "SQLite";
    public const string SQLiteClassic = "SQLite.Classic";
    public const string SQLiteMS = "SQLite.MS";
    public const string Sybase = "Sybase";
    public const string SybaseManaged = "Sybase.Managed";
    public const string SapHana = "SapHana";
    public const string SapHanaOdbc = "SapHana.Odbc";

#if NETFRAMEWORK || NETCOREAPP
    public const string SapHanaNative = "SapHana.Native";
#endif
       */
    }

    public void Add<TConnection, TDataReader>(string dbSystemName, string dataSourceProductName)
      where TConnection : DbConnection
      where TDataReader : IDataReader
      => Add(typeof(TConnection), typeof(TDataReader), dbSystemName, dataSourceProductName);

    public void Add(Type dbConnectionType, Type dataReaderType, string dbSystemName, string dataSourceProductName) {
      var info = new GenericDbInfo(dbSystemName, dataSourceProductName);
      info.GenericDbConnectionInfoList.Add(new GenericDbConnectionInfo(dbConnectionType, dataReaderType));
      Add(info);
    }
    
  }

  public class GenericDbInfo : IEquatable<GenericDbInfo> {
    public GenericDbInfo(string dbSystemName, string dataSourceProductName) {
      DbSystemName = dbSystemName;
      DataSourceProductName = dataSourceProductName;
    }
    public string DbSystemName { get; }
    public string DataSourceProductName { get; }
    public List<GenericDbConnectionInfo> GenericDbConnectionInfoList => new List<GenericDbConnectionInfo>();
    //public HashSet<Version> Versions => new HashSet<Version>();

    //public string GetProviderVersionName(Version version) => $"{DbSystemName}.v{version}";

    public bool Equals(GenericDbInfo? other) => other != null && DbSystemName.Equals(other.DbSystemName, StringComparison.OrdinalIgnoreCase) && DataSourceProductName.Equals(other.DataSourceProductName, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => Equals(obj as GenericDbInfo);
    public override int GetHashCode() => HashCode.Combine(DbSystemName, DataSourceProductName);
  }

  public class GenericDbConnectionInfo :IEquatable<GenericDbConnectionInfo> {
    public GenericDbConnectionInfo(Type dbConnectionType, Type dataReaderType) {
      DbConnectionType = dbConnectionType;
      DataReaderType = dataReaderType;
    }
    //public DataSourceInformation(string name, Type dbConnectionType, IEnumerable<Version> versions) : this(name, dbConnectionType) {
    //  Versions.AddRange(versions);
    //}
    //public string LinqToDBProviderName { get; set; }
    public Type DbConnectionType { get; }
    public Type DataReaderType { get; }

    public bool Equals(GenericDbConnectionInfo? other) => other != null && DbConnectionType.Equals(other.DbConnectionType) && DataReaderType.Equals(other.DataReaderType);
    public override bool Equals(object? obj) => Equals(obj as GenericDbConnectionInfo);
    public override int GetHashCode() => HashCode.Combine(DbConnectionType, DataReaderType);


  }

}