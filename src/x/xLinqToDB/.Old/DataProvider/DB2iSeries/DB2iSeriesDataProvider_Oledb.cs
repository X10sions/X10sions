using LinqToDB.Configuration;
using System;
using System.Data;
using System.Data.OleDb;

namespace LinqToDB.DataProvider.DB2iSeries{
  public class DB2iSeriesDataProvider_OleDb : GenericDataProvider <OleDbConnection, OleDbDataReader> {
    // https://www.ibm.com/support/knowledgecenter/en/ssw_ibm_i_71/rzaik/connectkeywords.htm
    public DB2iSeriesDataProvider_OleDb(string connectionString) : base(connectionString) { }
  }

  public static class DB2iSeriesDataProvider_OleDbExtensions {
    public static DataOptions UseDB2iSeries_OleDb(this DataOptions  builder, string connectionString) => builder.UseConnectionString(new DB2iSeriesDataProvider_OleDb(connectionString), connectionString);
    public static DataOptions UseDB2iSeries_OleDb(this DataOptions builder, IDbConnection connection, bool disposeConnection) => builder.UseConnection(new DB2iSeriesDataProvider_OleDb(connection.ConnectionString), connection, disposeConnection);
    public static DataOptions UseDB2iSeries_OleDb(this DataOptions builder, Func<IDbConnection> connectionFactory ) => builder.UseConnectionFactory(new DB2iSeriesDataProvider_OleDb(connectionFactory().ConnectionString), connectionFactory);
  }

}