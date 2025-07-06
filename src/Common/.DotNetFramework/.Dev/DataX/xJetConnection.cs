using System;
using System.Data;
using System.Data.Common;
using System.Data.Jet;
using System.Data.OleDb;
using System.IO;
using System.Transactions;

namespace Common.DataX {

  public class xJetConnection : DbConnection, ICloneable {
    public xJetConnection() {
      _state = ConnectionState.Closed;
    }

    public xJetConnection(string connectionString) : this() {
      ConnectionString = connectionString;
    }

    ConnectionState _state;
    internal DbConnection InnerConnection { get; private set; }
    internal DbTransaction ActiveTransaction { get; set; }
    public bool IsEmpty { get; set; }

    protected override DbProviderFactory DbProviderFactory => JetProviderFactory.Instance;

    string _ConnectionString;
    public override string ConnectionString {
      get => _ConnectionString;
      set {
        if (State != 0) {
          throw new InvalidOperationException(Messages.CannotChangePropertyValueInThisConnectionState("ConnectionString", State));
        }
        _ConnectionString = value;
      }
    }

    public override int ConnectionTimeout => 0;
    public override string Database => string.Empty;
    public override string DataSource => new OleDbConnectionStringBuilder(_ConnectionString).DataSource;
    public override string ServerVersion {
      get {
        if (State != ConnectionState.Open) {
          throw new InvalidOperationException(Messages.CannotReadPropertyValueInThisConnectionState("ServerVersion", State));
        }
        return InnerConnection.ServerVersion;
      }
    }

