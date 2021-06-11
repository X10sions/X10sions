using LinqToDB.Data.RetryPolicy;
using LinqToDB.DataProvider;
using LinqToDB.Mapping;
using System;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace LinqToDB.Data {
  public interface IDataConnection : ICloneable, IDataContext, IEntityServices, IDisposable {
    event Action<DataConnection, IDbConnection> OnBeforeConnectionOpen;
    event Func<DataConnection, IDbConnection, CancellationToken, Task> OnBeforeConnectionOpenAsync;
    event EventHandler OnClosed;
    event Action<DataConnection, IDbConnection> OnConnectionOpened;
    event Func<DataConnection, IDbConnection, CancellationToken, Task> OnConnectionOpenedAsync;

    IDbCommand Command { get; set; }
    int CommandTimeout { get; set; }
    string ConfigurationString { get; }
    IDbConnection Connection { get; }
    string ConnectionString { get; }
    IDataProvider DataProvider { get; }
    //bool Disposed { get; }
    int ID { get; }
    bool IsMarsEnabled { get; set; }
    Action<TraceInfo> OnTraceConnection { get; set; }
    IRetryPolicy RetryPolicy { get; set; }
    bool? ThrowOnDisposed { get; set; }
    IDbTransaction Transaction { get; }
    DataConnection AddMappingSchema(MappingSchema mappingSchema);
    DataConnectionTransaction BeginTransaction(IsolationLevel isolationLevel);
    DataConnectionTransaction BeginTransaction();
    Task<DataConnectionTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task<DataConnectionTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default);
    //void CheckAndThrowOnDisposed();
    Task CloseAsync(CancellationToken cancellationToken = default);
    void CommitTransaction();
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    IDbCommand CreateCommand();
    Task DisposeAsync(CancellationToken cancellationToken = default);
    void DisposeCommand();
    Task EnsureConnectionAsync(CancellationToken cancellationToken = default);
    ITable<T> GetTable<T>(object instance, MethodInfo methodInfo, params object[] parameters) where T : class;
    ITable<T> GetTable<T>() where T : class;
    //SqlStatement ProcessQuery(SqlStatement statement);
    void RollbackTransaction();
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
  }
}
