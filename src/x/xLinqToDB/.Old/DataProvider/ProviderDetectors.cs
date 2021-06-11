using LinqToDB.Common;
using LinqToDB.Configuration;
using LinqToDB.Data;
using LinqToDB.DataProvider.Access;
using LinqToDB.DataProvider.DB2;
using LinqToDB.DataProvider.Firebird;
using LinqToDB.DataProvider.Informix;
using LinqToDB.DataProvider.MySql;
using LinqToDB.DataProvider.Oracle;
using LinqToDB.DataProvider.PostgreSQL;
using LinqToDB.DataProvider.SapHana;
using LinqToDB.DataProvider.SqlCe;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.DataProvider.SqlServer;
using LinqToDB.DataProvider.Sybase;
using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.IO;

namespace LinqToDB.DataProvider {

  public partial class ProviderDetectors {

    //public static (Type dataReaderType, string connectionNamespace) GetDataProviderInfo<T>() where T : IDbConnection => GetDataProviderInfo(typeof(T));
    //public static (Type dataReaderType, string connectionNamespace) GetDataProviderInfo(Type connectionType) => connectionType switch {

    //  _ => throw new NotImplementedException()
    //};

    public static Type GetDataReaderType<T>() where T : IDbConnection => GetDataReaderType(typeof(T));
    public static Type GetDataReaderType(Type connectionType) => DataReaderTypeDictionary.Instance[connectionType];

    //  public static Type GetDataReaderType<T>(T connection) where T : IDbConnection => connection switch {
    //    OdbcConnection => typeof(OdbcDataReader),
    //    OleDbConnection => typeof(OleDbDataReader),
    //    _ => throw new NotImplementedException()
    //  };

    //  public static Type GetDataReaderType<T>() => typeof(T) switch {

    //  odbc => typeof(System.Data.Odbc.d OdbcDatare)
    //    };

    //public static Type GetDataReaderType(Type connectionType) => connectionTypeName.ToLower() switch {
    //"odbc" => typeof(System.Data.Odbc.d OdbcDatare)
    //    }

    /*
      AddProviderDetector(LinqToDB.DataProvider.Access    .AccessTools    .ProviderDetector);
      AddProviderDetector(LinqToDB.DataProvider.DB2       .DB2Tools       .ProviderDetector);
      AddProviderDetector(LinqToDB.DataProvider.Firebird  .FirebirdTools  .ProviderDetector);
      AddProviderDetector(LinqToDB.DataProvider.Informix  .InformixTools  .ProviderDetector);
      AddProviderDetector(LinqToDB.DataProvider.MySql     .MySqlTools     .ProviderDetector);
      AddProviderDetector(LinqToDB.DataProvider.Oracle    .OracleTools    .ProviderDetector);
      AddProviderDetector(LinqToDB.DataProvider.PostgreSQL.PostgreSQLTools.ProviderDetector);
      AddProviderDetector(LinqToDB.DataProvider.SapHana   .SapHanaTools   .ProviderDetector);
      AddProviderDetector(LinqToDB.DataProvider.SqlCe     .SqlCeTools     .ProviderDetector);
      AddProviderDetector(LinqToDB.DataProvider.SQLite    .SQLiteTools    .ProviderDetector);
      AddProviderDetector(LinqToDB.DataProvider.SqlServer .SqlServerTools .ProviderDetector);
      AddProviderDetector(LinqToDB.DataProvider.Sybase    .SybaseTools    .ProviderDetector);
     */

    public static IDataProvider? GetDataProvider<T>(T connection) where T : IDbConnection => GetDataProvider<T>(connection.ConnectionString);
    public static IDataProvider? GetDataProvider<T>(string connectionString) where T : IDbConnection => GetDataProvider(typeof(T), connectionString);
    public static IDataProvider? GetDataProvider(Type connectionType, string connectionString) => GetDataProvider(connectionType, new DbConnectionStringBuilder { ConnectionString = connectionString });
    public static IDataProvider? GetDataProvider(Type connectionType, DbConnectionStringBuilder connectionStringBuilder) {
      IDataProvider? dataProvider = null;
      if (connectionType == typeof(OdbcConnection)) {
        var driver = connectionStringBuilder["driver"].ToString();
        dataProvider = OdbcConnectionDriverDictionary.Instance[driver]();
      } else if (connectionType == typeof(OleDbConnection)) {
        var provider = connectionStringBuilder["provider"].ToString();
        dataProvider = OleDbConnectionProviderDictionary.Instance[provider]();
      } else {
        dataProvider = ConnectionTypeDictionary.Instance[connectionType]();
      }
      return dataProvider;
    }


    private static readonly Lazy<IDataProvider> _accessOleDbDataProvider = new Lazy<IDataProvider>(() => {
      var provider = new AccessOleDbDataProvider();
      DataConnection.AddDataProvider(provider);
      return provider;
    }, true);

    private static readonly Lazy<IDataProvider> _accessODBCDataProvider = new Lazy<IDataProvider>(() => {
      var provider = new AccessODBCDataProvider();
      DataConnection.AddDataProvider(provider);
      return provider;
    }, true);

