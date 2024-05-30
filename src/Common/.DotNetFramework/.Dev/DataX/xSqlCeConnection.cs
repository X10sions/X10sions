// System.Data.SqlServerCe.SqlCeConnection
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace Common.DataX {
  public class xSqlCeConnection : DbConnection {

    private class ObjectLifeTimeTracker : WeakReferenceCache {
      static ObjectLifeTimeTracker() {
        KillBitHelper.ThrowIfKillBitIsSet();
      }

      internal ObjectLifeTimeTracker(bool trackResurrection)
        : base(trackResurrection) {
      }

      internal bool HasOpenedCursors(SqlCeTransaction tx) {
        lock (this) {
          int num = items.Length;
          for (int i = 0; i < num; i++) {
            WeakReference weakReference = items[i];
            if (ADP.IsAlive(weakReference)) {
              object obj = null;
              obj = weakReference.Target;
              if (obj != null && obj is SqlCeDataReader) {
                SqlCeDataReader sqlCeDataReader = (SqlCeDataReader)obj;
                if (tx == sqlCeDataReader.transaction && !sqlCeDataReader.IsClosed) {
                  return true;
                }
              }
            }
          }
          return false;
        }
      }

      internal void CloseDataRdr(SqlCeTransaction tx) {
        ArrayList arrayList = new ArrayList();
        int num = items.Length;
        for (int i = 0; i < num; i++) {
          WeakReference weakReference = items[i];
          if (ADP.IsAlive(weakReference)) {
            object obj = null;
            obj = weakReference.Target;
            if (obj != null && obj is SqlCeDataReader) {
              SqlCeDataReader sqlCeDataReader = (SqlCeDataReader)obj;
              if ((tx == null || tx == sqlCeDataReader.transaction) && !sqlCeDataReader.IsClosed) {
                arrayList.Add(obj);
                items[i] = null;
              }
            }
          }
        }
        foreach (SqlCeDataReader item in arrayList) {
          item.Dispose();
        }
      }

      internal void Close(bool isDisposing) {
        ArrayList arrayList = new ArrayList();
        ArrayList arrayList2 = new ArrayList();
        ArrayList arrayList3 = new ArrayList();
        ArrayList arrayList4 = new ArrayList();
        int num = items.Length;
        for (int i = 0; i < num; i++) {
          WeakReference weakReference = items[i];
          if (ADP.IsAlive(weakReference)) {
            object obj = null;
            obj = weakReference.Target;
            if (obj != null) {
              if (obj is SqlCeDataReader) {
                arrayList3.Add(obj);
                items[i] = null;
              } else if (obj is SqlCeCommand) {
                arrayList2.Add(obj);
              } else if (obj is SqlCeTransaction) {
                arrayList.Add(obj);
                items[i] = null;
              } else if (obj is SqlCeChangeTracking) {
                arrayList4.Add(obj);
                items[i] = null;
              }
            }
          }
        }
        foreach (SqlCeDataReader item in arrayList3) {
          item.Dispose();
        }
        foreach (SqlCeChangeTracking item2 in arrayList4) {
          item2.Dispose();
        }
        foreach (SqlCeCommand item3 in arrayList2) {
          item3.CloseFromConnection();
          if (isDisposing) {
            item3.Connection = null;
          }
        }
        foreach (SqlCeTransaction item4 in arrayList) {
          item4.Dispose();
        }
      }

      internal void Zombie(SqlCeTransaction tx) {
        lock (this) {
          int num = items.Length;
          for (int i = 0; i < num; i++) {
            WeakReference weakReference = items[i];
            if (ADP.IsAlive(weakReference)) {
              object obj = null;
              obj = weakReference.Target;
              if (obj != null && obj is SqlCeCommand) {
                if (tx == ((SqlCeCommand)obj).Transaction) {
                  ((SqlCeCommand)obj).Transaction = null;
                } else if (tx == ((SqlCeCommand)obj).InternalTransaction) {
                  ((SqlCeCommand)obj).InternalTransaction = null;
                }
              }
            }
          }
        }
      }
    }

    private const int MaxRetrialAttempts = 10;

    private const int StartSleepInterval = 100;

    private static Hashtable connStrCache;

    private SqlCeConnectionStringBuilder connTokens;

    private SqlCeDelegatedTransaction _delegatedTransaction;

    private bool isOpened;

    private bool isHostControlled;

    private bool removePwd;

    private IntPtr pStoreService;

    private IntPtr pStoreServer;

    private IntPtr pSeStore;

    private IntPtr pQpServices;

    private IntPtr pQpDatabase;

    private IntPtr pQpSession;

    private IntPtr pTx;

    private IntPtr pStoreEvents;

    private IntPtr pError;

    private string connStr;

    private string dataSource;

    private string modifiedConnStr;

    private ConnectionState state;

    private bool isDisposed;

    private ObjectLifeTimeTracker weakReferenceCache;

    private bool isClosing;

    private int isNativeAssemblyReleased;

    private FlushFailureEventHandler flushFailureEventHandler;

    public string DatabaseIdentifier {
      [SecurityTreatAsSafe]
      [SecurityCritical]
      get {
        CheckStateOpen("GetDatabaseInfo");
        string empty = string.Empty;
        IntPtr pwszGuidString = (IntPtr)0;
        int databaseInstanceID = NativeMethods.GetDatabaseInstanceID(pSeStore, out pwszGuidString, pError);
        if (databaseInstanceID != 0) {
          ProcessResults(databaseInstanceID);
        }
        empty = Marshal.PtrToStringBSTR(pwszGuidString);
        NativeMethods.uwutil_SysFreeString(pwszGuidString);
        return empty;
      }
    }

    public override string ConnectionString {
      get {
        if (connStr == null || connStr.Trim().Length == 0) {
          return connStr = string.Empty;
        }
        if (removePwd) {
          if (connTokens == null) {
            return string.Empty;
          }
          if (!(bool)connTokens["Persist Security Info"]) {
            connStr = ConStringUtil.RemoveKeyValuesFromString(connStr, "Password");
          }
          removePwd = false;
        }
        return connStr;
      }
      set {
        if (state != 0) {
          throw new InvalidOperationException(Res.GetString("ADP_OpenConnectionPropertySet", "ConnectionString", state));
        }
        Hashtable hashtable = connStrCache;
        if (hashtable != null && value != null && hashtable.Contains(value)) {
          object[] array = (object[])hashtable[value];
          modifiedConnStr = (string)array[0];
          if (state != 0) {
            throw new InvalidOperationException(Res.GetString("ADP_OpenConnectionPropertySet", "ConnectionString", state));
          }
          connTokens = (SqlCeConnectionStringBuilder)array[1];
        } else if (value != null && value.Length > 0) {
          connTokens = new SqlCeConnectionStringBuilder(value);
          modifiedConnStr = value;
          if (connTokens != null) {
            CachedConnectionStringAdd(value, modifiedConnStr, connTokens);
          } else {
            modifiedConnStr = null;
          }
        } else {
          modifiedConnStr = null;
          connTokens = null;
        }
        connStr = value;
        removePwd = false;
        if (connTokens != null) {
          dataSource = (string)connTokens["Data Source"];
        }
      }
    }

    public override int ConnectionTimeout => 0;

    public override string Database => dataSource;

    public override string DataSource => dataSource;

    internal SqlCeDelegatedTransaction DelegatedTransaction {
      get {
        return _delegatedTransaction;
      }
      set {
        _delegatedTransaction = value;
      }
    }

    internal bool HasDelegatedTransaction => null != _delegatedTransaction;

    public override ConnectionState State => state;

    public override string ServerVersion => "4.0.8876.1";

    protected override DbProviderFactory DbProviderFactory => SqlCeProviderFactory.Instance;

    internal IntPtr ITransact => pTx;

    internal IntPtr IQPSession => pQpSession;

    internal IntPtr IQPServices => pQpServices;

    internal bool IsEnlisted => (bool)connTokens["Enlist"];

    internal SqlCeTransaction Transaction {
      get {
        for (int i = 0; i < weakReferenceCache.Count; i++) {
          object @object = weakReferenceCache.GetObject(i);
          if (@object is SqlCeTransaction) {
            SqlCeTransaction sqlCeTransaction = (SqlCeTransaction)@object;
            if (!HasDelegatedTransaction || sqlCeTransaction != DelegatedTransaction.SqlCeTransaction) {
              return sqlCeTransaction;
            }
          }
        }
        return null;
      }
    }

    public event SqlCeInfoMessageEventHandler InfoMessage {
      add {
        base.Events.AddHandler(ADP.EventInfoMessage, value);
      }
      remove {
        base.Events.RemoveHandler(ADP.EventInfoMessage, value);
      }
    }

    public event SqlCeFlushFailureEventHandler FlushFailure {
      add {
        base.Events.AddHandler(ADP.EventFlushFailure, value);
      }
      remove {
        base.Events.RemoveHandler(ADP.EventFlushFailure, value);
      }
    }

    public override event StateChangeEventHandler StateChange {
      add {
        base.Events.AddHandler(ADP.EventStateChange, value);
      }
      remove {
        base.Events.RemoveHandler(ADP.EventStateChange, value);
      }
    }

    static SqlCeConnection() {
      KillBitHelper.ThrowIfKillBitIsSet();
    }

    [SecurityCritical]
    internal void OnFlushFailure(int hr, IntPtr pError) {
      SqlCeFlushFailureEventHandler sqlCeFlushFailureEventHandler = (SqlCeFlushFailureEventHandler)base.Events[ADP.EventFlushFailure];
      if (sqlCeFlushFailureEventHandler != null) {
        try {
          sqlCeFlushFailureEventHandler(this, new SqlCeFlushFailureEventArgs(hr, pError, this));
        } catch (Exception e) {
          if (!ADP.IsCatchableExceptionType(e)) {
            throw;
          }
        }
      }
    }

    [SecurityTreatAsSafe]
    [SecurityCritical]
    public override void EnlistTransaction(Transaction SysTrans) {
      if ((Transaction)null == SysTrans) {
        string message = string.Format(CultureInfo.InvariantCulture, "SysTrans");
        throw new NullReferenceException(message);
      }
      if (IsEnlisted) {
        if (Transaction != null) {
          throw new InvalidOperationException(Res.GetString("ADP_LocalTransactionPresent"));
        }
        if (DelegatedTransaction == null && ConnectionState.Open == State) {
          Enlist(SysTrans);
          SqlCeDelegatedTransaction delegatedTransaction = DelegatedTransaction;
          for (int i = 0; i < weakReferenceCache.Count; i++) {
            object @object = weakReferenceCache.GetObject(i);
            if (@object is SqlCeCommand) {
              ((SqlCeCommand)@object).Transaction = delegatedTransaction.SqlCeTransaction;
            }
          }
        } else if (!HasDelegatedTransaction || !(SysTrans == DelegatedTransaction.Transaction)) {
          throw new InvalidOperationException(Res.GetString("ADP_ConnectionNotEnlisted"));
        }
      }
    }

    [SecurityCritical]
    internal void Enlist(Transaction tx) {
      if ((Transaction)null == tx) {
        string message = string.Format(CultureInfo.InvariantCulture, "transaction");
        throw new NullReferenceException(message);
      }
      SqlCeDelegatedTransaction sqlCeDelegatedTransaction = new SqlCeDelegatedTransaction(this, tx);
      if (tx.EnlistPromotableSinglePhase(sqlCeDelegatedTransaction)) {
        _delegatedTransaction = sqlCeDelegatedTransaction;
        return;
      }
      throw new InvalidOperationException(Res.GetString("ADP_ConnectionNotEnlisted"));
    }

    [SecurityTreatAsSafe]
    [SecurityCritical]
    public SqlCeConnection() {
      NativeMethods.LoadNativeBinaries();
      dataSource = string.Empty;
      isHostControlled = false;
      weakReferenceCache = new ObjectLifeTimeTracker(true);
      NativeMethods.DllAddRef();
      isNativeAssemblyReleased = 0;
    }

    public SqlCeConnection(string connectionString)
      : this() {
      ConnectionString = connectionString;
    }

    ~SqlCeConnection() {
      Dispose(false);
    }

    [SecurityCritical]
    private void ReleaseNativeInterfaces() {
      if (IntPtr.Zero != pQpSession) {
        NativeMethods.SafeRelease(ref pQpSession);
      }
      if (IntPtr.Zero != pQpDatabase) {
        NativeMethods.SafeRelease(ref pQpDatabase);
      }
      if (IntPtr.Zero != pTx) {
        NativeMethods.SafeRelease(ref pTx);
      }
      if (IntPtr.Zero != pStoreService) {
        NativeMethods.SafeRelease(ref pStoreService);
      }
      if (IntPtr.Zero != pQpServices) {
        NativeMethods.SafeRelease(ref pQpServices);
      }
      if (IntPtr.Zero != pStoreServer) {
        NativeMethods.SafeRelease(ref pStoreServer);
      }
      if (IntPtr.Zero != pStoreEvents) {
        NativeMethods.SafeRelease(ref pStoreEvents);
      }
      if (IntPtr.Zero != pError) {
        NativeMethods.SafeDelete(ref pError);
      }
      if (IntPtr.Zero != pSeStore) {
        NativeMethods.CloseAndReleaseStore(ref pSeStore);
      }
    }

    internal void DisposeSqlCeDataRdr(SqlCeTransaction tx) {
      if (weakReferenceCache != null) {
        weakReferenceCache.CloseDataRdr(tx);
      }
    }

    public new void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    [SecurityCritical]
    [SecurityTreatAsSafe]
    protected override void Dispose(bool disposing) {
      if (!isDisposed) {
        if (HasDelegatedTransaction) {
          if (disposing) {
            isDisposed = true;
            state = ConnectionState.Closed;
            if (weakReferenceCache != null) {
              weakReferenceCache.CloseDataRdr(null);
            }
          }
        } else {
          if (disposing) {
            if (isOpened) {
              try {
                OnStateChange(ConnectionState.Open, ConnectionState.Closed);
              } catch (Exception e) {
                if (!ADP.IsCatchableExceptionType(e)) {
                  throw;
                }
              }
            }
            if (weakReferenceCache != null) {
              weakReferenceCache.Close(true);
              weakReferenceCache = null;
            }
            connStr = null;
            dataSource = null;
            modifiedConnStr = null;
            isOpened = false;
            isDisposed = true;
            state = ConnectionState.Closed;
          }
          if (!isHostControlled) {
            ReleaseNativeInterfaces();
          }
          if (Interlocked.Exchange(ref isNativeAssemblyReleased, 1) == 0) {
            NativeMethods.DllRelease();
          }
          base.Dispose(disposing);
        }
      }
    }

    public override void Close() {
      Close(false);
      GC.KeepAlive(this);
    }

    internal void Zombie(SqlCeTransaction tx) {
      if (weakReferenceCache != null) {
        weakReferenceCache.Zombie(tx);
      }
    }

    [SecurityTreatAsSafe]
    [SecurityCritical]
    private void Close(bool silent) {
      if (isOpened && !isClosing) {
        isClosing = true;
        try {
          if (HasDelegatedTransaction) {
            state = ConnectionState.Closed;
            if (weakReferenceCache != null) {
              weakReferenceCache.CloseDataRdr(null);
            }
          } else {
            if (!silent) {
              OnStateChange(ConnectionState.Open, ConnectionState.Closed);
            }
            if (weakReferenceCache != null) {
              weakReferenceCache.Close(false);
            }
            ReleaseNativeInterfaces();
            isOpened = false;
            state = ConnectionState.Closed;
          }
        } finally {
          isClosing = false;
        }
      }
    }

    [SecurityCritical]
    [SecurityTreatAsSafe]
    public List<KeyValuePair<string, string>> GetDatabaseInfo() {
      List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
      int locale = 0;
      CheckStateOpen("GetDatabaseInfo");
      int locale2 = NativeMethods.GetLocale(pSeStore, ref locale, pError);
      if (locale2 != 0) {
        ProcessResults(locale2);
      }
      list.Add(new KeyValuePair<string, string>("Locale Identifier", locale.ToString()));
      int encryptionMode = 0;
      locale2 = NativeMethods.GetEncryptionMode(pSeStore, ref encryptionMode, pError);
      if (locale2 != 0) {
        ProcessResults(locale2);
      }
      string value = null;
      switch (encryptionMode) {
        case 0:
          value = string.Empty;
          break;
        case 2:
          value = "Engine Default";
          break;
        case 1:
          value = "Platform Default";
          break;
      }
      list.Add(new KeyValuePair<string, string>("Encryption Mode", value));
      int sortFlags = 0;
      locale2 = NativeMethods.GetLocaleFlags(pSeStore, ref sortFlags, pError);
      if (locale2 != 0) {
        ProcessResults(locale2);
      }
      string text = null;
      text = ((1 != (sortFlags & 1)) ? bool.TrueString : bool.FalseString);
      list.Add(new KeyValuePair<string, string>("Case Sensitive", text));
      return list;
    }

    [SecurityTreatAsSafe]
    [SecurityCritical]
    public new SqlCeTransaction BeginTransaction(System.Data.IsolationLevel isolationLevel) {
      int num = 0;
      if (HasDelegatedTransaction) {
        throw new InvalidOperationException(Res.GetString("ADP_ParallelTransactionsNotSupported", GetType().Name));
      }
      if (!isDisposed) {
        CheckStateOpen("BeginTransaction");
        SEISOLATION isoLevel;
        switch (isolationLevel) {
          case System.Data.IsolationLevel.Unspecified:
          case System.Data.IsolationLevel.ReadCommitted:
            isoLevel = SEISOLATION.ISO_READ_COMMITTED;
            break;
          case System.Data.IsolationLevel.RepeatableRead:
            isoLevel = SEISOLATION.ISO_REPEATABLE_READ;
            break;
          case System.Data.IsolationLevel.Serializable:
            isoLevel = SEISOLATION.ISO_SERIALIZABLE;
            break;
          default:
            throw new ArgumentException(Res.GetString("ADP_InvalidIsolationLevel", isolationLevel.ToString()));
        }
        SqlCeTransaction sqlCeTransaction = null;
        IntPtr ppUnknown = IntPtr.Zero;
        IntPtr ppUnknown2 = IntPtr.Zero;
        try {
          num = NativeMethods.OpenTransaction(pSeStore, pQpDatabase, isoLevel, IQPSession, ref ppUnknown, ref ppUnknown2, pError);
          if (num != 0) {
            ProcessResults(num);
          }
          sqlCeTransaction = new SqlCeTransaction(this, isolationLevel, ppUnknown, ppUnknown2);
          AddWeakReference(sqlCeTransaction);
          return sqlCeTransaction;
        } catch (Exception) {
          if (IntPtr.Zero != ppUnknown2) {
            NativeMethods.SafeRelease(ref ppUnknown2);
          }
          if (IntPtr.Zero != ppUnknown) {
            NativeMethods.SafeRelease(ref ppUnknown);
          }
          throw;
        }
      }
      throw new ObjectDisposedException("SqlCeConnection");
    }

    protected override DbTransaction BeginDbTransaction(System.Data.IsolationLevel isolationLevel) {
      return BeginTransaction(isolationLevel);
    }

    public new SqlCeTransaction BeginTransaction() {
      return BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
    }

    public override void ChangeDatabase(string value) {
      if (isDisposed) {
        throw new ObjectDisposedException("SqlCeConnection");
      }
      CheckStateOpen("ChangeDatabase");
      if (value == null || value.Trim().Length == 0) {
        throw new ArgumentException(Res.GetString("ADP_EmptyDatabaseName"));
      }
      string connectionString = ConnectionString;
      try {
        Close(true);
        SqlCeConnectionStringBuilder sqlCeConnectionStringBuilder = new SqlCeConnectionStringBuilder(ConnectionString) {
          DataSource = value
        };
        ConnectionString = sqlCeConnectionStringBuilder.ConnectionString;
        Open(true);
      } catch (Exception) {
        ConnectionString = connectionString;
        throw;
      }
    }

    internal void CheckStateOpen(string method) {
      if (ConnectionState.Open != State) {
        string name = null;
        switch (method) {
          case "BeginTransaction":
            name = "ADP_OpenConnectionRequired_BeginTransaction";
            break;
          case "ChangeDatabase":
            name = "ADP_OpenConnectionRequired_ChangeDatabase";
            break;
          case "CommitTransaction":
            name = "ADP_OpenConnectionRequired_CommitTransaction";
            break;
          case "RollbackTransaction":
            name = "ADP_OpenConnectionRequired_RollbackTransaction";
            break;
          case "set_Connection":
            name = "ADP_OpenConnectionRequired_SetConnection";
            break;
          case "GetDatabaseInfo":
            name = "ADP_OpenConnectionRequired_GetDatabaseInfo";
            break;
        }
        throw new InvalidOperationException(Res.GetString(name, method, State));
      }
    }

    internal void AddWeakReference(object value) {
      if (isDisposed || weakReferenceCache == null) {
        throw new ObjectDisposedException("SqlCeConnection");
      }
      weakReferenceCache.Add(value);
    }

    internal void RemoveWeakReference(object value) {
      if (weakReferenceCache == null) {
        throw new ObjectDisposedException("SqlCeConnection");
      }
      weakReferenceCache.Remove(value);
    }

    public override DataTable GetSchema() {
      return SchemaCollections.GetSchema(this);
    }

    public override DataTable GetSchema(string collectionName) {
      return SchemaCollections.GetSchema(this, collectionName);
    }

    public override DataTable GetSchema(string collectionName, string[] restrictionValues) {
      return SchemaCollections.GetSchema(this, collectionName, restrictionValues);
    }

    internal bool HasOpenedCursors(SqlCeTransaction tx) {
      if (weakReferenceCache != null) {
        return weakReferenceCache.HasOpenedCursors(tx);
      }
      return false;
    }

    protected override DbCommand CreateDbCommand() {
      return CreateCommand();
    }

    public new SqlCeCommand CreateCommand() {
      if (isDisposed) {
        throw new ObjectDisposedException("SqlCeConnection");
      }
      return new SqlCeCommand("", this);
    }

    private void OnStateChange(ConnectionState original, ConnectionState state) {
      StateChangeEventHandler stateChangeEventHandler = (StateChangeEventHandler)base.Events[ADP.EventStateChange];
      if (stateChangeEventHandler != null) {
        try {
          stateChangeEventHandler(this, new StateChangeEventArgs(original, state));
        } catch (Exception e) {
          if (!ADP.IsCatchableExceptionType(e)) {
            throw;
          }
        }
      }
    }

    [SecurityTreatAsSafe]
    [SecurityCritical]
    public override void Open() {
      if (HasDelegatedTransaction) {
        throw new InvalidOperationException(Res.GetString("ADP_ConnectionNotEnlisted"));
      }
      Open(false);
      if (IsEnlisted && (Transaction)null != System.Transactions.Transaction.Current) {
        try {
          Enlist(System.Transactions.Transaction.Current);
          SqlCeDelegatedTransaction delegatedTransaction = DelegatedTransaction;
          for (int i = 0; i < weakReferenceCache.Count; i++) {
            object @object = weakReferenceCache.GetObject(i);
            if (@object is SqlCeCommand) {
              ((SqlCeCommand)@object).Transaction = delegatedTransaction.SqlCeTransaction;
            }
          }
        } catch {
          Close();
          throw;
        }
      }
    }

    [SecurityTreatAsSafe]
    [SecurityCritical]
    internal unsafe void Open(bool silent) {
      int num = 0;
      int lcidLocale = -1;
      int cbBufferPool = -1;
      int dwAutoShrinkPercent = -1;
      int cMaxPages = -1;
      int cMaxTmpPages = -1;
      int dwDefaultEscalation = -1;
      int dwDefaultTimeout = -1;
      int dwFlushInterval = -1;
      object obj = null;
      string text = null;
      string source = null;
      string text2 = null;
      string value = null;
      SEOPENFLAGS dwFlags = SEOPENFLAGS.MODE_READ | SEOPENFLAGS.MODE_WRITE;
      bool flag = false;
      int num2 = 0;
      if (isDisposed) {
        throw new ObjectDisposedException("SqlCeConnection");
      }
      DateTime utcNow = DateTime.UtcNow;
      if (ConnectionString == null || ConnectionString.Length == 0) {
        throw new InvalidOperationException(Res.GetString("ADP_NoConnectionString"));
      }
      if (dataSource == null || dataSource.Trim().Length == 0) {
        throw new ArgumentException(Res.GetString("ADP_EmptyDatabaseName"));
      }
      if (isOpened) {
        throw new InvalidOperationException(Res.GetString("ADP_ConnectionAlreadyOpen", ConnectionState.Open.ToString()));
      }
      MEOPENINFO mEOPENINFO = default(MEOPENINFO);
      IntPtr intPtr = Marshal.AllocCoTaskMem(sizeof(MEOPENINFO));
      if (IntPtr.Zero == intPtr) {
        throw new OutOfMemoryException();
      }
      try {
        if (ADP.IsEmpty(modifiedConnStr)) {
          throw new InvalidOperationException(Res.GetString("ADP_NoConnectionString"));
        }
        text = ConStringUtil.ReplaceDataDirectory(dataSource);
        obj = connTokens["Locale Identifier"];
        if (obj != null) {
          lcidLocale = (int)obj;
        }
        obj = connTokens["Max Buffer Size"];
        if (obj != null) {
          cbBufferPool = (int)obj * 1024;
        }
        obj = connTokens["Autoshrink Threshold"];
        if (obj != null) {
          dwAutoShrinkPercent = (int)obj;
        }
        obj = connTokens["Max Database Size"];
        if (obj != null) {
          cMaxPages = (int)obj * 256;
        }
        obj = connTokens["Temp File Max Size"];
        if (obj != null) {
          cMaxTmpPages = (int)obj * 256;
        }
        obj = connTokens["Flush Interval"];
        if (obj != null) {
          dwFlushInterval = (int)obj;
        }
        obj = connTokens["Default Lock Escalation"];
        if (obj != null) {
          dwDefaultEscalation = (int)obj;
        }
        obj = connTokens["Default Lock Timeout"];
        if (obj != null) {
          dwDefaultTimeout = (int)obj;
        }
        obj = connTokens["Temp File Directory"];
        if (obj != null) {
          text2 = (string)obj;
        }
        obj = connTokens["Encryption Mode"];
        if (obj != null) {
          value = (string)obj;
        }
        obj = connTokens["Password"];
        if (obj != null) {
          string text3 = (string)obj;
          if (text3.Length > 0) {
            source = text3;
          }
        }
        obj = connTokens["Case Sensitive"];
        if (obj != null) {
          flag = (bool)obj;
        }
        string text4 = null;
        obj = connTokens["Mode"];
        if (obj != null) {
          text4 = (string)obj;
        }
        num2 = connTokens.FileAccessRetryTimeout * 1000;
        if (text4 != null) {
          switch (text4) {
            case "Read Only":
              dwFlags = SEOPENFLAGS.MODE_READ;
              break;
            case "Read Write":
              dwFlags = (SEOPENFLAGS.MODE_READ | SEOPENFLAGS.MODE_WRITE);
              break;
            case "Exclusive":
              dwFlags = (SEOPENFLAGS.MODE_READ | SEOPENFLAGS.MODE_WRITE | SEOPENFLAGS.MODE_SHARE_DENY_READ | SEOPENFLAGS.MODE_SHARE_DENY_WRITE);
              break;
            case "Shared Read":
              dwFlags = (SEOPENFLAGS.MODE_READ | SEOPENFLAGS.MODE_WRITE | SEOPENFLAGS.MODE_SHARE_DENY_WRITE);
              break;
          }
        }
        FileIOPermissionAccess fileIOPermissionAccess = FileIOPermissionAccess.Read;
        if (!string.IsNullOrEmpty(text4) && !text4.Equals("Read Only", StringComparison.OrdinalIgnoreCase)) {
          fileIOPermissionAccess = (fileIOPermissionAccess | FileIOPermissionAccess.Write | FileIOPermissionAccess.Append);
        }
        SqlCeUtil.DemandForPermission(text, fileIOPermissionAccess);
        if (!string.IsNullOrEmpty(text2)) {
          SqlCeUtil.DemandForPermission(text2, FileIOPermissionAccess.AllAccess);
        }
        mEOPENINFO.pwszFileName = NativeMethods.MarshalStringToLPWSTR(text);
        mEOPENINFO.pwszPassword = NativeMethods.MarshalStringToLPWSTR(source);
        mEOPENINFO.pwszTempPath = NativeMethods.MarshalStringToLPWSTR(text2);
        mEOPENINFO.lcidLocale = lcidLocale;
        mEOPENINFO.cbBufferPool = cbBufferPool;
        mEOPENINFO.dwAutoShrinkPercent = dwAutoShrinkPercent;
        mEOPENINFO.dwFlushInterval = dwFlushInterval;
        mEOPENINFO.cMaxPages = cMaxPages;
        mEOPENINFO.cMaxTmpPages = cMaxTmpPages;
        mEOPENINFO.dwDefaultTimeout = dwDefaultTimeout;
        mEOPENINFO.dwDefaultEscalation = dwDefaultEscalation;
        mEOPENINFO.dwFlags = dwFlags;
        mEOPENINFO.dwEncryptionMode = ConStringUtil.MapEncryptionMode(value);
        mEOPENINFO.dwLocaleFlags = 0;
        if (flag) {
          mEOPENINFO.dwLocaleFlags &= 1;
        }
        flushFailureEventHandler = OnFlushFailure;
        Marshal.StructureToPtr((object)mEOPENINFO, intPtr, false);
        num = NativeMethods.OpenStore(intPtr, Marshal.GetFunctionPointerForDelegate((Delegate)flushFailureEventHandler), ref pStoreService, ref pStoreServer, ref pQpServices, ref pSeStore, ref pTx, ref pQpDatabase, ref pQpSession, ref pStoreEvents, ref pError);
        SqlCeException ex = ProcessResults(num, pError, this);
        if (ex != null) {
          if (ex.NativeError != 25035 || num2 == 0) {
            throw ex;
          }
          int num3 = 100;
          int num4 = 1;
          DateTime utcNow2 = DateTime.UtcNow;
          TimeSpan timeSpan = utcNow2 - utcNow;
          while (ex != null && ex.NativeError == 25035 && utcNow <= utcNow2 && timeSpan.TotalMilliseconds < num2 && num4 <= 10) {
            int num5 = num2 - (int)timeSpan.TotalMilliseconds;
            if (num5 < num3) {
              num3 = num5;
            }
            Thread.Sleep(num3);
            num3 *= 2;
            num = NativeMethods.OpenStore(intPtr, Marshal.GetFunctionPointerForDelegate((Delegate)flushFailureEventHandler), ref pStoreService, ref pStoreServer, ref pQpServices, ref pSeStore, ref pTx, ref pQpDatabase, ref pQpSession, ref pStoreEvents, ref pError);
            ex = ProcessResults(num, pError, this);
            num4++;
            utcNow2 = DateTime.UtcNow;
            timeSpan = utcNow2 - utcNow;
          }
          if (ex != null) {
            throw ex;
          }
        }
        removePwd = true;
        state = ConnectionState.Open;
        isOpened = true;
      } finally {
        Marshal.FreeCoTaskMem(mEOPENINFO.pwszFileName);
        Marshal.FreeCoTaskMem(mEOPENINFO.pwszPassword);
        Marshal.FreeCoTaskMem(mEOPENINFO.pwszTempPath);
        Marshal.FreeCoTaskMem(intPtr);
        if (ConnectionState.Open != state) {
          Close();
          removePwd = false;
          state = ConnectionState.Closed;
        }
      }
      if (!silent) {
        OnStateChange(ConnectionState.Closed, ConnectionState.Open);
      }
    }

    private static void CachedConnectionStringAdd(string connStr, string modifiedConnStr, SqlCeConnectionStringBuilder connTokens) {
      Hashtable hashtable = connStrCache;
      lock (typeof(SqlCeConnection)) {
        if (hashtable == null) {
          hashtable = new Hashtable {
            [connStr] = new object[2]
          {
          modifiedConnStr,
          connTokens
          }
          };
          connStrCache = hashtable;
          return;
        }
      }
      lock (hashtable.SyncRoot) {
        if (!hashtable.Contains(connStr)) {
          if (hashtable.Count < 250) {
            hashtable[connStr] = new object[2]
            {
            modifiedConnStr,
            connTokens
            };
          } else {
            connStrCache = null;
          }
        }
      }
    }

    [SecurityCritical]
    private void ProcessResults(int hr) {
      Exception ex = ProcessResults(hr, pError, this);
      if (ex != null) {
        throw ex;
      }
    }

    [SecurityCritical]
    internal SqlCeException ProcessResults(int hr, IntPtr pError, object src) {
      if (hr == 0) {
        return null;
      }
      if (NativeMethods.Failed(hr)) {
        Exception ex = SqlCeUtil.CreateException(pError, hr);
        if (ex is SqlCeException) {
          return (SqlCeException)ex;
        }
        throw ex;
      }
      if (base.Events[ADP.EventInfoMessage] != null) {
        SqlCeInfoMessageEventHandler sqlCeInfoMessageEventHandler = (SqlCeInfoMessageEventHandler)base.Events[ADP.EventInfoMessage];
        if (sqlCeInfoMessageEventHandler != null) {
          try {
            sqlCeInfoMessageEventHandler(this, new SqlCeInfoMessageEventArgs(hr, pError, src));
          } catch (Exception e) {
            if (!ADP.IsCatchableExceptionType(e)) {
              throw;
            }
          }
        }
      } else {
        NativeMethods.ClearErrorInfo(pError);
      }
      return null;
    }
  }
}