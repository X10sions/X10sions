using LinqToDB.Data;
using LinqToDB.DataProvider;
using System;
using Xunit.Abstractions;

namespace LinqToDB.Tests.Linq.DataProvider {
  public class DataSource {
    public DataSource(IDataProvider dataProvider, bool isOdbc, string connectionString) {
      //ConnectionString = AppSettings.GetConnectionString(connectionStringName);
      ConnectionString = connectionString;
      IsOdbc = isOdbc;
      DataProvider = dataProvider;
    }

    public string ConnectionString { get; }
    public IDataProvider DataProvider { get; }
    public bool IsOdbc { get; }
    //  public DataSourceInformation DataSourceInformation { get; private set; }

    public DataConnection GetDataConnection(ITestOutputHelper output) {
      DataConnection.TurnTraceSwitchOn();
      var dc = new DataConnection(DataProvider, ConnectionString) {
        OnTraceConnection = (e) => {
          output.WriteLine($"{DateTime.Now}: CommandText:{e.CommandText}, Step: {e.TraceInfoStep}, Time: {e.ExecutionTime}, Records: {e.RecordsAffected}, SqlText:{e.SqlText} ");
        }
      };
      dc.OnConnectionOpened += (dc, cn) => {
        //    DataSourceInformation = new DataSourceInformation((DbConnection)cn);
      };
      return dc;
    }
  }
}