    private static readonly Lazy<IDataProvider> _db2DataProviderzOS = new Lazy<IDataProvider>(() => {
      var provider = new DB2DataProvider(ProviderName.DB2zOS, DB2Version.zOS);
      DataConnection.AddDataProvider(provider);
      return provider;
    }, true);

    private static readonly Lazy<IDataProvider> _db2DataProviderLUW = new Lazy<IDataProvider>(() => {
      var provider = new DB2DataProvider(ProviderName.DB2LUW, DB2Version.LUW);
      DataConnection.AddDataProvider(provider);
      return provider;
    }, true);

    private static readonly Lazy<IDataProvider> _firebirdDataProvider = new Lazy<IDataProvider>(() => {
      var provider = new FirebirdDataProvider();
      DataConnection.AddDataProvider(provider);
      return provider;
    }, true);

    private static readonly Lazy<IDataProvider> _hanaOdbcDataProvider = new Lazy<IDataProvider>(() => {
      var provider = new SapHanaOdbcDataProvider();
      DataConnection.AddDataProvider(provider);
      return provider;
    }, true);

    private static readonly Lazy<IDataProvider> _informixDataProvider = new Lazy<IDataProvider>(() => {
      var provider = new InformixDataProvider(ProviderName.Informix);
      DataConnection.AddDataProvider(provider);
      return provider;
    }, true);

    private static readonly Lazy<IDataProvider> _informixDB2DataProvider = new Lazy<IDataProvider>(() => {
      var provider = new InformixDataProvider(ProviderName.InformixDB2);
      DataConnection.AddDataProvider(provider);
      return provider;
    }, true);

    private static readonly Lazy<IDataProvider> _mySqlDataProvider = new Lazy<IDataProvider>(() => {
      var provider = new MySqlDataProvider(ProviderName.MySqlOfficial);
      DataConnection.AddDataProvider(provider);
      return provider;
    }, true);

    private static readonly Lazy<IDataProvider> _mySqlConnectorDataProvider = new Lazy<IDataProvider>(() => {
      var provider = new MySqlDataProvider(ProviderName.MySqlConnector);
      DataConnection.AddDataProvider(provider);
      return provider;
    }, true);

    private static readonly Lazy<IDataProvider> _oracleManagedDataProvider11 = new Lazy<IDataProvider>(() => {
      var provider = new OracleDataProvider(ProviderName.OracleManaged, OracleVersion.v11);
      DataConnection.AddDataProvider(provider);
      return provider;
    }, true);

    private static readonly Lazy<IDataProvider> _oracleManagedDataProvider12 = new Lazy<IDataProvider>(() => {
      var provider = new OracleDataProvider(ProviderName.OracleManaged, OracleVersion.v12);
      DataConnection.AddDataProvider(provider);
      return provider;
    }, true);

    private static readonly Lazy<IDataProvider> _postgreSQLDataProvider92 = new Lazy<IDataProvider>(() => {
      var provider = new PostgreSQLDataProvider(ProviderName.PostgreSQL92, PostgreSQLVersion.v92);
      DataConnection.AddDataProvider(provider);
      return provider;
    }, true);

    private static readonly Lazy<IDataProvider> _postgreSQLDataProvider93 = new Lazy<IDataProvider>(() => {
      var provider = new PostgreSQLDataProvider(ProviderName.PostgreSQL93, PostgreSQLVersion.v93);
      DataConnection.AddDataProvider(provider);
      return provider;
    }, true);

    private static readonly Lazy<IDataProvider> _postgreSQLDataProvider95 = new Lazy<IDataProvider>(() => {
      var provider = new PostgreSQLDataProvider(ProviderName.PostgreSQL95, PostgreSQLVersion.v95);
      DataConnection.AddDataProvider(provider);
      return provider;
    }, true);

    private static readonly Lazy<IDataProvider> _sqlCeDataProvider = new Lazy<IDataProvider>(() => {
      var provider = new SqlCeDataProvider();
      DataConnection.AddDataProvider(provider);
      return provider;
    }, true);

    private static readonly Lazy<IDataProvider> _SQLiteClassicDataProvider = new Lazy<IDataProvider>(() => {
      var provider = new SQLiteDataProvider(ProviderName.SQLiteClassic);
      DataConnection.AddDataProvider(provider);
      return provider;
    }, true);

    private static readonly Lazy<IDataProvider> _SQLiteMSDataProvider = new Lazy<IDataProvider>(() => {
      var provider = new SQLiteDataProvider(ProviderName.SQLiteMS);
      DataConnection.AddDataProvider(provider);
      return provider;
    }, true);


    private static readonly ConcurrentQueue<SqlServerDataProvider> _providers_SqlServer = new ConcurrentQueue<SqlServerDataProvider>();

