using LinqToDB.Configuration;
using System;
using System.Data;
using System.Data.Odbc;

namespace LinqToDB.DataProvider.DB2iSeries {
  public class DB2iSeriesDataProvider_Odbc : GenericDataProvider<OdbcConnection, OdbcDataReader> {
    // https://www.ibm.com/support/knowledgecenter/en/ssw_ibm_i_71/rzaik/connectkeywords.htm
    public DB2iSeriesDataProvider_Odbc(string connectionString) : base(connectionString) { }
  }

  public static class DB2iSeriesDataProvider_OdbcExtensions {
    public static LinqToDbConnectionOptionsBuilder UseDB2iSeries_Odbc(this LinqToDbConnectionOptionsBuilder builder, string connectionString) => builder.UseConnectionString(new DB2iSeriesDataProvider_Odbc(connectionString), connectionString);
    public static LinqToDbConnectionOptionsBuilder UseDB2iSeries_Odbc(this LinqToDbConnectionOptionsBuilder builder, IDbConnection connection, bool disposeConnection ) => builder.UseConnection(new DB2iSeriesDataProvider_Odbc(connection.ConnectionString), connection, disposeConnection);
    public static LinqToDbConnectionOptionsBuilder UseDB2iSeries_Odbc(this LinqToDbConnectionOptionsBuilder builder, Func<IDbConnection> connectionFactory) => builder.UseConnectionFactory(new DB2iSeriesDataProvider_Odbc(connectionFactory().ConnectionString), connectionFactory);
  }

}