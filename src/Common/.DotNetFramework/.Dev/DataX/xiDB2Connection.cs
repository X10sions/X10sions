using IBM.Data.DB2.iSeries;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Security.Permissions;
using System.Text;

namespace Common.DataX {
  public class xiDB2Connection : DbConnection, ICloneable {
    internal enum Supported {
      Unknown,
      Yes,
      No
    }

    private string m_OriginalConnectionString;
    private string m_OriginalConnectionStringNoPassword;
    private iDB2ConnectionStringBuilder m_OriginalConnectionStringBuilder;
    private iDB2ConcurrentAccessResolution m_OriginalConcurrentAccessResolution;
    private string m_OverrideConnectionString;
    private string m_OverrideConnectionStringNoPassword;
    private iDB2ConnectionStringBuilder m_OverrideConnectionStringBuilder;
    private string m_CurrentConnectionString;
    private string m_CurrentConnectionStringNoPassword;
    private iDB2ConnectionStringBuilder m_CurrentConnectionStringBuilder;
    private iDB2ConcurrentAccessResolution m_CurrentConcurrentAccessResolution;
    private int m_HostVersion;
    private string m_ServerVersion;
    private string m_UserID;
    private string m_Password;
    private string m_Database;
    private string m_DefaultCollection;
    private string m_JobName;
    private MPConnection m_mpConnection;
    private IntPtr m_dcHandle;
    private ushort m_jobCCSID;
    private ConnectionState m_State;
    private string[] m_ParsedSchemaSearchList;
    private ArrayList m_CommandCollection = new ArrayList();
    private bool m_Disposed;
    private object m_LockObject = new object();
    private iDB2Transaction m_Transaction;
    private Supported m_ServerSupportsOptimisticUpdateColumns;

    private Supported m_ServerSupportsUnassignedIndicatorValue;

    private Supported m_ServerSupportsSqlschemas;

    private iDB2OptimisticUpdateOptions m_OptimisticUpdateOption = iDB2OptimisticUpdateOptions.ReturnOnlyIfGranularityGuaranteed;

    internal string OriginalConnectionString => m_OriginalConnectionString;

    internal bool sslIsDefault {
      get {
        if (m_CurrentConnectionStringBuilder.ShouldSerialize("SSL")) {
          return false;
        }
        return true;
      }
    }

    internal bool InTransactionMode {
      get {
        if (m_Transaction == null || !m_Transaction.TransactionPending) {
          return false;
        }
        return true;
      }
    }

    internal string InternalPassword => m_Password;

    internal Supported ServerSupportsOptimisticUpdateColumns => m_ServerSupportsOptimisticUpdateColumns;

    internal Supported ServerSupportsUnassignedIndicatorValue => m_ServerSupportsUnassignedIndicatorValue;

    internal Supported ServerSupportsSqlschemas => m_ServerSupportsSqlschemas;

    internal iDB2OptimisticUpdateOptions OptimisticUpdateOption {
      get {
        return m_OptimisticUpdateOption;
      }
      set {
        m_OptimisticUpdateOption = value;
      }
    }

    internal string[] ParsedSchemaSearchList {
      get {
        if (m_ParsedSchemaSearchList == null) {
          string text = m_CurrentConnectionStringBuilder.SchemaSearchList.Trim();
          if (!(text != string.Empty)) {
            text = ((Naming != 0) ? GetLibList() : m_DefaultCollection);
          } else {
            int num = text.IndexOf("*USRLIBL", 0, StringComparison.CurrentCultureIgnoreCase);
            if (num >= 0) {
              string libList = GetLibList();
              string text2 = text.Substring(0, num) + libList;
              if (text.Length > num + 8) {
                text2 += text.Substring(num + 8);
              }
              text = text2;
            }
          }
          char[] separator = new char[1]
          {
          ','
          };
          text = text.Trim();
          string text3 = "zzzzzzzzzzzzzzz";
          if (!text.Contains('"'.ToString())) {
            m_ParsedSchemaSearchList = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
          } else {
            StringBuilder stringBuilder = new StringBuilder();
            bool flag = false;
            foreach (char c in text) {
              switch (c) {
                case '"':
                  flag = ((!flag) ? true : false);
                  stringBuilder.Append(c);
                  break;
                case ',':
                  if (flag) {
                    stringBuilder.Append(text3);
                  } else {
                    stringBuilder.Append(c);
                  }
                  break;
                default:
                  stringBuilder.Append(c);
                  break;
              }
            }
            m_ParsedSchemaSearchList = stringBuilder.ToString().Split(separator, StringSplitOptions.RemoveEmptyEntries);
          }
          for (int j = 0; j < m_ParsedSchemaSearchList.Length; j++) {
            string text4 = (string)m_ParsedSchemaSearchList.GetValue(j);
            if (text4.Contains(text3)) {
              text4 = text4.Replace(text3, ','.ToString());
            }
            m_ParsedSchemaSearchList.SetValue(text4.Trim(), j);
          }
        }
        return m_ParsedSchemaSearchList;
      }
    }

    internal IntPtr DcHandle => m_dcHandle;

    internal ushort JobCCSID => m_jobCCSID;

    internal int HostVersion => m_HostVersion;

    internal string SortTableLib => m_CurrentConnectionStringBuilder.SortTableLib;

    internal string SortTableFile => m_CurrentConnectionStringBuilder.SortTableFile;

    [iDB2Category("PropertyCategory_Data")]
    [RefreshProperties(RefreshProperties.All)]
    [iDB2Description("PropertyDescription_ConnectionString")]
    public override string ConnectionString {
      get {
        if (m_CurrentConnectionStringBuilder.PersistSecurityInfo) {
          return m_OriginalConnectionString;
        }
        return m_CurrentConnectionStringNoPassword;
      }
      set {
        lock (m_LockObject) {
          if (iDB2Trace.Tracing) {
            iDB2Trace.Trace(this, "entry");
          }
          if (m_State != 0) {
            if (iDB2Trace.Logging) {
              iDB2Trace.Log(this, "Cannot set the ConnectionString property after the connection is open.");
            }
            throw new NotSupportedException(MPResourceManager.CannotSetConnectionString);
          }
          iDB2ConnectionStringBuilder iDB2ConnectionStringBuilder = new iDB2ConnectionStringBuilder(0);
          string connectionString = string.Empty;
          string empty = string.Empty;
          connectionString = value;
          string propertyInError = string.Empty;
          Exception innerException = new Exception();
          switch (iDB2ConnectionStringBuilder.SetConnectionString(ref connectionString, true, ref propertyInError, ref innerException)) {
            case parseReturnValue.invalidPropertyName:
              if (iDB2Trace.Tracing) {
                iDB2Trace.Trace(this, "Invalid property name specified: " + propertyInError);
              }
              throw new iDB2InvalidConnectionStringException(propertyInError);
            case parseReturnValue.invalidPropertyValue:
              if (iDB2Trace.Tracing) {
                iDB2Trace.Trace(this, "Invalid property value specified: " + propertyInError);
              }
              if (innerException == null) {
                throw new iDB2InvalidConnectionStringException(propertyInError);
              }
              throw new iDB2InvalidConnectionStringException(innerException, propertyInError);
            default:
              m_UserID = iDB2ConnectionStringBuilder.UserID;
              m_Password = iDB2ConnectionStringBuilder.Password;
              m_DefaultCollection = iDB2ConnectionStringBuilder.DefaultCollection;
              m_Database = iDB2ConnectionStringBuilder.Database;
              m_OriginalConcurrentAccessResolution = iDB2ConnectionStringBuilder.ConcurrentAccessResolution;
              m_CurrentConcurrentAccessResolution = m_OriginalConcurrentAccessResolution;
              empty = ((m_Password == null || !(m_Password != string.Empty)) ? connectionString : RemovePassword(ref connectionString, ref m_Password));
              m_OriginalConnectionString = connectionString;
              m_OriginalConnectionStringNoPassword = empty;
              m_OriginalConnectionStringBuilder = iDB2ConnectionStringBuilder;
              m_CurrentConnectionString = connectionString;
              m_CurrentConnectionStringNoPassword = empty;
              m_CurrentConnectionStringBuilder = iDB2ConnectionStringBuilder;
              m_ServerSupportsOptimisticUpdateColumns = Supported.Unknown;
              m_ServerSupportsUnassignedIndicatorValue = Supported.Unknown;
              m_ServerSupportsSqlschemas = Supported.Unknown;
              if (iDB2Trace.Tracing) {
                iDB2Trace.Trace(this, "(" + m_CurrentConnectionStringNoPassword + ") exit");
              }
              break;
          }
        }
      }
    }