    private static readonly Lazy<IDataProvider> _sqlServerDataProvider2000sdc = new Lazy<IDataProvider>(() => {
      var provider = new SqlServerDataProvider(ProviderName.SqlServer2000, SqlServerVersion.v2000, SqlServerProvider.SystemDataSqlClient);
      if (Provider_SqlServer == SqlServerProvider.SystemDataSqlClient)
        DataConnection.AddDataProvider(provider);
      _providers_SqlServer.Enqueue(provider);
      return provider;
    }, true);
    private static readonly Lazy<IDataProvider> _sqlServerDataProvider2005sdc = new Lazy<IDataProvider>(() => {
      var provider = new SqlServerDataProvider(ProviderName.SqlServer2005, SqlServerVersion.v2005, SqlServerProvider.SystemDataSqlClient);
      if (Provider_SqlServer == SqlServerProvider.SystemDataSqlClient)
        DataConnection.AddDataProvider(provider);
      _providers_SqlServer.Enqueue(provider);
      return provider;
    }, true);
    private static readonly Lazy<IDataProvider> _sqlServerDataProvider2008sdc = new Lazy<IDataProvider>(() => {
      var provider = new SqlServerDataProvider(ProviderName.SqlServer2008, SqlServerVersion.v2008, SqlServerProvider.SystemDataSqlClient);
      if (Provider_SqlServer == SqlServerProvider.SystemDataSqlClient) {
        DataConnection.AddDataProvider(provider);
      }
      _providers_SqlServer.Enqueue(provider);
      return provider;
    }, true);
    private static readonly Lazy<IDataProvider> _sqlServerDataProvider2012sdc = new Lazy<IDataProvider>(() => {
      var provider = new SqlServerDataProvider(ProviderName.SqlServer2012, SqlServerVersion.v2012, SqlServerProvider.SystemDataSqlClient);
      if (Provider_SqlServer == SqlServerProvider.SystemDataSqlClient) {
        DataConnection.AddDataProvider(ProviderName.SqlServer2014, provider);
        DataConnection.AddDataProvider(provider);
      }
      _providers_SqlServer.Enqueue(provider);
      return provider;
    }, true);
    private static readonly Lazy<IDataProvider> _sqlServerDataProvider2017sdc = new Lazy<IDataProvider>(() => {
      var provider = new SqlServerDataProvider(ProviderName.SqlServer2017, SqlServerVersion.v2017, SqlServerProvider.SystemDataSqlClient);
      if (Provider_SqlServer == SqlServerProvider.SystemDataSqlClient)
        DataConnection.AddDataProvider(provider);
      _providers_SqlServer.Enqueue(provider);
      return provider;
    }, true);

    // Microsoft.Data.SqlClient
    private static readonly Lazy<IDataProvider> _sqlServerDataProvider2000mdc = new Lazy<IDataProvider>(() => {
      var provider = new SqlServerDataProvider(ProviderName.SqlServer2000, SqlServerVersion.v2000, SqlServerProvider.MicrosoftDataSqlClient);
      if (Provider_SqlServer == SqlServerProvider.MicrosoftDataSqlClient)
        DataConnection.AddDataProvider(provider);
      _providers_SqlServer.Enqueue(provider);
      return provider;
    }, true);
    private static readonly Lazy<IDataProvider> _sqlServerDataProvider2005mdc = new Lazy<IDataProvider>(() => {
      var provider = new SqlServerDataProvider(ProviderName.SqlServer2005, SqlServerVersion.v2005, SqlServerProvider.MicrosoftDataSqlClient);
      if (Provider_SqlServer == SqlServerProvider.MicrosoftDataSqlClient)
        DataConnection.AddDataProvider(provider);
      _providers_SqlServer.Enqueue(provider);
      return provider;
    }, true);
    private static readonly Lazy<IDataProvider> _sqlServerDataProvider2008mdc = new Lazy<IDataProvider>(() => {
      var provider = new SqlServerDataProvider(ProviderName.SqlServer2008, SqlServerVersion.v2008, SqlServerProvider.MicrosoftDataSqlClient);
      if (Provider_SqlServer == SqlServerProvider.MicrosoftDataSqlClient) {
        DataConnection.AddDataProvider(provider);
      }
      _providers_SqlServer.Enqueue(provider);
      return provider;
    }, true);
    private static readonly Lazy<IDataProvider> _sqlServerDataProvider2012mdc = new Lazy<IDataProvider>(() => {
      var provider = new SqlServerDataProvider(ProviderName.SqlServer2012, SqlServerVersion.v2012, SqlServerProvider.MicrosoftDataSqlClient);
      if (Provider_SqlServer == SqlServerProvider.MicrosoftDataSqlClient) {
        DataConnection.AddDataProvider(ProviderName.SqlServer2014, provider);
        DataConnection.AddDataProvider(provider);
      }
      _providers_SqlServer.Enqueue(provider);
      return provider;
    }, true);
    private static readonly Lazy<IDataProvider> _sqlServerDataProvider2017mdc = new Lazy<IDataProvider>(() => {
      var provider = new SqlServerDataProvider(ProviderName.SqlServer2017, SqlServerVersion.v2017, SqlServerProvider.MicrosoftDataSqlClient);
      if (Provider_SqlServer == SqlServerProvider.MicrosoftDataSqlClient)
        DataConnection.AddDataProvider(provider);
      _providers_SqlServer.Enqueue(provider);
      return provider;
    }, true);

