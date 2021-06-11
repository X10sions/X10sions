using IBM.Data.DB2.iSeries;
using LinqToDB.Configuration;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_2_3.TB {
  public static class DB2iSeriesTools_TB {
    static DB2iSeriesTools_TB() {
      DataConnection.AddProviderDetector(ProviderDetector);
    }
   

    private static readonly Lazy<IDataProvider> _db2iSeriesDataProviderV5R4_System = new Lazy<IDataProvider>(() => {
      var provider = new DB2iSeriesDataProvider_TB<iDB2Connection, IDataReader>(DB2iSeriesVersionRelease.V5R4, DB2iSeriesNamingConvention.System);
      DataConnection.AddDataProvider(provider);
      return provider;
    }, true);

    public static IDataProvider GetDataProvider(
      DB2iSeriesVersionRelease versionRelease= DB2iSeriesVersionRelease.V5R4,
      DB2iSeriesNamingConvention naming= DB2iSeriesNamingConvention.System) {
      return (versionRelease, naming) switch {
        (DB2iSeriesVersionRelease.V5R4, DB2iSeriesNamingConvention) => _db2iSeriesDataProviderV5R4_System .Value,
        //PostgreSQLVersion.v93 => _postgreSQLDataProvider93.Value,
        //_ => _postgreSQLDataProvider92.Value,
      };
    }

    public static bool AutoDetectProvider { get; set; } = true;

    private static IDataProvider ProviderDetector(IConnectionStringSettings css, string connectionString) {
      if (css.IsGlobal) {
        return null;
      }
      if ((css.Name.Equals(DB2iSeriesConstants.ProviderName, StringComparison.OrdinalIgnoreCase) || new string[] {
        DB2iSeriesConstants.ProviderName,
        "IBM.Data.DB2.iSeries"
      }.Contains(css.ProviderName)) && AutoDetectProvider) {
        try {
          using (var conn = Type.GetType("IBM.Data.DB2.iSeries.iDB2Connection, IBM.Data.DB2.iSeries", throwOnError: true).CreateConnectionExpression().Compile()(css.ConnectionString)) {
            conn.Open();
            return _db2iSeriesDataProviderV5R4_System.Value;
          }
        } catch (Exception ex) {
        }
      }
      return null;
    }


    public const iDB2NamingConvention DefaultNamingConvention = iDB2NamingConvention.System;
    //private static readonly DB2iSeriesDataProvider_TB _db2iSeriesDataProvider = new DB2iSeriesDataProvider_TB(DB2iSeriesVersionRelease.V5R4, DB2iSeriesNamingConvention.System,_connec);

    private static bool _isInitialized;
    private static readonly object _syncAfterInitialized = new object();
    private static ConcurrentBag<Action> _afterInitializedActions = new ConcurrentBag<Action>();
    public static BulkCopyType DefaultBulkCopyType = BulkCopyType.MultipleRows;


    public static void LogText(string className, string method, string msg) => File.AppendAllText("C:\\temp\\DB2iSeriesTools.log", $"{DateTime.Now}:{className}.{method}: {msg}");

    internal static void Initialized() {
      if (!_isInitialized) {
        var syncAfterInitialized = _syncAfterInitialized;
        var lockTaken = false;
        try {
          Monitor.Enter(syncAfterInitialized, ref lockTaken);
          if (!_isInitialized) {
            _isInitialized = true;
            foreach (var afterInitializedAction in _afterInitializedActions) {
              afterInitializedAction();
            }
            _afterInitializedActions = null;
          }
        } finally {
          if (lockTaken) {
            Monitor.Exit(syncAfterInitialized);
          }
        }
      }
    }

    public static void AfterInitialized(Action action) {
      if (_isInitialized) {
        action();
        return;
      }
      var syncAfterInitialized = _syncAfterInitialized;
      var lockTaken = false;
      try {
        Monitor.Enter(syncAfterInitialized, ref lockTaken);
        if (_isInitialized) {
          action();
        } else {
          _afterInitializedActions.Add(action);
        }
      } finally {
        if (lockTaken) {
          Monitor.Exit(syncAfterInitialized);
        }
      }
    }

    //public static DataConnection CreateDataConnection(string connectionString) => new DataConnection(_db2iSeriesDataProvider, connectionString);

    //public static DataConnection CreateDataConnection(IDbConnection connection) => new DataConnection(_db2iSeriesDataProvider, connection);

    //public static DataConnection CreateDataConnection(IDbTransaction transaction) => new DataConnection(_db2iSeriesDataProvider, transaction);

    public static BulkCopyRowsCopied MultipleRowsCopy<T>(DataConnection dataConnection, IEnumerable<T> source, int maxBatchSize = 1000, Action<BulkCopyRowsCopied> rowsCopiedCallback = null)
      where T : class => dataConnection.BulkCopy(new BulkCopyOptions {
        BulkCopyType = BulkCopyType.MultipleRows,
        MaxBatchSize = maxBatchSize,
        RowsCopiedCallback = rowsCopiedCallback
      }, source);

    public static BulkCopyRowsCopied ProviderSpecificBulkCopy<T>(DataConnection dataConnection, IEnumerable<T> source, int bulkCopyTimeout = 0, bool keepIdentity = false, int notifyAfter = 0, Action<BulkCopyRowsCopied> rowsCopiedCallback = null)
      where T : class => dataConnection.BulkCopy(new BulkCopyOptions {
        BulkCopyType = BulkCopyType.ProviderSpecific,
        BulkCopyTimeout = bulkCopyTimeout,
        KeepIdentity = keepIdentity,
        NotifyAfter = notifyAfter,
        RowsCopiedCallback = rowsCopiedCallback
      }, source);

  }
}