    [iDB2Description("PropertyDescription_ConnectionTimeout")]
    public override int ConnectionTimeout {
      get {
        return m_CurrentConnectionStringBuilder.ConnectionTimeout;
      }
    }

    [iDB2Description("PropertyDescription_Database")]
    public override string Database {
      get {
        return m_Database;
      }
    }

    [iDB2Description("PropertyDescription_DataSource")]
    public override string DataSource {
      get {
        return m_CurrentConnectionStringBuilder.DataSource;
      }
    }

    [Browsable(false)]
    public override string ServerVersion {
      get {
        return m_ServerVersion;
      }
    }

    [Browsable(false)]
    public override ConnectionState State {
      get {
        return m_State;
      }
    }

    [Browsable(false)]
    public string Provider {
      get {
        return "IBM.Data.DB2.iSeries";
      }
    }

    [iDB2Description("PropertyDescription_UserID")]
    [iDB2Category("PropertyCategory_Security")]
    public string UserID {
      get {
        return m_UserID;
      }
    }

    [iDB2Description("PropertyDescription_Password")]
    [PasswordPropertyText(true)]
    [iDB2Category("PropertyCategory_Security")]
    public string Password {
      get {
        if (m_CurrentConnectionStringBuilder.PersistSecurityInfo) {
          return m_CurrentConnectionStringBuilder.Password;
        }
        return string.Empty;
      }
    }

    [Browsable(false)]
    public bool PersistSecurityInfo {
      get {
        return m_CurrentConnectionStringBuilder.PersistSecurityInfo;
      }
    }

    [Browsable(false)]
    public bool SSL {
      get {
        return m_CurrentConnectionStringBuilder.SSL;
      }
    }

    [Browsable(false)]
    public bool DataCompression {
      get {
        return m_CurrentConnectionStringBuilder.DataCompression;
      }
    }

    [Browsable(false)]
    public iDB2SortSequence SortSequence {
      get {
        return m_CurrentConnectionStringBuilder.SortSequence;
      }
    }

    [Browsable(false)]
    public string SortLanguageId {
      get {
        return m_CurrentConnectionStringBuilder.SortLanguageId;
      }
    }

    [Browsable(false)]
    public string SortTable {
      get {
        return m_CurrentConnectionStringBuilder.SortTable;
      }
    }

    [Browsable(false)]
    public iDB2HexParseOption HexParserOption {
      get {
        return m_CurrentConnectionStringBuilder.HexParserOption;
      }
    }

    [Browsable(false)]
    public int MaximumDecimalPrecision {
      get {
        return m_CurrentConnectionStringBuilder.MaximumDecimalPrecision;
      }
    }

    [Browsable(false)]
    public int MaximumDecimalScale {
      get {
        return m_CurrentConnectionStringBuilder.MaximumDecimalScale;
      }
    }

    [Browsable(false)]
    public int MinimumDivideScale {
      get {
        return m_CurrentConnectionStringBuilder.MinimumDivideScale;
      }
    }

    [iDB2Description("PropertyDescription_DefaultCollection")]
    public string DefaultCollection {
      get {
        return m_DefaultCollection;
      }
    }

    [Browsable(false)]
    public System.Data.IsolationLevel AutocommitIsolationLevel {
      get {
        return m_CurrentConnectionStringBuilder.AutocommitIsolationLevel;
      }
    }

    [Browsable(false)]
    public System.Data.IsolationLevel DefaultIsolationLevel {
      get {
        return m_CurrentConnectionStringBuilder.DefaultIsolationLevel;
      }
    }

    [Browsable(false)]
    public bool Pooling {
      get {
        return m_CurrentConnectionStringBuilder.Pooling;
      }
    }

    [Browsable(false)]
    public int MinimumPoolSize {
      get {
        return m_CurrentConnectionStringBuilder.MinimumPoolSize;
      }
    }

    [Browsable(false)]
    public int MaximumPoolSize {
      get {
        return m_CurrentConnectionStringBuilder.MaximumPoolSize;
      }
    }

    [Browsable(false)]
    public int MaximumUseCount {
      get {
        return m_CurrentConnectionStringBuilder.MaximumUseCount;
      }
    }

    [Browsable(false)]
    public iDB2TraceOptions Trace {
      get {
        return m_CurrentConnectionStringBuilder.Trace;
      }
    }

    [Browsable(false)]
    public string QueryOptionsFileLibrary {
      get {
        return m_CurrentConnectionStringBuilder.QueryOptionsFileLibrary;
      }
    }

    [Browsable(false)]
    public string JobName {
      get {
        return m_JobName;
      }
    }

    [iDB2Description("PropertyDescription_Naming")]
    public iDB2NamingConvention Naming {
      get {
        return m_CurrentConnectionStringBuilder.Naming;
      }
    }

    [Browsable(false)]
    public int MaximumInlineLobSize {
      get {
        return m_CurrentConnectionStringBuilder.MaximumInlineLobSize;
      }
    }

    [Browsable(false)]
    public int LobBlockSize {
      get {
        return m_CurrentConnectionStringBuilder.LobBlockSize;
      }
    }

    [Browsable(false)]
    public bool XmlStripWhitespace {
      get {
        return m_CurrentConnectionStringBuilder.XmlStripWhitespace;
      }
    }

    [iDB2Description("PropertyDescription_LibraryList")]
    public string LibraryList {
      get {
        return m_CurrentConnectionStringBuilder.LibraryList;
      }
    }

    [Browsable(false)]
    public bool CheckConnectionOnOpen {
      get {
        return m_CurrentConnectionStringBuilder.CheckConnectionOnOpen;
      }
    }

    [Browsable(false)]
    public iDB2QueryOptimizeGoal QueryOptimizeGoal {
      get {
        return m_CurrentConnectionStringBuilder.QueryOptimizeGoal;
      }
    }

    [Browsable(false)]
    public int QueryStorageLimit {
      get {
        return m_CurrentConnectionStringBuilder.QueryStorageLimit;
      }
    }

    [Browsable(false)]
    public int NumericErrorOption {
      get {
        return m_CurrentConnectionStringBuilder.NumericErrorOption;
      }
    }