    private static readonly Lazy<IDataProvider> _sybaseManagedDataProvider = new Lazy<IDataProvider>(() => {
      var provider = new SybaseDataProvider(ProviderName.SybaseManaged);
      DataConnection.AddDataProvider(provider);
      return provider;
    }, true);

    public static bool AutoDetectProvider { get; set; } = true;

    internal static IDataProvider? ProviderDetector(IConnectionStringSettings css, string connectionString) {
      if (css.ProviderName == ProviderName.Firebird || css.ProviderName == FirebirdProviderAdapter.ClientNamespace || css.Name.Contains("Firebird")) {
        return _firebirdDataProvider.Value;
      }
      if (css.ProviderName?.Contains("SqlCe") == true
        || css.ProviderName?.Contains("SqlServerCe") == true
        || css.Name.Contains("SqlCe")
        || css.Name.Contains("SqlServerCe"))
        return _sqlCeDataProvider.Value;

      switch (css.ProviderName) {
        case "":
        case null:
        case DB2ProviderAdapter.NetFxClientNamespace:
        case DB2ProviderAdapter.CoreClientNamespace:
          // this check used by both Informix and DB2 providers to avoid conflicts
          if (css.Name.Contains("Informix")) {
#if NETFRAMEWORK
					return _informixDataProvider.Value;
#else
            return _informixDB2DataProvider.Value;
#endif
          }
          break;
        case ProviderName.Informix:
          if (css.Name.Contains("DB2"))
            return _informixDB2DataProvider.Value;
#if NETFRAMEWORK
					return _informixDataProvider.Value;
#else
          return _informixDB2DataProvider.Value;
#endif
        case ProviderName.InformixDB2:
          return _informixDB2DataProvider.Value;
#if NETFRAMEWORK
				case InformixProviderAdapter.IfxClientNamespace:
					return _informixDataProvider.Value;
#endif
      }

      if (connectionString.IndexOf("HDBODBC", StringComparison.InvariantCultureIgnoreCase) >= 0)
        return _hanaOdbcDataProvider.Value;

      switch (css.ProviderName) {
#if NETFRAMEWORK || NETCOREAPP
				case SapHanaProviderAdapter.ClientNamespace:
				case "Sap.Data.Hana.v4.5"                  :
				case "Sap.Data.Hana.Core"                  :
				case "Sap.Data.Hana.Core.v2.1"             :
				case ProviderName.SapHanaNative            : return _hanaDataProvider.Value;
#endif
        case ProviderName.SapHanaOdbc: return _hanaOdbcDataProvider.Value;
        case "":
        case null:
          if (css.Name.Contains("Hana"))
            goto case ProviderName.SapHana;
          break;
        case ProviderName.SapHana:
          if (css.Name.IndexOf("ODBC", StringComparison.InvariantCultureIgnoreCase) >= 0)
            return _hanaOdbcDataProvider.Value;

          return GetDataProvider_SapHana();
      }

      switch (css.ProviderName) {
        case ProviderName.PostgreSQL92: return _postgreSQLDataProvider92.Value;
        case ProviderName.PostgreSQL93: return _postgreSQLDataProvider93.Value;
        case ProviderName.PostgreSQL95: return _postgreSQLDataProvider95.Value;
        case "":
        case null:
          if (css.Name == "PostgreSQL")
            goto case "Npgsql";
          break;
        case NpgsqlProviderAdapter.ClientNamespace:
        case var providerName when providerName.Contains("PostgreSQL") || providerName.Contains(NpgsqlProviderAdapter.AssemblyName):
          if (css.Name.Contains("92") || css.Name.Contains("9.2"))
            return _postgreSQLDataProvider92.Value;

          if (css.Name.Contains("93") || css.Name.Contains("9.3") || css.Name.Contains("94") || css.Name.Contains("9.4"))
            return _postgreSQLDataProvider93.Value;

          if (css.Name.Contains("95") || css.Name.Contains("9.5") || css.Name.Contains("96") || css.Name.Contains("9.6"))
            return _postgreSQLDataProvider95.Value;

          if (AutoDetectProvider) {
            try {
              var cs = string.IsNullOrWhiteSpace(connectionString) ? css.ConnectionString : connectionString;

              using (var conn = NpgsqlProviderAdapter.GetInstance().CreateConnection(cs)) {
                conn.Open();

                var postgreSqlVersion = conn.PostgreSqlVersion;

                if (postgreSqlVersion.Major > 9 || postgreSqlVersion.Major == 9 && postgreSqlVersion.Minor > 4)
                  return _postgreSQLDataProvider95.Value;

                if (postgreSqlVersion.Major == 9 && postgreSqlVersion.Minor > 2)
                  return _postgreSQLDataProvider93.Value;

                return _postgreSQLDataProvider92.Value;
              }
            } catch {
              return _postgreSQLDataProvider92.Value;
            }
          }

          return GetDataProvider_PostgreSQL();
      }

      switch (css.ProviderName) {
        case SybaseProviderAdapter.ManagedClientNamespace:
        case ProviderName.SybaseManaged: return _sybaseManagedDataProvider.Value;
#if NETFRAMEWORK
				case "Sybase.Native"                             :
				case SybaseProviderAdapter.NativeClientNamespace :
				case SybaseProviderAdapter.NativeAssemblyName    : return _sybaseNativeDataProvider.Value;
#endif
        case "":
        case null:
          if (css.Name.Contains("Sybase"))
            goto case ProviderName.Sybase;
          break;
        case ProviderName.Sybase:
          if (css.Name.Contains("Managed"))
            return _sybaseManagedDataProvider.Value;
#if NETFRAMEWORK
					if (css.Name.Contains("Native"))
						return _sybaseNativeDataProvider.Value;
#endif
          return GetDataProvider_Sybase();
      }

      if (css.IsGlobal)
        return null;

      switch (css.ProviderName) {
        case ProviderName.MySqlOfficial:
        case MySqlProviderAdapter.MySqlDataAssemblyName: return _mySqlDataProvider.Value;
        case ProviderName.MySqlConnector: return _mySqlConnectorDataProvider.Value;

        case "":
        case null:
          if (css.Name.Contains("MySql")) {
            if (css.Name.Contains(MySqlProviderAdapter.MySqlConnectorAssemblyName))
              return _mySqlConnectorDataProvider.Value;

            if (css.Name.Contains(MySqlProviderAdapter.MySqlDataAssemblyName))
              return _mySqlDataProvider.Value;
          }
          break;
        case MySqlProviderAdapter.MySqlDataClientNamespace:
        case ProviderName.MySql:
          if (css.Name.Contains(MySqlProviderAdapter.MySqlConnectorAssemblyName))
            return _mySqlConnectorDataProvider.Value;

          if (css.Name.Contains(MySqlProviderAdapter.MySqlDataAssemblyName))
            return _mySqlDataProvider.Value;

          return GetDataProvider_MySql();
        case var providerName when providerName.Contains("MySql"):
          if (providerName.Contains(MySqlProviderAdapter.MySqlConnectorAssemblyName))
            return _mySqlConnectorDataProvider.Value;

          if (providerName.Contains(MySqlProviderAdapter.MySqlDataAssemblyName))
            return _mySqlDataProvider.Value;

          goto case ProviderName.MySql;
      }

      // DB2 ODS provider could be used by informix
      if (css.Name.Contains("Informix"))
        return null;

      switch (css.ProviderName) {
        case ProviderName.DB2LUW: return _db2DataProviderLUW.Value;
        case ProviderName.DB2zOS: return _db2DataProviderzOS.Value;

        case "":
        case null:

          if (css.Name == "DB2")
            goto case ProviderName.DB2;
          break;

        case ProviderName.DB2:
        case DB2ProviderAdapter.NetFxClientNamespace:
        case DB2ProviderAdapter.CoreClientNamespace:

          if (css.Name.Contains("LUW"))
            return _db2DataProviderLUW.Value;
          if (css.Name.Contains("z/OS") || css.Name.Contains("zOS"))
            return _db2DataProviderzOS.Value;

          if (AutoDetectProvider) {
            try {
              var cs = string.IsNullOrWhiteSpace(connectionString) ? css.ConnectionString : connectionString;
              using (var conn = DB2ProviderAdapter.GetInstance().CreateConnection(cs)) {
                conn.Open();
                var iszOS = conn.eServerType == DB2ProviderAdapter.DB2ServerTypes.DB2_390;
                return iszOS ? _db2DataProviderzOS.Value : _db2DataProviderLUW.Value;
              }
            } catch {
            }
          }
          return GetDataProvider_DB2();
      }

      if (css.IsGlobal)
        return null;

      switch (css.ProviderName) {
        case SQLiteProviderAdapter.SystemDataSQLiteClientNamespace:
        case ProviderName.SQLiteClassic: return _SQLiteClassicDataProvider.Value;
        case SQLiteProviderAdapter.MicrosoftDataSQLiteClientNamespace:
        case "Microsoft.Data.SQLite":
        case ProviderName.SQLiteMS: return _SQLiteMSDataProvider.Value;
        case "":
        case null:
          if (css.Name.Contains("SQLite") || css.Name.Contains("Sqlite"))
            goto case ProviderName.SQLite;
          break;
        case ProviderName.SQLite:
          if (css.Name.Contains("MS") || css.Name.Contains("Microsoft"))
            return _SQLiteMSDataProvider.Value;

          if (css.Name.Contains("Classic"))
            return _SQLiteClassicDataProvider.Value;

          return GetDataProvider_SQLite();
        case var providerName when providerName.Contains("SQLite") || providerName.Contains("Sqlite"):
          if (css.ProviderName.Contains("MS") || css.ProviderName.Contains("Microsoft"))
            return _SQLiteMSDataProvider.Value;

          if (css.ProviderName.Contains("Classic"))
            return _SQLiteClassicDataProvider.Value;

          return GetDataProvider_SQLite();
      }

      if (connectionString.Contains("Microsoft.ACE.OLEDB")
        || connectionString.Contains("Microsoft.Jet.OLEDB")) {
        return _accessOleDbDataProvider.Value;
      }

      if (css.ProviderName == ProviderName.AccessOdbc
        || css.Name.Contains("Access.Odbc")) {
        return _accessODBCDataProvider.Value;
      }

      if (css.ProviderName == ProviderName.Access || css.Name.Contains("Access")) {
        if (connectionString.Contains("*.mdb")
          || connectionString.Contains("*.accdb"))
          return _accessODBCDataProvider.Value;

        return _accessOleDbDataProvider.Value;
      }

      bool? managed = null;
      switch (css.ProviderName) {
#if NETFRAMEWORK
				case OracleProviderAdapter.NativeAssemblyName    :
				case OracleProviderAdapter.NativeClientNamespace :
				case ProviderName.OracleNative                   :
					managed = false;
					goto case ProviderName.Oracle;
#endif
        case OracleProviderAdapter.ManagedAssemblyName:
        case OracleProviderAdapter.ManagedClientNamespace:
        case "Oracle.ManagedDataAccess.Core":
        case ProviderName.OracleManaged:
          managed = true;
          goto case ProviderName.Oracle;
        case "":
        case null:

          if (css.Name.Contains("Oracle"))
            goto case ProviderName.Oracle;
          break;
        case ProviderName.Oracle:
#if NETFRAMEWORK
					if (css.Name.Contains("Native") || managed == false)
					{
						if (css.Name.Contains("11"))
							return _oracleNativeDataProvider11.Value;
						if (css.Name.Contains("12"))
							return _oracleNativeDataProvider12.Value;
						return GetDataProvider(css, connectionString, false);
					}
#endif

          if (css.Name.Contains("Managed") || managed == true) {
            if (css.Name.Contains("11"))
              return _oracleManagedDataProvider11.Value;
            if (css.Name.Contains("12"))
              return _oracleManagedDataProvider12.Value;
            return GetDataProvider_Oracle(css, connectionString, true);
          }

          return GetDataProvider_Oracle();
      }
      var provider = Provider_SqlServer;
      if (css.ProviderName == SqlServerProviderAdapter.MicrosoftClientNamespace)
        provider = SqlServerProvider.MicrosoftDataSqlClient;
      else if (css.ProviderName == SqlServerProviderAdapter.SystemClientNamespace)
        provider = SqlServerProvider.SystemDataSqlClient;

      switch (css.ProviderName) {
        case "":
        case null:
          if (css.Name == "SqlServer")
            goto case ProviderName.SqlServer;
          break;
        // SqlClient use dot prefix, as SqlClient itself used by some other providers
        case var providerName when providerName.Contains("SqlServer") || providerName.Contains(".SqlClient"):
        case ProviderName.SqlServer:
          if (css.Name.Contains("2000") || css.ProviderName?.Contains("2000") == true) return GetDataProvider_SqlServer(SqlServerVersion.v2000, provider);
          if (css.Name.Contains("2005") || css.ProviderName?.Contains("2005") == true) return GetDataProvider_SqlServer(SqlServerVersion.v2005, provider);
          if (css.Name.Contains("2008") || css.ProviderName?.Contains("2008") == true) return GetDataProvider_SqlServer(SqlServerVersion.v2008, provider);
          if (css.Name.Contains("2012") || css.ProviderName?.Contains("2012") == true) return GetDataProvider_SqlServer(SqlServerVersion.v2012, provider);
          if (css.Name.Contains("2014") || css.ProviderName?.Contains("2014") == true) return GetDataProvider_SqlServer(SqlServerVersion.v2012, provider);
          if (css.Name.Contains("2016") || css.ProviderName?.Contains("2016") == true) return GetDataProvider_SqlServer(SqlServerVersion.v2012, provider);
          if (css.Name.Contains("2017") || css.ProviderName?.Contains("2017") == true) return GetDataProvider_SqlServer(SqlServerVersion.v2017, provider);
          if (css.Name.Contains("2019") || css.ProviderName?.Contains("2019") == true) return GetDataProvider_SqlServer(SqlServerVersion.v2017, provider);

          if (AutoDetectProvider) {
            try {
              var cs = string.IsNullOrWhiteSpace(connectionString) ? css.ConnectionString : connectionString;

              using (var conn = SqlServerProviderAdapter.GetInstance(provider).CreateConnection(cs)) {
                conn.Open();

                if (int.TryParse(conn.ServerVersion.Split('.')[0], out var version)) {
                  if (version <= 8)
                    return GetDataProvider_SqlServer(SqlServerVersion.v2000, provider);

                  using (var cmd = conn.CreateCommand()) {
                    cmd.CommandText = "SELECT compatibility_level FROM sys.databases WHERE name = db_name()";
                    var level = Converter.ChangeTypeTo<int>(cmd.ExecuteScalar());

                    if (level >= 140)
                      return GetDataProvider_SqlServer(SqlServerVersion.v2017, provider);
                    if (level >= 110)
                      return GetDataProvider_SqlServer(SqlServerVersion.v2012, provider);
                    if (level >= 100)
                      return GetDataProvider_SqlServer(SqlServerVersion.v2008, provider);
                    if (level >= 90)
                      return GetDataProvider_SqlServer(SqlServerVersion.v2005, provider);
                    if (level >= 80)
                      return GetDataProvider_SqlServer(SqlServerVersion.v2000, provider);

                    switch (version) {
                      case 8: return GetDataProvider_SqlServer(SqlServerVersion.v2000, provider);
                      case 9: return GetDataProvider_SqlServer(SqlServerVersion.v2005, provider);
                      case 10: return GetDataProvider_SqlServer(SqlServerVersion.v2008, provider);
                      case 11:
                      case 12:
                      case 13: return GetDataProvider_SqlServer(SqlServerVersion.v2012, provider);
                      case 14:
                      case 15: return GetDataProvider_SqlServer(SqlServerVersion.v2017, provider);
                      default:
                        if (version > 15)
                          return GetDataProvider_SqlServer(SqlServerVersion.v2017, provider);
                        return GetDataProvider_SqlServer(SqlServerVersion.v2008, provider);
                    }
                  }
                }
              }
            } catch {
            }
          }

          return GetDataProvider_SqlServer(provider: provider);
      }

      return null;
    }