    public override ConnectionState State => _state;

    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) {
      if (State != ConnectionState.Open) {
        throw new InvalidOperationException(Messages.CannotCallMethodInThisConnectionState("BeginDbTransaction", State));
      }
      if (ActiveTransaction != null) {
        throw new InvalidOperationException(Messages.UnsupportedParallelTransactions());
      }
      if (isolationLevel == System.Data.IsolationLevel.Serializable) {
        ActiveTransaction = new JetTransaction(InnerConnection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted), this);
      } else {
        ActiveTransaction = new JetTransaction(InnerConnection.BeginTransaction(isolationLevel), this);
      }
      return ActiveTransaction;
    }

    public override void ChangeDatabase(string databaseName) {
      if (State != ConnectionState.Open) {
        throw new InvalidOperationException(Messages.CannotCallMethodInThisConnectionState("ConnectionString", ConnectionState.Open, State));
      }
      throw new InvalidOperationException(Messages.MethodUnsupportedByJet("ChangeDatabase"));
    }

    public override void Close() {
      if (_state != 0) {
        if (ActiveTransaction != null) {
          ActiveTransaction.Rollback();
        }
        ActiveTransaction = null;
        _state = ConnectionState.Closed;
        if (InnerConnection != null) {
          InnerConnection.StateChange -= WrappedConnection_StateChange;
          InnerConnectionFactory.Instance.CloseConnection(_ConnectionString, InnerConnection);
        }
        InnerConnection = null;
        OnStateChange(new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Closed));
      }
    }

    protected override DbCommand CreateDbCommand() {
      DbCommand dbCommand = JetProviderFactory.Instance.CreateCommand();
      dbCommand.Connection = this;
      return dbCommand;
    }

    protected override void Dispose(bool disposing) {
      _ConnectionString = string.Empty;
      if (disposing) {
        Close();
      }
      base.Dispose(disposing);
    }

    public override void EnlistTransaction(Transaction transaction) {
      if (InnerConnection == null) {
        throw new InvalidOperationException(Messages.PropertyNotInitialized("Connection"));
      }
      InnerConnection.EnlistTransaction(transaction);
    }

    public override DataTable GetSchema(string collectionName) {
      if (State != ConnectionState.Open) {
        throw new InvalidOperationException(Messages.CannotCallMethodInThisConnectionState("GetSchema", State));
      }
      return InnerConnection.GetSchema(collectionName);
    }

    public override DataTable GetSchema() {
      if (State != ConnectionState.Open) {
        throw new InvalidOperationException(Messages.CannotCallMethodInThisConnectionState("GetSchema", State));
      }
      return InnerConnection.GetSchema();
    }

    public override DataTable GetSchema(string collectionName, string[] restrictionValues) {
      if (State != ConnectionState.Open) {
        throw new InvalidOperationException(Messages.CannotCallMethodInThisConnectionState("GetSchema", State));
      }
      return InnerConnection.GetSchema(collectionName, restrictionValues);
    }

    public override void Open() {
      if (!IsEmpty) {
        if (string.IsNullOrWhiteSpace(_ConnectionString)) {
          throw new InvalidOperationException(Messages.PropertyNotInitialized("ConnectionString"));
        }
        if (State != 0) {
          throw new InvalidOperationException(Messages.CannotCallMethodInThisConnectionState("Open", ConnectionState.Closed, State));
        }
        _state = ConnectionState.Open;
        InnerConnection = InnerConnectionFactory.Instance.OpenConnection(_ConnectionString);
        InnerConnection.StateChange += WrappedConnection_StateChange;
        OnStateChange(new StateChangeEventArgs(ConnectionState.Closed, ConnectionState.Open));
      }
    }

    private void WrappedConnection_StateChange(object sender, StateChangeEventArgs e) {
      OnStateChange(e);
    }

    public bool TableExists(string tableName) {
      var state = State;
      if (state == ConnectionState.Closed) {
        Open();
      }
      bool result;
      try {
        string format = "select count(*) from [{0}] where 1=2";
        CreateCommand(string.Format(format, tableName), null).ExecuteNonQuery();
        result = true;
      } catch {
        result = false;
      }
      if (state == ConnectionState.Closed) {
        Close();
      }
      return result;
    }

    public DbCommand CreateCommand(string commandText, int? commandTimeout = default(int?)) {
      if (string.IsNullOrEmpty(commandText)) {
        commandText = Environment.NewLine;
      }
      JetCommand jetCommand = new JetCommand(commandText, this);
      if (commandTimeout.HasValue) {
        jetCommand.CommandTimeout = commandTimeout.Value;
      }
      return jetCommand;
    }

    object ICloneable.Clone() {
      JetConnection jetConnection = new JetConnection();
      if (InnerConnection != null) {
        jetConnection.InnerConnection = InnerConnectionFactory.Instance.OpenConnection(_ConnectionString);
      }
      return jetConnection;
    }

    public static explicit operator OleDbConnection(JetConnection connection) {
      return (OleDbConnection)connection.InnerConnection;
    }

    public static void ClearPool(JetConnection connection) {
    }

    public static void ClearAllPools() {
      InnerConnectionFactory.Instance.ClearAllPools();
    }

    public void CreateEmptyDatabase() {
      AdoxWrapper.CreateEmptyDatabase(_ConnectionString);
    }

    public static void CreateEmptyDatabase(string connectionString) {
      AdoxWrapper.CreateEmptyDatabase(connectionString);
    }

    public static string GetConnectionString(string fileName) {
      return $"Provider={JetConfiguration.OleDbDefaultProvider};Data Source={fileName}";
    }

    public void DropDatabase(bool throwOnError = true) {
      DropDatabase(_ConnectionString, throwOnError);
    }

    public static void DropDatabase(string connectionString, bool throwOnError = true) {
      string text = JetStoreDatabaseHandling.ExtractFileNameFromConnectionString(connectionString);
      if (string.IsNullOrWhiteSpace(text)) {
        throw new Exception("Cannot retrieve file name from connection string");
      }
      JetStoreDatabaseHandling.DeleteFile(text.Trim(), throwOnError);
    }

    public bool DatabaseExists() {
      return DatabaseExists(_ConnectionString);
    }

    public static bool DatabaseExists(string connectionString) {
      string text = JetStoreDatabaseHandling.ExtractFileNameFromConnectionString(connectionString);
      if (string.IsNullOrWhiteSpace(text)) {
        throw new Exception("Cannot retrieve file name from connection string");
      }
      return File.Exists(text);
    }
  }
}