    [Browsable(false)]
    public iDB2DecfloatRoundingMode DecfloatRoundingMode {
      get {
        return m_CurrentConnectionStringBuilder.DecfloatRoundingMode;
      }
    }

    [Browsable(false)]
    public int DecfloatErrorOption {
      get {
        return m_CurrentConnectionStringBuilder.DecfloatErrorOption;
      }
    }

    [Browsable(false)]
    public string ApplicationName {
      get {
        return m_CurrentConnectionStringBuilder.ApplicationName;
      }
    }

    [Browsable(false)]
    public string ClientAccounting {
      get {
        return m_CurrentConnectionStringBuilder.ClientAccounting;
      }
    }

    [Browsable(false)]
    public string ClientProgramID {
      get {
        return m_CurrentConnectionStringBuilder.ClientProgramID;
      }
    }

    [Browsable(false)]
    public string ClientUserID {
      get {
        return m_CurrentConnectionStringBuilder.ClientUserID;
      }
    }

    [Browsable(false)]
    public string ClientWorkstation {
      get {
        return m_CurrentConnectionStringBuilder.ClientWorkstation;
      }
    }

    [Browsable(false)]
    public iDB2ConcurrentAccessResolution ConcurrentAccessResolution {
      get {
        return m_CurrentConnectionStringBuilder.ConcurrentAccessResolution;
      }
    }

    [Browsable(false)]
    public bool Enlist {
      get {
        return m_CurrentConnectionStringBuilder.Enlist;
      }
    }

    [Browsable(false)]
    public int XaLockTimeout {
      get {
        return m_CurrentConnectionStringBuilder.XaLockTimeout;
      }
    }

    [Browsable(false)]
    public int XaTransactionTimeout {
      get {
        return m_CurrentConnectionStringBuilder.XaTransactionTimeout;
      }
    }

    [Browsable(false)]
    public int BlockSize {
      get {
        return m_CurrentConnectionStringBuilder.BlockSize;
      }
    }

    [Browsable(false)]
    public string SchemaSearchList {
      get {
        return m_CurrentConnectionStringBuilder.SchemaSearchList;
      }
    }

    [Browsable(false)]
    public bool DateTimeAsString {
      get {
        return m_CurrentConnectionStringBuilder.DateTimeAsString;
      }
    }

    [Browsable(false)]
    public bool DecNumericAsString {
      get {
        return m_CurrentConnectionStringBuilder.DecNumericAsString;
      }
    }

    [Browsable(false)]
    public bool CharBitDataAsString {
      get {
        return m_CurrentConnectionStringBuilder.CharBitDataAsString;
      }
    }

    [Browsable(false)]
    public int CharBitDataCcsid {
      get {
        return m_CurrentConnectionStringBuilder.CharBitDataCcsid;
      }
    }

    [Browsable(false)]
    public int TransactionCompletionTimeout {
      get {
        return m_CurrentConnectionStringBuilder.TransactionCompletionTimeout;
      }
    }

    internal char NameSeparator {
      get {
        if (m_CurrentConnectionStringBuilder.Naming == iDB2NamingConvention.SQL) {
          return '.';
        }
        return '/';
      }
    }

    [Browsable(false)]
    protected override DbProviderFactory DbProviderFactory {
      get {
        return iDB2Factory.Instance;
      }
    }

    [Browsable(false)]
    public bool EnablePreFetch {
      get {
        return m_CurrentConnectionStringBuilder.EnablePreFetch;
      }
    }

    [Browsable(false)]
    public bool AllowUnsupportedChar {
      get {
        return m_CurrentConnectionStringBuilder.AllowUnsupportedChar;
      }
    }

    public override event StateChangeEventHandler StateChange;