    public static SqlServerProvider Provider_SqlServer = SqlServerProvider.SystemDataSqlClient;


    public static IDataProvider GetDataProvider_SqlServer(
      SqlServerVersion version = SqlServerVersion.v2008,
      SqlServerProvider provider = SqlServerProvider.SystemDataSqlClient) => provider switch {
        SqlServerProvider.SystemDataSqlClient => version switch {
          SqlServerVersion.v2000 => _sqlServerDataProvider2000sdc.Value,
          SqlServerVersion.v2005 => _sqlServerDataProvider2005sdc.Value,
          SqlServerVersion.v2012 => _sqlServerDataProvider2012sdc.Value,
          SqlServerVersion.v2017 => _sqlServerDataProvider2017sdc.Value,
          _ => _sqlServerDataProvider2008sdc.Value,
        },
        SqlServerProvider.MicrosoftDataSqlClient => version switch {
          SqlServerVersion.v2000 => _sqlServerDataProvider2000mdc.Value,
          SqlServerVersion.v2005 => _sqlServerDataProvider2005mdc.Value,
          SqlServerVersion.v2012 => _sqlServerDataProvider2012mdc.Value,
          SqlServerVersion.v2017 => _sqlServerDataProvider2017mdc.Value,
          _ => _sqlServerDataProvider2008mdc.Value,
        },
        _ => _sqlServerDataProvider2008sdc.Value,
      };

