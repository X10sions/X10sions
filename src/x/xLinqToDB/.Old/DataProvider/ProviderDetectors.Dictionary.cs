using AdoNetCore.AseClient;
using FirebirdSql.Data.FirebirdClient;
using IBM.Data.DB2.Core;
using LinqToDB.DataProvider.Access;
using LinqToDB.DataProvider.SqlServer;
using Microsoft.Data.Sqlite;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Data.SqlServerCe;

namespace LinqToDB.DataProvider {
  public partial class ProviderDetectors {

    public class DataReaderTypeDictionary : Dictionary<Type, Type> {
      public static DataReaderTypeDictionary Instance = new DataReaderTypeDictionary();

      DataReaderTypeDictionary() {
        Add(typeof(OdbcConnection), typeof(OdbcDataReader));
        Add(typeof(OleDbConnection), typeof(OleDbDataReader));
        Add(typeof(IBM.Data.DB2.Core.DB2Connection), typeof(DB2DataReader));
        Add(typeof(FbConnection), typeof(FbDataReader));
        //"IfxConnection" => (typeof(IfxDataReader));
        Add(typeof(global::MySql.Data.MySqlClient.MySqlConnection), typeof(global::MySql.Data.MySqlClient.MySqlDataReader));
        Add(typeof(MySqlConnector.MySqlConnection), typeof(MySqlConnector.MySqlDataReader));
        Add(typeof(OracleConnection), typeof(OracleDataReader));
        Add(typeof(NpgsqlConnection), typeof(NpgsqlDataReader));
        Add(typeof(SqliteConnection), typeof(SqliteDataReader));
        Add(typeof(SQLiteConnection), typeof(SQLiteDataReader));
        //"HanaConnection" ,typeof(HanaDataReader));
        Add(typeof(SqlCeConnection), typeof(SqlCeDataReader));
        Add(typeof(Microsoft.Data.SqlClient.SqlConnection), typeof(Microsoft.Data.SqlClient.SqlDataReader));
        Add(typeof(System.Data.SqlClient.SqlConnection), typeof(System.Data.SqlClient.SqlDataReader));
        Add(typeof(AseConnection), typeof(AseDataReader));
      }

    }

    public class ConnectionTypeDictionary : Dictionary<Type, Func<IDataProvider>> {
      public static ConnectionTypeDictionary Instance = new ConnectionTypeDictionary();

      ConnectionTypeDictionary() {

        Add(typeof(DB2Connection), () => GetDataProvider_DB2());
        Add(typeof(FbConnection), () => GetDataProvider_Firebird());
        //Add(  typeof(IfxConnection),() => (typeof(IfxDataReader), typeof(IfxConnection).Namespace));
        Add(typeof(global::MySql.Data.MySqlClient.MySqlConnection), () => GetDataProvider_MySql());
        Add(typeof(MySqlConnector.MySqlConnection), () => GetDataProvider_MySql());
        Add(typeof(OracleConnection), () => GetDataProvider_Oracle());
        Add(typeof(NpgsqlConnection), () => GetDataProvider_PostgreSQL());
        Add(typeof(SqliteConnection), () => GetDataProvider_SQLite());
        Add(typeof(SQLiteConnection), () => GetDataProvider_SQLite());
        //Add(       "HanaConnection" ,()=> GetDataProvider_SapHana() );
        Add(typeof(SqlCeConnection), () => GetDataProvider_SqlCe());
        Add(typeof(Microsoft.Data.SqlClient.SqlConnection), () => GetDataProvider_SqlServer(provider: SqlServerProvider.MicrosoftDataSqlClient));
        Add(typeof(System.Data.SqlClient.SqlConnection), () => GetDataProvider_SqlServer(provider: SqlServerProvider.SystemDataSqlClient));
        Add(typeof(AseConnection), () => GetDataProvider_Sybase());
      }
    }

    public class OdbcConnectionDriverDictionary : Dictionary<string, Func<IDataProvider>> {
      public static OdbcConnectionDriverDictionary Instance = new OdbcConnectionDriverDictionary();