    public iDB2Connection() {
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "iDB2Connection() constructor");
      }
      m_OriginalConnectionStringBuilder = new iDB2ConnectionStringBuilder(0);
      m_OriginalConnectionString = string.Empty;
      m_OriginalConnectionStringNoPassword = string.Empty;
      m_OriginalConcurrentAccessResolution = iDB2ConcurrentAccessResolution.Default;
      m_CurrentConcurrentAccessResolution = iDB2ConcurrentAccessResolution.Default;
      InitializeConnectionVariables();
      m_CurrentConnectionString = m_OriginalConnectionString;
      m_CurrentConnectionStringNoPassword = m_OriginalConnectionStringNoPassword;
      m_CurrentConnectionStringBuilder = m_OriginalConnectionStringBuilder;
    }

    public iDB2Connection(string connectionString) {
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "iDB2Connection(String) constructor");
      }
      m_OriginalConnectionStringBuilder = new iDB2ConnectionStringBuilder(0);
      m_OriginalConnectionString = string.Empty;
      m_OriginalConnectionStringNoPassword = string.Empty;
      InitializeConnectionVariables();
      m_OriginalConnectionString = connectionString;
      string propertyInError = string.Empty;
      Exception innerException = new Exception();
      parseReturnValue parseReturnValue = m_OriginalConnectionStringBuilder.SetConnectionString(ref connectionString, true, ref propertyInError, ref innerException);
      if (parseReturnValue != 0) {
        if (iDB2Trace.Tracing) {
          iDB2Trace.Trace(this, "iDB2Connection(\"" + m_OriginalConnectionStringNoPassword + "\") detected an error.");
        }
        switch (parseReturnValue) {
          case parseReturnValue.invalidPropertyName:
            if (iDB2Trace.Tracing) {
              iDB2Trace.Trace(this, "Invalid property name specified: " + propertyInError);
            }
            throw new iDB2InvalidConnectionStringException(propertyInError);
          case parseReturnValue.invalidPropertyValue:
            if (iDB2Trace.Tracing) {
              iDB2Trace.Trace(this, "Invalid property value specified: " + propertyInError);
            }
            if (innerException == null) {
              throw new iDB2InvalidConnectionStringException(propertyInError);
            }
            throw new iDB2InvalidConnectionStringException(innerException, propertyInError);
        }
      }
      m_UserID = m_OriginalConnectionStringBuilder.UserID;
      m_Password = m_OriginalConnectionStringBuilder.Password;
      m_DefaultCollection = m_OriginalConnectionStringBuilder.DefaultCollection;
      m_Database = m_OriginalConnectionStringBuilder.Database;
      m_OriginalConcurrentAccessResolution = m_OriginalConnectionStringBuilder.ConcurrentAccessResolution;
      m_CurrentConcurrentAccessResolution = m_OriginalConcurrentAccessResolution;
      if (m_Password != null && m_Password != string.Empty) {
        m_OriginalConnectionStringNoPassword = RemovePassword(ref m_OriginalConnectionString, ref m_Password);
      } else {
        m_OriginalConnectionStringNoPassword = m_OriginalConnectionString;
      }
      m_CurrentConnectionString = m_OriginalConnectionString;
      m_CurrentConnectionStringNoPassword = m_OriginalConnectionStringNoPassword;
      m_CurrentConnectionStringBuilder = m_OriginalConnectionStringBuilder;
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "iDB2Connection(\"" + m_CurrentConnectionStringNoPassword + "\") exit");
      }
    }

    internal MPConnection GetMPConnection() {
      return m_mpConnection;
    }

    public new iDB2Transaction BeginTransaction() {
      return (iDB2Transaction)BeginDbTransaction(m_CurrentConnectionStringBuilder.DefaultIsolationLevel);
    }

    public new iDB2Transaction BeginTransaction(System.Data.IsolationLevel level) {
      return (iDB2Transaction)BeginDbTransaction(level);
    }

    protected override DbTransaction BeginDbTransaction(System.Data.IsolationLevel level) {
      System.Data.IsolationLevel isoLevel = level;
      if (level == System.Data.IsolationLevel.Unspecified) {
        isoLevel = DefaultIsolationLevel;
      }
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "BeginTransaction(" + level.ToString() + ")");
      }
      if (m_State != ConnectionState.Open) {
        if (iDB2Trace.Logging) {
          iDB2Trace.Log(this, "Cannot begin a transaction when the connection is Closed.");
        }
        throw new InvalidOperationException(MPResourceManager.InvalidOperationOnClosedConnection);
      }
      if (m_mpConnection.DistributedTransactionActive()) {
        if (iDB2Trace.Logging) {
          iDB2Trace.Log(this, "Unable to begin a transaction.  A distributed transaction is already active.");
        }
        throw new iDB2TransactionFailedException(MPResourceManager.DistTransAlreadyStarted, 0);
      }
      if (m_Transaction == null || !m_Transaction.TransactionPending) {
        m_Transaction = new iDB2Transaction(this, isoLevel);
        if (iDB2Trace.Tracing) {
          iDB2Trace.Trace(this, "created and associated iDB2Transaction(" + m_Transaction.GetHashCode() + "), level :" + m_Transaction.IsolationLevel.ToString());
        }
        return m_Transaction;
      }
      if (iDB2Trace.Logging) {
        iDB2Trace.Log(this, "already in transaction mode with iDB2Transaction(" + m_Transaction.GetHashCode() + "), level: " + m_Transaction.IsolationLevel.ToString());
      }
      throw new iDB2TransactionFailedException(MPResourceManager.AlreadyInTransactionMode, 0);
    }

    internal void CleanupTransaction() {
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "entry/exit");
      }
      m_Transaction = null;
    }

    public override void ChangeDatabase(string dbName) {
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, dbName);
      }
      if (m_State != ConnectionState.Open) {
        if (iDB2Trace.Logging) {
          iDB2Trace.Log(this, "Cannot call ChangeDatabase when the connection is Closed.");
        }
        throw new InvalidOperationException(MPResourceManager.InvalidOperationOnClosedConnection);
      }
      string text = dbName.Trim().ToUpper();
      string text2 = m_CurrentConnectionStringBuilder.Database.Trim().ToUpper();
      if (text2 == string.Empty || text2.CompareTo("*SYSBAS") == 0) {
        if (text.CompareTo(m_Database) != 0 && text.CompareTo("*SYSBAS") != 0) {
          if (iDB2Trace.Logging) {
            iDB2Trace.Log(this, "The Database cannot be changed.");
          }
          throw new InvalidOperationException(MPResourceManager.CannotChangeDatabase);
        }
      } else if (text.CompareTo(text2) != 0) {
        if (iDB2Trace.Logging) {
          iDB2Trace.Log(this, "The Database cannot be changed.");
        }
        throw new InvalidOperationException(MPResourceManager.CannotChangeDatabase);
      }
    }

    public void SetiDB2eWLMCorrelator(byte[] correlator) {
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "SetiDB2eWLMCorrelator()");
      }
      if (m_State != ConnectionState.Open) {
        if (iDB2Trace.Logging) {
          iDB2Trace.Log(this, "Cannot set the eWLM Correlator when the connection is Closed.");
        }
        throw new InvalidOperationException(MPResourceManager.InvalidOperationOnClosedConnection);
      }
      if (m_HostVersion < 328448) {
        if (iDB2Trace.Tracing) {
          iDB2Trace.Trace(this, "Setting the eWLM Correlator is not supported to this host version.  Operation ignored.");
        }
      } else if (correlator == null || correlator.Length < 2) {
        if (iDB2Trace.Tracing) {
          iDB2Trace.Trace(this, "The eWLM Correlator is either null or not in the correct format.  Operation ignored.");
        }
      } else {
        int num = (correlator[0] << 8) + correlator[1];
        if (correlator.Length < num) {
          if (iDB2Trace.Tracing) {
            iDB2Trace.Trace(this, "The eWLM Correlator is not in the correct format (incorrect length).  Operation ignored.");
          }
        } else {
          m_mpConnection.setiDB2eWLMCorrelator(correlator);
        }
      }
    }

    public void SetApplicationName(string applicationName) {
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "SetApplicationName()");
      }
      if (m_State != ConnectionState.Open) {
        if (iDB2Trace.Logging) {
          iDB2Trace.Log(this, "Cannot set the ApplicationName when the connection is Closed.");
        }
        throw new InvalidOperationException(MPResourceManager.InvalidOperationOnClosedConnection);
      }
      if (applicationName == null) {
        m_CurrentConnectionStringBuilder.ApplicationName = string.Empty;
      } else {
        m_CurrentConnectionStringBuilder.ApplicationName = applicationName;
      }
      if (m_HostVersion < 393472) {
        if (iDB2Trace.Tracing) {
          iDB2Trace.Trace(this, "Setting the ApplicationName is not supported to this host version.  Operation ignored.");
        }
      } else {
        m_mpConnection.setSpecialRegister(1, m_CurrentConnectionStringBuilder.ApplicationName);
      }
    }

    public void SetClientAccounting(string accountingString) {
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "SetClientAccounting()");
      }
      if (m_State != ConnectionState.Open) {
        if (iDB2Trace.Logging) {
          iDB2Trace.Log(this, "Cannot set the ClientAccounting when the connection is Closed.");
        }
        throw new InvalidOperationException(MPResourceManager.InvalidOperationOnClosedConnection);
      }
      if (accountingString == null) {
        m_CurrentConnectionStringBuilder.ClientAccounting = string.Empty;
      } else {
        m_CurrentConnectionStringBuilder.ClientAccounting = accountingString;
      }
      if (m_HostVersion < 393472) {
        if (iDB2Trace.Tracing) {
          iDB2Trace.Trace(this, "Setting the ClientAccounting is not supported to this host version.  Operation ignored.");
        }
      } else {
        m_mpConnection.setSpecialRegister(2, m_CurrentConnectionStringBuilder.ClientAccounting);
      }
    }

    public void SetClientProgramID(string programID) {
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "SetClientProgramID()");
      }
      if (m_State != ConnectionState.Open) {
        if (iDB2Trace.Logging) {
          iDB2Trace.Log(this, "Cannot set the ClientProgramID when the connection is Closed.");
        }
        throw new InvalidOperationException(MPResourceManager.InvalidOperationOnClosedConnection);
      }
      if (programID == null) {
        m_CurrentConnectionStringBuilder.ClientProgramID = string.Empty;
      } else {
        m_CurrentConnectionStringBuilder.ClientProgramID = programID;
      }
      if (m_HostVersion < 393472) {
        if (iDB2Trace.Tracing) {
          iDB2Trace.Trace(this, "Setting the ClientProgramID is not supported to this host version.  Operation ignored.");
        }
      } else {
        m_mpConnection.setSpecialRegister(5, m_CurrentConnectionStringBuilder.ClientProgramID);
      }
    }

    public void SetClientUserID(string userIDString) {
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "SetClientUserID()");
      }
      if (m_State != ConnectionState.Open) {
        if (iDB2Trace.Logging) {
          iDB2Trace.Log(this, "Cannot set the ClientUserID when the connection is Closed.");
        }
        throw new InvalidOperationException(MPResourceManager.InvalidOperationOnClosedConnection);
      }
      if (userIDString == null) {
        m_CurrentConnectionStringBuilder.ClientUserID = string.Empty;
      } else {
        m_CurrentConnectionStringBuilder.ClientUserID = userIDString;
      }
      if (m_HostVersion < 393472) {
        if (iDB2Trace.Tracing) {
          iDB2Trace.Trace(this, "Setting the ClientUserID is not supported to this host version.  Operation ignored.");
        }
      } else {
        m_mpConnection.setSpecialRegister(3, m_CurrentConnectionStringBuilder.ClientUserID);
      }
    }

    public void SetClientWorkstation(string workstationName) {
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "SetClientWorkstation()");
      }
      if (m_State != ConnectionState.Open) {
        if (iDB2Trace.Logging) {
          iDB2Trace.Log(this, "Cannot set the ClientWorkstation when the connection is Closed.");
        }
        throw new InvalidOperationException(MPResourceManager.InvalidOperationOnClosedConnection);
      }
      if (workstationName == null) {
        m_CurrentConnectionStringBuilder.ClientWorkstation = string.Empty;
      } else {
        m_CurrentConnectionStringBuilder.ClientWorkstation = workstationName;
      }
      if (m_HostVersion < 393472) {
        if (iDB2Trace.Tracing) {
          iDB2Trace.Trace(this, "Setting the ClientWorkstation is not supported to this host version.  Operation ignored.");
        }
      } else {
        m_mpConnection.setSpecialRegister(4, m_CurrentConnectionStringBuilder.ClientWorkstation);
      }
    }

    public void SetConcurrentAccessResolution(iDB2ConcurrentAccessResolution accessResolution) {
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "SetConcurrentAccessResolution()");
      }
      if (m_State != ConnectionState.Open) {
        if (iDB2Trace.Logging) {
          iDB2Trace.Log(this, "Cannot set the ConcurrentAccessResolution when the connection is Closed.");
        }
        throw new InvalidOperationException(MPResourceManager.InvalidOperationOnClosedConnection);
      }
      m_CurrentConnectionStringBuilder.ConcurrentAccessResolution = accessResolution;
      if (m_HostVersion < 459008) {
        if (iDB2Trace.Tracing) {
          iDB2Trace.Trace(this, "Setting the ConcurrentAccessResolution is not supported to this host version.  Operation ignored.");
        }
      } else {
        m_mpConnection.setConcurrentAccessResolution(accessResolution);
        m_CurrentConcurrentAccessResolution = accessResolution;
      }
    }

    public override void Open() {
      lock (m_LockObject) {
        if (iDB2Trace.Tracing) {
          iDB2Trace.Trace(this, "entry");
        }
        if (m_Disposed) {
          if (iDB2Trace.Tracing) {
            iDB2Trace.Trace(this, "iDB2Connection was previously Disposed.  Resurrecting object.");
          }
          GC.ReRegisterForFinalize(this);
          m_Disposed = false;
        }
        if (m_State == ConnectionState.Open) {
          if (iDB2Trace.Logging) {
            iDB2Trace.Log(this, "connection is already open.");
          }
          throw new InvalidOperationException(MPResourceManager.ConnectionAlreadyOpen);
        }
        if (iDB2Trace.ConnectionStringOverride != string.Empty) {
          if (iDB2Trace.Tracing) {
            iDB2Trace.Trace(this, "Using ConnectionStringOverride: " + iDB2Trace.ConnectionStringOverride);
          }
          OverrideConnectionString();
        }
        string propertyInError = string.Empty;
        parseReturnValue parseReturnValue = VerifyConnectionStringElements(ref m_CurrentConnectionStringBuilder, ref propertyInError);
        if (parseReturnValue == parseReturnValue.invalidPropertyValue) {
          throw new iDB2InvalidConnectionStringException(propertyInError);
        }
        iDB2Permission iDB2Permission = new iDB2Permission(PermissionState.None);
        iDB2Permission.Add(m_CurrentConnectionStringBuilder.ConnectionString, string.Empty, KeyRestrictionBehavior.AllowOnly);
        iDB2Permission.Demand();
        if (m_mpConnection != null) {
          if (iDB2Trace.Tracing) {
            iDB2Trace.Trace(this, "retrying the connection");
          }
          m_mpConnection.Disconnect();
          m_mpConnection.Connect();
        } else {
          m_mpConnection = MPConnectionManager.GetConnection(this);
        }
        MpDcErrorInfo errorInfo = m_mpConnection.ErrorInfo;
        int returnCode = errorInfo.returnCode;
        int errorType = errorInfo.errorType;
        int errorCode = errorInfo.errorCode;
        switch (errorType) {
          case 3:
            if (returnCode == 32106 && errorInfo.errorClass == 7 && errorInfo.errorCode == -704) {
              CleanupMPConnection();
              if (iDB2Trace.Tracing) {
                iDB2Trace.Trace(this, "Unable to connect to specified Database.");
              }
              throw new iDB2ConnectionFailedException(MPResourceManager.IASPMismatch);
            }
            if (returnCode == 32013) {
              CleanupMPConnection();
              if (iDB2Trace.Tracing) {
                iDB2Trace.Trace(this, "Unable to connect because at least one library was not added to the Library List");
              }
              throw new iDB2ConnectionFailedException(MPResourceManager.LibraryNotAddedToList);
            }
            if (!mpReturnCodes.isHostWarning(returnCode)) {
              if (iDB2Trace.Tracing) {
                iDB2Trace.Trace(this, "Host error occurred.  Error Class: " + errorInfo.errorClass.ToString() + ", Error Code: " + errorInfo.errorCode.ToString());
              }
              CleanupMPConnection();
              throw new iDB2HostErrorException(errorInfo);
            }
            if (iDB2Trace.Tracing) {
              iDB2Trace.Trace(this, "Non-fatal host warning occurred.  Error Class: " + errorInfo.errorClass.ToString() + ", Error Code: " + errorInfo.errorCode.ToString());
            }
            break;
          case 2:
            if (errorCode == iDB2Constants.CWB_CONNECTION_TIMED_OUT) {
              CleanupMPConnection();
              if (iDB2Trace.Tracing) {
                iDB2Trace.Trace(this, "tcp/ip comm stack timed out");
              }
              throw new iDB2ConnectionTimeoutException();
            }
            if (errorCode == iDB2Constants.CWB_USER_TIMEOUT) {
              CleanupMPConnection();
              if (iDB2Trace.Tracing) {
                iDB2Trace.Trace(this, "Connection timed out");
              }
              throw new iDB2ConnectionTimeoutException();
            }
            CleanupMPConnection();
            if (iDB2Trace.Tracing) {
              iDB2Trace.Trace(this, "Communications error");
            }
            throw new iDB2CommErrorException(errorInfo);
          case 1:
            switch (returnCode) {
              case 32325:
                CleanupMPConnection();
                if (iDB2Trace.Tracing) {
                  iDB2Trace.Trace(this, "Unsupported host version");
                }
                throw new iDB2UnsupportedHostVersionException();
              case 32600:
                CleanupMPConnection();
                if (iDB2Trace.Tracing) {
                  iDB2Trace.Trace(this, "Unable to connect to specified Database");
                }
                throw new iDB2ConnectionFailedException(MPResourceManager.IASPMismatch);
              case 32327:
                CleanupMPConnection();
                if (iDB2Trace.Tracing) {
                  iDB2Trace.Trace(this, "Unable to connect due to system policy");
                }
                throw new iDB2ConnectionFailedException(MPResourceManager.PermissionDenied);
              default:
                CleanupMPConnection();
                if (iDB2Trace.Tracing) {
                  iDB2Trace.Trace(this, "DC Function error.");
                }
                throw new iDB2DCFunctionErrorException(errorInfo);
            }
        }
        m_ServerVersion = m_mpConnection.ServerVersion;
        m_HostVersion = m_mpConnection.ServerVersionAsInt;
        m_jobCCSID = m_mpConnection.JobCCSID;
        m_dcHandle = m_mpConnection.DcHandle;
        m_UserID = m_mpConnection.UserId;
        m_Password = m_mpConnection.Password;
        m_DefaultCollection = m_mpConnection.DefaultCollection;
        m_Database = m_mpConnection.Database;
        m_JobName = m_mpConnection.JobName;
        m_ServerSupportsSqlschemas = ((m_HostVersion >= 328192) ? Supported.Yes : Supported.No);
        m_ServerSupportsOptimisticUpdateColumns = ((m_HostVersion >= 393472) ? Supported.Yes : Supported.No);
        m_ServerSupportsUnassignedIndicatorValue = ((m_HostVersion >= 393472) ? Supported.Yes : Supported.No);
        if (iDB2Trace.Tracing) {
          iDB2Trace.Trace(this, "Succeeded - JobName: " + m_JobName.TrimEnd() + ", Host Version: " + m_ServerVersion + ", DataBase: " + m_Database);
        }
        m_State = ConnectionState.Open;
        if ((Transaction)null != Transaction.Current) {
          if (Enlist) {
            m_mpConnection.StartDistTransaction(Transaction.Current);
          } else if (iDB2Trace.Tracing) {
            iDB2Trace.Trace(this, "An ambient transaction exists, but Enlist set to false");
          }
        }
        OnStateChange(new StateChangeEventArgs(ConnectionState.Closed, ConnectionState.Open));
        if (iDB2Trace.Tracing) {
          iDB2Trace.Trace(this, "exit");
        }
      }
    }

    public override void Close() {
      lock (m_LockObject) {
        if (iDB2Trace.Tracing) {
          iDB2Trace.Trace(this, "entry");
        }
        if (m_mpConnection == null) {
          if (iDB2Trace.Tracing) {
            iDB2Trace.Trace(this, "connection not open.  Ignoring close request.");
          }
        } else {
          ResetTransactionMode();
          if (m_CurrentConcurrentAccessResolution != m_OriginalConcurrentAccessResolution) {
            if (iDB2Trace.Tracing) {
              iDB2Trace.Trace(this, "Resetting ConcurrentAccessResolution to its original value.");
            }
            m_mpConnection.setConcurrentAccessResolution(m_OriginalConcurrentAccessResolution);
            m_CurrentConcurrentAccessResolution = m_OriginalConcurrentAccessResolution;
          }
          foreach (iDB2Command item in m_CommandCollection) {
            if (item.DisposeDelayed) {
              item.RemoveConnection(true);
            } else {
              item.RemoveConnection(false);
            }
          }
          if (iDB2Trace.Tracing) {
            iDB2Trace.Trace(this, "closing connection.");
          }
          CleanupMPConnection();
          InitializeConnectionVariables();
          if (m_OverrideConnectionStringBuilder != null) {
            m_OverrideConnectionStringBuilder.Clear();
            m_OverrideConnectionStringBuilder = null;
          }
          m_OverrideConnectionString = null;
          m_OverrideConnectionStringNoPassword = null;
          m_CurrentConnectionString = m_OriginalConnectionString;
          m_CurrentConnectionStringNoPassword = m_OriginalConnectionStringNoPassword;
          m_CurrentConnectionStringBuilder = m_OriginalConnectionStringBuilder;
          OnStateChange(new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Closed));
          if (iDB2Trace.Tracing) {
            iDB2Trace.Trace(this, "exit");
          }
        }
      }
    }

    internal void AddCommandToCollection(iDB2Command cmd) {
      if (m_CommandCollection.Contains(cmd)) {
        if (iDB2Trace.Tracing) {
          iDB2Trace.Trace(this, "iDB2Command(" + cmd.GetHashCode() + ") not added.  Command already exists in connection.");
        }
      } else {
        m_CommandCollection.Add(cmd);
        if (iDB2Trace.Tracing) {
          iDB2Trace.Trace(this, "iDB2Command(" + cmd.GetHashCode() + ") associated with connection.");
        }
      }
    }

    internal void RemoveCommandFromCollection(iDB2Command cmd) {
      if (m_CommandCollection.Contains(cmd)) {
        if (iDB2Trace.Tracing) {
          iDB2Trace.Trace(this, "Removing iDB2Command(" + cmd.GetHashCode() + ") from connection.");
        }
        m_CommandCollection.Remove(cmd);
      } else if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "iDB2Command( " + cmd.GetHashCode() + ") not found in connection.");
      }
    }

    private void CleanupMPConnection() {
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "entry");
        if (m_mpConnection != null) {
          iDB2Trace.Trace(this, "releasing MPConnection(" + m_mpConnection.GetHashCode() + ")");
        }
      }
      MPConnectionManager.ReleaseConnection(ref m_mpConnection);
      m_mpConnection = null;
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "exit");
      }
    }

    protected override DbCommand CreateDbCommand() {
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "entry");
      }
      iDB2Command iDB2Command = new iDB2Command(this);
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "created iDB2Command(" + iDB2Command.GetHashCode() + ")");
      }
      if (m_Transaction != null && m_Transaction.TransactionPending) {
        iDB2Command.Transaction = m_Transaction;
      }
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "exit");
      }
      return iDB2Command;
    }

    public new iDB2Command CreateCommand() {
      return (iDB2Command)CreateDbCommand();
    }

    object ICloneable.Clone() {
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "Creating a new connection");
      }
      iDB2Connection iDB2Connection = new iDB2Connection(m_OriginalConnectionString);
      string applicationName = m_CurrentConnectionStringBuilder.ApplicationName;
      if (applicationName != string.Empty) {
        iDB2Connection.m_CurrentConnectionStringBuilder.ApplicationName = applicationName;
      }
      applicationName = m_CurrentConnectionStringBuilder.ClientAccounting;
      if (applicationName != string.Empty) {
        iDB2Connection.m_CurrentConnectionStringBuilder.ClientAccounting = applicationName;
      }
      applicationName = m_CurrentConnectionStringBuilder.ClientProgramID;
      if (applicationName != string.Empty) {
        iDB2Connection.m_CurrentConnectionStringBuilder.ClientProgramID = applicationName;
      }
      applicationName = m_CurrentConnectionStringBuilder.ClientUserID;
      if (applicationName != string.Empty) {
        iDB2Connection.m_CurrentConnectionStringBuilder.ClientUserID = applicationName;
      }
      applicationName = m_CurrentConnectionStringBuilder.ClientWorkstation;
      if (applicationName != string.Empty) {
        iDB2Connection.m_CurrentConnectionStringBuilder.ClientWorkstation = applicationName;
      }
      iDB2ConcurrentAccessResolution concurrentAccessResolution = m_CurrentConnectionStringBuilder.ConcurrentAccessResolution;
      iDB2Connection.m_CurrentConnectionStringBuilder.ConcurrentAccessResolution = concurrentAccessResolution;
      return iDB2Connection;
    }

    public override DataTable GetSchema() {
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "Retrieve the MetaDataCollections schema");
      }
      DataTable dataTable = iDB2Schemas.MetaDataCollections();
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "{0} rows returned.", dataTable.Rows.Count);
      }
      return dataTable;
    }

    public override DataTable GetSchema(string collectionName) {
      DataTable dataTable = null;
      if (collectionName == null) {
        if (iDB2Trace.Tracing) {
          iDB2Trace.Trace(this, "collectionName = null");
        }
        dataTable = iDB2Schemas.MetaDataCollections();
      } else {
        if (iDB2Trace.Tracing) {
          iDB2Trace.Trace(this, "collectionName = {0}", collectionName);
        }
        dataTable = ((string.Compare(collectionName, DbMetaDataCollectionNames.MetaDataCollections, true) == 0) ? iDB2Schemas.MetaDataCollections() : ((string.Compare(collectionName, DbMetaDataCollectionNames.DataSourceInformation, true) == 0) ? iDB2Schemas.DataSourceInformation(this) : ((string.Compare(collectionName, DbMetaDataCollectionNames.DataTypes, true) == 0) ? iDB2Schemas.DataTypes(this) : ((string.Compare(collectionName, DbMetaDataCollectionNames.Restrictions, true) == 0) ? iDB2Schemas.Restrictions(this) : ((string.Compare(collectionName, DbMetaDataCollectionNames.ReservedWords, true) != 0) ? iDB2Schemas.GetSchema(collectionName, null, this) : iDB2Schemas.ReservedWords(this))))));
      }
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "{0} rows returned.", dataTable.Rows.Count);
      }
      return dataTable;
    }

    public override DataTable GetSchema(string collectionName, string[] restrictionValues) {
      if (collectionName == null || collectionName == string.Empty) {
        if (iDB2Trace.Logging) {
          iDB2Trace.Log(this, "Error - collectionName cannot be null or empty.");
        }
        throw new ArgumentException(MPResourceManager.NullSchemaRequested);
      }
      DataTable dataTable = null;
      int num = 0;
      if (restrictionValues != null) {
        num = restrictionValues.GetLength(0);
      }
      if (iDB2Trace.Tracing) {
        StringBuilder stringBuilder = new StringBuilder("collectionName = ").Append(collectionName);
        if (num == 0) {
          stringBuilder.Append(", restrictionValues=<no restrictions specified>");
        } else {
          stringBuilder.Append(", restrictionValues={");
          for (int i = 0; i < num; i++) {
            if (restrictionValues[i] == null) {
              stringBuilder.Append("null");
            } else {
              stringBuilder.Append(restrictionValues[i]);
            }
            if (i != num - 1) {
              stringBuilder.Append(", ");
            }
          }
          stringBuilder.Append('}');
        }
        iDB2Trace.Trace(this, stringBuilder.ToString());
      }
      if (string.Compare(collectionName, DbMetaDataCollectionNames.MetaDataCollections, true) == 0) {
        if (num != 0) {
          if (iDB2Trace.Logging) {
            iDB2Trace.Log(this, MPResourceManager.InvalidNumberOfRestrictions(collectionName));
          }
          throw new ArgumentException(MPResourceManager.InvalidNumberOfRestrictions(collectionName));
        }
        dataTable = iDB2Schemas.MetaDataCollections();
      } else if (string.Compare(collectionName, DbMetaDataCollectionNames.DataSourceInformation, true) == 0) {
        if (num != 0) {
          if (iDB2Trace.Logging) {
            iDB2Trace.Log(this, MPResourceManager.InvalidNumberOfRestrictions(collectionName));
          }
          throw new ArgumentException(MPResourceManager.InvalidNumberOfRestrictions(collectionName));
        }
        dataTable = iDB2Schemas.DataSourceInformation(this);
      } else if (string.Compare(collectionName, DbMetaDataCollectionNames.DataTypes, true) == 0) {
        if (num != 0) {
          if (iDB2Trace.Logging) {
            iDB2Trace.Log(this, MPResourceManager.InvalidNumberOfRestrictions(collectionName));
          }
          throw new ArgumentException(MPResourceManager.InvalidNumberOfRestrictions(collectionName));
        }
        dataTable = iDB2Schemas.DataTypes(this);
      } else if (string.Compare(collectionName, DbMetaDataCollectionNames.Restrictions, true) == 0) {
        if (num != 0) {
          if (iDB2Trace.Logging) {
            iDB2Trace.Log(this, MPResourceManager.InvalidNumberOfRestrictions(collectionName));
          }
          throw new ArgumentException(MPResourceManager.InvalidNumberOfRestrictions(collectionName));
        }
        dataTable = iDB2Schemas.Restrictions(this);
      } else if (string.Compare(collectionName, DbMetaDataCollectionNames.ReservedWords, true) == 0) {
        if (num != 0) {
          if (iDB2Trace.Logging) {
            iDB2Trace.Log(this, MPResourceManager.InvalidNumberOfRestrictions(collectionName));
          }
          throw new ArgumentException(MPResourceManager.InvalidNumberOfRestrictions(collectionName));
        }
        dataTable = iDB2Schemas.ReservedWords(this);
      } else {
        dataTable = iDB2Schemas.GetSchema(collectionName, restrictionValues, this);
      }
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "{0} rows returned.", dataTable.Rows.Count);
      }
      return dataTable;
    }

    public override void EnlistTransaction(Transaction transaction) {
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "iDB2Connection.EnlistTransaction");
      }
      m_mpConnection.StartDistTransaction(transaction);
    }

    protected override void OnStateChange(StateChangeEventArgs stateChange) {
      if (StateChange != null) {
        StateChange(this, stateChange);
      }
      base.OnStateChange(stateChange);
    }

    protected override void Dispose(bool disposing) {
      if (!m_Disposed) {
        try {
          if (disposing) {
            if (iDB2Trace.Tracing) {
              iDB2Trace.Trace(this, "iDB2Connection.Dispose");
            }
            Close();
            if (m_OverrideConnectionStringBuilder != null) {
              m_OverrideConnectionStringBuilder.Clear();
            }
            m_OriginalConnectionString = string.Empty;
            m_OriginalConnectionStringNoPassword = string.Empty;
            InitializeConnectionVariables();
            m_CurrentConnectionString = m_OriginalConnectionString;
            m_CurrentConnectionStringNoPassword = m_OriginalConnectionStringNoPassword;
            m_CurrentConnectionStringBuilder = m_OriginalConnectionStringBuilder;
          }
          base.Dispose(disposing);
        } catch {
        }
        m_Disposed = true;
        GC.SuppressFinalize(this);
      }
    }

    internal void MPConnectionDestructorCleanup() {
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "entry");
      }
      if (m_mpConnection != null) {
        ResetTransactionMode();
      }
      foreach (iDB2Command item in m_CommandCollection) {
        item.RemoveConnection(false);
      }
      m_mpConnection = null;
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "exit");
      }
    }

    private void InitializeConnectionVariables() {
      m_State = ConnectionState.Closed;
      m_ServerVersion = "00.00.0000";
      m_HostVersion = 0;
      m_jobCCSID = 0;
      m_dcHandle = IntPtr.Zero;
      m_JobName = string.Empty;
      m_mpConnection = null;
      m_ParsedSchemaSearchList = null;
      m_UserID = m_OriginalConnectionStringBuilder.UserID;
      m_Password = m_OriginalConnectionStringBuilder.Password;
      m_DefaultCollection = m_OriginalConnectionStringBuilder.DefaultCollection;
      m_Database = m_OriginalConnectionStringBuilder.Database;
    }

    private void ResetTransactionMode() {
      if (iDB2Trace.Tracing) {
        iDB2Trace.Trace(this, "iDB2Connection.ResetTransactionMode");
      }
      if (m_Transaction != null && m_Transaction.TransactionPending) {
        m_Transaction.Rollback();
        m_Transaction = null;
      }
    }

    private void OverrideConnectionString() {
      StringBuilder stringBuilder = new StringBuilder(m_OriginalConnectionString.Trim());
      string text = iDB2Trace.ConnectionStringOverride.Trim();
      if (!stringBuilder.ToString().EndsWith(";")) {
        stringBuilder.Append(";");
      }
      if (text.StartsWith(";")) {
        stringBuilder.Append(text.Substring(1, text.Length - 1));
      } else {
        stringBuilder.Append(text);
      }
      if (!stringBuilder.ToString().EndsWith(";")) {
        stringBuilder.Append(";");
      }
      m_OverrideConnectionStringBuilder = new iDB2ConnectionStringBuilder(0);
      m_OverrideConnectionString = string.Empty;
      m_OverrideConnectionStringNoPassword = string.Empty;
      m_OverrideConnectionString = stringBuilder.ToString();
      string propertyInError = string.Empty;
      Exception innerException = new Exception();
      string connectionString = stringBuilder.ToString();
      switch (m_OverrideConnectionStringBuilder.SetConnectionString(ref connectionString, true, ref propertyInError, ref innerException)) {
        case parseReturnValue.invalidPropertyName:
          if (iDB2Trace.Tracing) {
            iDB2Trace.Trace(this, "Invalid property name specified: " + propertyInError);
          }
          throw new iDB2InvalidConnectionStringException(propertyInError);
        case parseReturnValue.invalidPropertyValue:
          if (iDB2Trace.Tracing) {
            iDB2Trace.Trace(this, "Invalid property value specified: " + propertyInError);
          }
          if (innerException == null) {
            throw new iDB2InvalidConnectionStringException(propertyInError);
          }
          throw new iDB2InvalidConnectionStringException(innerException, propertyInError);
        default:
          m_UserID = m_OverrideConnectionStringBuilder.UserID;
          m_Password = m_OverrideConnectionStringBuilder.Password;
          m_DefaultCollection = m_OverrideConnectionStringBuilder.DefaultCollection;
          m_Database = m_OverrideConnectionStringBuilder.Database;
          if (m_Password != null && m_Password != string.Empty) {
            m_OverrideConnectionStringNoPassword = RemovePassword(ref connectionString, ref m_Password);
          }
          m_CurrentConnectionString = m_OverrideConnectionString;
          m_CurrentConnectionStringNoPassword = m_OverrideConnectionStringNoPassword;
          m_CurrentConnectionStringBuilder = m_OverrideConnectionStringBuilder;
          if (iDB2Trace.Tracing) {
            iDB2Trace.Trace(this, "(" + m_CurrentConnectionStringNoPassword + ") exit");
          }
          break;
      }
    }

    private string GetLibList() {
      MpDcGetLibraryList parms = default(MpDcGetLibraryList);
      parms.bufferLength = 5000;
      parms.buffer = new string(' ', parms.bufferLength);
      int num = 0;
      try {
        num = ((m_HostVersion < 459008) ? CwbDc.getLibraryList(m_mpConnection.DcHandle, ref parms) : CwbDc.getLongLibraryList(m_mpConnection.DcHandle, ref parms));
      } catch (Exception e) {
        string text = "Exception calling CwbDc.getLibraryList from iDB2Connection.GetLibList";
        if (iDB2Trace.Tracing) {
          iDB2Trace.Trace(this, text);
        }
        throw new iDB2DCFunctionErrorException(e, text);
      }
      if (num != 0) {
        string empty = string.Empty;
        empty = ((num != 32502) ? ("An unexpected error was returned from CwbDc.getLibraryList: " + num) : "The library list passed to CwbDc.getLibraryList is longer than the provider is prepared to handle.  Contact IBM Service or reduce the size of the LibraryList.");
        if (iDB2Trace.Tracing) {
          iDB2Trace.Trace(this, empty);
        }
        throw new iDB2DCFunctionErrorException(num);
      }
      return parms.buffer;
    }

    internal static string RemovePassword(ref string originalString, ref string pwd) {
      int num = originalString.LastIndexOf(pwd);
      int num2 = originalString.LastIndexOf("password", StringComparison.InvariantCultureIgnoreCase);
      if (num == -1 || num2 == -1) {
        return originalString;
      }
      int num3 = num + pwd.Length - num2;
      if (num3 < 0) {
        return originalString;
      }
      int num4 = originalString.IndexOf('=', num2, num3);
      if (num4 == -1) {
        return originalString;
      }
      int num5 = originalString.IndexOf(';', num + pwd.Length);
      if (num5 >= 0) {
        string text = originalString.Substring(num + pwd.Length).Trim();
        if (text[0] == ';') {
          num3 = num5 - num2 + 1;
        }
      }
      StringBuilder stringBuilder = new StringBuilder(originalString.Substring(0, num2));
      stringBuilder.Append(originalString.Substring(num2 + num3));
      return stringBuilder.ToString();
    }

    internal static parseReturnValue VerifyConnectionStringElements(ref iDB2ConnectionStringBuilder sb, ref string propertyInError) {
      switch (sb.SortSequence) {
        case iDB2SortSequence.SharedWeight:
        case iDB2SortSequence.UniqueWeight:
          if (sb.SortLanguageId == string.Empty) {
            propertyInError = "SortLanguageId";
            return parseReturnValue.invalidPropertyValue;
          }
          break;
        case iDB2SortSequence.UserSpecified:
          if (sb.SortTable == string.Empty) {
            propertyInError = "SortTable";
            return parseReturnValue.invalidPropertyValue;
          }
          break;
      }
      int maximumDecimalPrecision = sb.MaximumDecimalPrecision;
      int maximumDecimalScale = sb.MaximumDecimalScale;
      if (maximumDecimalScale > maximumDecimalPrecision) {
        propertyInError = "MaximumDecimalScale";
        return parseReturnValue.invalidPropertyValue;
      }
      int minimumPoolSize = sb.MinimumPoolSize;
      int maximumPoolSize = sb.MaximumPoolSize;
      if (maximumPoolSize >= 0 && minimumPoolSize > maximumPoolSize) {
        propertyInError = "MinimumPoolSize";
        return parseReturnValue.invalidPropertyValue;
      }
      if (sb.DataSource == string.Empty) {
        propertyInError = MPResourceManager.MissingDataSource;
        return parseReturnValue.invalidPropertyValue;
      }
      return parseReturnValue.noError;
    }
  }

}