    private static IDataProvider GetVersionedDataProvider_Oracle(OracleVersion version, bool managed) =>
#if NETFRAMEWORK
			if (!managed)
			{
				return version switch
				{
					OracleVersion.v11 => _oracleNativeDataProvider11.Value,
					_                 => _oracleNativeDataProvider12.Value,
				};
			}
#endif
      version switch {
        OracleVersion.v11 => _oracleManagedDataProvider11.Value,
        _ => _oracleManagedDataProvider12.Value,
      };

    public static IDataProvider GetDataProvider_Oracle(string? providerName = null, string? assemblyName = null) =>
#if NETFRAMEWORK
			if (assemblyName == OracleProviderAdapter.NativeAssemblyName ) return GetVersionedDataProvider(DefaultVersion, false);
			if (assemblyName == OracleProviderAdapter.ManagedAssemblyName) return GetVersionedDataProvider(DefaultVersion, true);

			return providerName switch
			{
				ProviderName.OracleNative  => GetVersionedDataProvider(DefaultVersion, false),
				ProviderName.OracleManaged => GetVersionedDataProvider(DefaultVersion, true),
				_						   =>
					DetectedProviderName == ProviderName.OracleNative
					? GetVersionedDataProvider(DefaultVersion, false)
					: GetVersionedDataProvider(DefaultVersion, true),
			};
#else
      GetVersionedDataProvider_Oracle(DefaultVersion_Oracle, true);
#endif


