using System.Runtime.Serialization;

namespace Common.Data;

public enum DbProviderNames {
  [EnumMember(Value = "Devart.Data.MySql")] Devart_Data_MySql,
  [EnumMember(Value = "Devart.Data.Oracle")] Devart_Data_Oracle,
  [EnumMember(Value = "Devart.Data.PostgreSql")] Devart_Data_PostgreSql,
  [EnumMember(Value = "FirebirdSql.Data.FirebirdClient")] FirebirdSql_Data_FirebirdClient,
  [EnumMember(Value = "IBM.Data.DB2")] IBM_Data_DB2,
  [EnumMember(Value = "IBM.Data.DB2.Core")] IBM_Data_DB2_Core,
  [EnumMember(Value = "IBM.Data.DB2.iSeries")] IBM_Data_DB2_iSeries,
  [EnumMember(Value = "Microsoft.Data.SqlClient")] Microsoft_Data_SqlClient,
  [EnumMember(Value = "Microsoft.Data.Sqlite")] Microsoft_Data_Sqlite,
  [EnumMember(Value = "MongoDB.Driver")] MongoDB_Driver,
  [EnumMember(Value = "MySql.Data.MySqlClient")] MySql_Data_MySqlClient,
  [EnumMember(Value = "MySqlConnector")] MySqlConnector,
  [EnumMember(Value = "Npgsql")] Npgsql,
  [EnumMember(Value = "Oracle.ManagedDataAccess.Client")] Oracle_ManagedDataAccess_Client,
  [EnumMember(Value = "System.Data.Odbc")] System_Data_Odbc,
  [EnumMember(Value = "System.Data.OleDb")] System_Data_OleDb,
  [EnumMember(Value = "System.Data.OracleClient")] System_Data_OracleClient,
  [EnumMember(Value = "System.Data.SqlClient")] System_Data_SqlClient,
  [EnumMember(Value = "System.Data.SQLite")] System_Data_SQLite,
}