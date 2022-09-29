//using System;
//using System.Data;
//using System.Data.Common;
//using System.Data.Odbc;

//namespace Common.Data.Odbc {
//  public class _BaseOdbcConnection : DbConnection, ICloneable {

//    public _BaseOdbcConnection() { }

//    public _BaseOdbcConnection(string connectionString) {
//      ConnectionString = connectionString;
//    }

//    private _BaseOdbcConnectionStringBuilder _csb => new _BaseOdbcConnectionStringBuilder();
//    protected OdbcConnection _connection => new OdbcConnection();

//    #region DbConnection
//    public override string ConnectionString { get => _csb.ConnectionString; set => this.SetConnectionString(value); }

//    public override string Database => _connection.Database;
//    public override string DataSource => _connection.DataSource;
//    public override string ServerVersion => _connection.ServerVersion;
//    public override ConnectionState State => _connection.State;

//    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) => _connection.BeginTransaction(isolationLevel);
//    public override void ChangeDatabase(string databaseName) => _connection.ChangeDatabase(databaseName);
//    public override void Close() => _connection.Close();
//    protected override DbCommand CreateDbCommand() => _connection.CreateCommand();
//    public override void Open() => _connection.Open();
//    #endregion

//    #region ICloneable
//    private _BaseOdbcConnection(_BaseOdbcConnection connection) : this() { // Clone
//      _connection.ConnectionTimeout = connection.ConnectionTimeout;
//    }

//    public object Clone() {
//      var clone = new _BaseOdbcConnection(this);
//      return clone;
//    }
//    #endregion

//  }
//}