    public static OracleVersion DefaultVersion_Oracle = OracleVersion.v12;

    private static IDataProvider GetDataProvider_Oracle(IConnectionStringSettings css, string connectionString, bool managed) {
      var version = DefaultVersion_Oracle;
      if (AutoDetectProvider)
        version = DetectProviderVersion_Oracle(css, connectionString, managed);

      return GetVersionedDataProvider_Oracle(version, managed);
    }

    private static OracleVersion DetectProviderVersion_Oracle(IConnectionStringSettings css, string connectionString, bool managed) {

      OracleProviderAdapter providerAdapter;
      try {
        var cs = string.IsNullOrWhiteSpace(connectionString) ? css.ConnectionString : connectionString;

#if NETFRAMEWORK
				if (!managed)
					providerAdapter = OracleProviderAdapter.GetInstance(ProviderName.OracleNative);
				else
#endif
        providerAdapter = OracleProviderAdapter.GetInstance(ProviderName.OracleManaged);

        using (var conn = providerAdapter.CreateConnection(cs)) {
          conn.Open();

          var command = conn.CreateCommand();
          command.CommandText =
            "select VERSION from PRODUCT_COMPONENT_VERSION where PRODUCT like 'PL/SQL%'";
          if (command.ExecuteScalar() is string result) {
            var version = int.Parse(result.Split('.')[0]);

            if (version <= 11)
              return OracleVersion.v11;

            return OracleVersion.v12;
          }
          return DefaultVersion_Oracle;
        }
      } catch {
        return DefaultVersion_Oracle;
      }
    }