      OdbcConnectionDriverDictionary() {
        Add("{Microsoft Access Driver (*.mdb, *.accdb)}", () => new AccessODBCDataProvider());
        Add("{Microsoft Access Driver (*.mdb)}", () => new AccessODBCDataProvider());

        Add("{IBM DB2 ODBC DRIVER}", () => GetDataProvider_DB2());
        Add("{Client Access ODBC Driver (32-bit)}", () => GetDataProvider_DB2());
        Add("{IBM i Access ODBC Driver}", () => GetDataProvider_DB2());
        Add("{iSeries Access ODBC Driver}", () => GetDataProvider_DB2());

        Add("Firebird/InterBase(r) driver", () => GetDataProvider_Firebird());

        Add("{Informix-CLI 2.5 (32 Bit)}", () => GetDataProvider_Informix());
        Add("{INFORMIX 3.30 32 BIT}", () => GetDataProvider_Informix());

        Add("{mySQL}", () => GetDataProvider_MySql());
        Add("{MySQL ODBC 3.51 Driver}", () => GetDataProvider_MySql());
        Add("{MySQL ODBC 5.1 Driver}", () => GetDataProvider_MySql());
        Add("{MySQL ODBC 5.2 ANSI Driver}", () => GetDataProvider_MySql());
        Add("{MySQL ODBC 5.2 UNICODE Driver}", () => GetDataProvider_MySql());
        Add("{MySQL ODBC 5.2w Driver}", () => GetDataProvider_MySql());

        Add("{Microsoft ODBC for Oracle}", () => GetDataProvider_Oracle());
        Add("{Microsoft ODBC Driver for Oracle}", () => GetDataProvider_Oracle());
        Add("{Oracle in OraClient11g_home1}", () => GetDataProvider_Oracle());
        Add("{Oracle in XEClient}", () => GetDataProvider_Oracle());
        Add("{Oracle in OraHome92}", () => GetDataProvider_Oracle());

        Add("{PostgreSQL}", () => GetDataProvider_Oracle());
        Add("{PostgreSQL ANSI}", () => GetDataProvider_Oracle());
        Add("{PostgreSQL UNICODE}", () => GetDataProvider_Oracle());

        Add("SQLite3 ODBC Driver", () => GetDataProvider_SQLite());

        Add("{SQL Server}", () => GetDataProvider_SqlServer());
        Add("{SQL Native Client}", () => GetDataProvider_SqlServer());
        Add("{SQL Server Native Client 10.0}", () => GetDataProvider_SqlServer());
        Add("{SQL Server Native Client 11.0}", () => GetDataProvider_SqlServer());
        Add("{ODBC Driver 11 for SQL Server}", () => GetDataProvider_SqlServer());
        Add("{ODBC Driver 13 for SQL Server}", () => GetDataProvider_SqlServer());
        Add("{ODBC Driver 17 for SQL Server}", () => GetDataProvider_SqlServer());

        //https://www.connectionstrings.com/sql-server/
        //https://www.connectionstrings.com/sybase-adaptive/   
      }
    }

    public class OleDbConnectionProviderDictionary : Dictionary<string, Func<IDataProvider>> {
      public static OleDbConnectionProviderDictionary Instance = new OleDbConnectionProviderDictionary();

      OleDbConnectionProviderDictionary() {
        Add("Microsoft.Jet.OLEDB.4.0;", () => new AccessOleDbDataProvider());
        Add("Microsoft.ACE.OLEDB.12.0", () => new AccessOleDbDataProvider());

        Add("DB2OLEDB", () => ProviderDetectors.GetDataProvider_DB2());
        Add("IBMDADB2", () => ProviderDetectors.GetDataProvider_DB2());

        Add("IBMDA400", () => ProviderDetectors.GetDataProvider_DB2());

        Add("Ifxoledbc", () => ProviderDetectors.GetDataProvider_Informix());

        Add("MySQLProv", () => ProviderDetectors.GetDataProvider_MySql());

        Add("OraOLEDB.Oracle", () => ProviderDetectors.GetDataProvider_Oracle());
        Add("msdaora", () => ProviderDetectors.GetDataProvider_Oracle());

        Add("PostgreSQL OLE DB Provider", () => ProviderDetectors.GetDataProvider_PostgreSQL());

        Add("Microsoft.SQLSERVER.MOBILE.OLEDB.3.0", () => ProviderDetectors.GetDataProvider_SqlCe());
        Add("Microsoft.SQLSERVER.CE.OLEDB.3.5", () => ProviderDetectors.GetDataProvider_SqlCe());

        Add("MSOLEDBSQL", () => ProviderDetectors.GetDataProvider_SqlServer());
        Add("SQLXMLOLEDB.3.0", () => ProviderDetectors.GetDataProvider_SqlServer(SqlServerVersion.v2000));
        Add("SQLXMLOLEDB.4.0", () => ProviderDetectors.GetDataProvider_SqlServer(SqlServerVersion.v2000));
        Add("sqloledb", () => ProviderDetectors.GetDataProvider_SqlServer(SqlServerVersion.v2000));
        Add("SQLNCLI", () => ProviderDetectors.GetDataProvider_SqlServer(SqlServerVersion.v2005));
        Add("SQLNCLI10", () => ProviderDetectors.GetDataProvider_SqlServer(SqlServerVersion.v2008));
        Add("SQLNCLI11", () => ProviderDetectors.GetDataProvider_SqlServer(SqlServerVersion.v2012));
        ////https://www.connectionstrings.com/sql-server/

        Add("Sybase ASE OLE DB Provider", () => ProviderDetectors.GetDataProvider_Sybase());
        Add("Sybase.ASEOLEDBProvider", () => ProviderDetectors.GetDataProvider_Sybase());
        Add("Sybase.ASEOLEDBProvider.2", () => ProviderDetectors.GetDataProvider_Sybase());
        Add("ASEOLEDB", () => ProviderDetectors.GetDataProvider_Sybase());
        Add("ASAProv", () => ProviderDetectors.GetDataProvider_Sybase());
        Add("ASAProv.90", () => ProviderDetectors.GetDataProvider_Sybase());
      }
    }

  }
}