    private static string? _detectedProviderName_SQLite;
    public static string DetectedProviderName_SQLite => _detectedProviderName_SQLite ??= DetectProviderName_SQLite();

    static string DetectProviderName_SQLite() {
      try {
        var path = typeof(SQLiteTools).Assembly.GetPath();
        if (!File.Exists(Path.Combine(path, $"{SQLiteProviderAdapter.SystemDataSQLiteAssemblyName}.dll")))
          if (File.Exists(Path.Combine(path, $"{SQLiteProviderAdapter.MicrosoftDataSQLiteAssemblyName}.dll")))
            return ProviderName.SQLiteMS;
      } catch {
      }

      return ProviderName.SQLiteClassic;
    }

    public static IDataProvider GetDataProvider_SQLite(string? providerName = null) {
      switch (providerName) {
        case ProviderName.SQLiteClassic: return _SQLiteClassicDataProvider.Value;
        case ProviderName.SQLiteMS: return _SQLiteMSDataProvider.Value;
      }
      if (DetectedProviderName_SQLite == ProviderName.SQLiteClassic) return _SQLiteClassicDataProvider.Value;
      return _SQLiteMSDataProvider.Value;
    }

    public static IDataProvider GetDataProvider_DB2(DB2Version version = DB2Version.LUW) => version == DB2Version.zOS ? _db2DataProviderzOS.Value : _db2DataProviderLUW.Value;

    public static IDataProvider GetDataProvider_Firebird() => _firebirdDataProvider.Value;

    public static IDataProvider GetDataProvider_Informix() => _firebirdDataProvider.Value;

    public static IDataProvider GetDataProvider_MySql(string? providerName = null) => providerName switch {
      ProviderName.MySqlOfficial => _mySqlDataProvider.Value,
      ProviderName.MySqlConnector => _mySqlConnectorDataProvider.Value,
      _ => DetectedProviderName_MySql == ProviderName.MySqlOfficial ? _mySqlDataProvider.Value : _mySqlConnectorDataProvider.Value,
    };

    private static string? _detectedProviderName_MySql;
    public static string DetectedProviderName_MySql => _detectedProviderName_MySql ??= DetectProviderName_MySql();

    static string DetectProviderName_MySql() {
      try {
        var path = typeof(MySqlTools).Assembly.GetPath();

        if (!File.Exists(Path.Combine(path, $"{MySqlProviderAdapter.MySqlDataAssemblyName}.dll")))
          if (File.Exists(Path.Combine(path, $"{MySqlProviderAdapter.MySqlConnectorAssemblyName}.dll")))
            return ProviderName.MySqlConnector;
      } catch (Exception) {
      }

      return ProviderName.MySqlOfficial;
    }

    public static IDataProvider GetDataProvider_Sybase(string? providerName = null, string? assemblyName = null) =>
#if NETFRAMEWORK
			if (assemblyName == SybaseProviderAdapter.NativeAssemblyName)  return _sybaseNativeDataProvider.Value;
			if (assemblyName == SybaseProviderAdapter.ManagedAssemblyName) return _sybaseManagedDataProvider.Value;

			switch (providerName)
			{
				case ProviderName.Sybase       : return _sybaseNativeDataProvider.Value;
				case ProviderName.SybaseManaged: return _sybaseManagedDataProvider.Value;
			}

			if (DetectedProviderName == ProviderName.Sybase)
				return _sybaseNativeDataProvider.Value;
#endif

      _sybaseManagedDataProvider.Value;

    public static IDataProvider GetDataProvider_PostgreSQL(PostgreSQLVersion version = PostgreSQLVersion.v92) => version switch {
      PostgreSQLVersion.v95 => _postgreSQLDataProvider95.Value,
      PostgreSQLVersion.v93 => _postgreSQLDataProvider93.Value,
      _ => _postgreSQLDataProvider92.Value,
    };

    public static IDataProvider GetDataProvider_SqlCe() => _sqlCeDataProvider.Value;

    public static IDataProvider GetDataProvider_SapHana(string? providerName = null, string? assemblyName = null) {
#if NETFRAMEWORK || NETCOREAPP
			if (assemblyName == SapHanaProviderAdapter.AssemblyName) return _hanaDataProvider.Value;
#endif
      if (assemblyName == OdbcProviderAdapter.AssemblyName) return _hanaOdbcDataProvider.Value;
      switch (providerName) {
        case ProviderName.SapHanaOdbc: return _hanaOdbcDataProvider.Value;
#if NETFRAMEWORK || NETCOREAPP
				case ProviderName.SapHanaNative: return _hanaDataProvider.Value;
#endif
      }

#if NETFRAMEWORK || NETCOREAPP
			if (DetectedProviderName == ProviderName.SapHanaNative)
				return _hanaDataProvider.Value;
#endif
      return _hanaOdbcDataProvider.Value;
    }


    #region Dictionaries

   

    #endregion
  }

}