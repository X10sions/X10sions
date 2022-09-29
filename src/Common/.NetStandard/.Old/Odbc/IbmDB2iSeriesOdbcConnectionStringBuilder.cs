using System.ComponentModel;

namespace Common.Data.Odbc {

  public class IbmDB2iSeriesOdbcConnectionStringBuilder : _BaseOdbcConnectionStringBuilder {
    public const string _DriverName = "{iSeries Access ODBC Driver}";
    public const string _Path32Bit = "C:\\Windows\\SysWOW64\\cwbodbc.dll";
    public const string _Path64Bit = "C:\\Windows\\system32\\cwbodbc.dll";

    public IbmDB2iSeriesOdbcConnectionStringBuilder() {
      Driver = _DriverName;
    }

    public IbmDB2iSeriesOdbcConnectionStringBuilder(string connectionString) : this() {
      ConnectionString = connectionString;
    }
    public IbmDB2iSeriesOdbcConnectionStringBuilder(string system, string userProfileName, string password) : this() {
      //Driver = is32Bit ? Path32Bit : Path64Bit;
      System = system;
      UserId = userProfileName;
      Password = password;
      //Signon = _Signon.UseSystemINavigatorDefault;
      //CommitMode = _CommitMode.ReadUncommitted;
    }

    // https://www.ibm.com/support/knowledgecenter/ssw_ibm_i_74/rzaik/connectkeywords.htm
    // https://www.ibm.com/support/knowledgecenter/en/ssw_ibm_i_71/rzaik/rzaikconnstrkeywordsgeneralprop.htm

    public override string ConnectionString { get => base.ConnectionString; set => this.SetConnectionString(value); }

    #region General

    public string Password { get => (string)this[nameof(Password)]; set => this[nameof(Password)] = value; }
    public _Signon Signon { get => (_Signon)this[nameof(Signon)]; set => this[nameof(Signon)] = value; }
    public _SSL SSL { get => (_SSL)this[nameof(SSL)]; set => this[nameof(SSL)] = value; }
    public string System { get => (string)this[nameof(System)]; set => this[nameof(System)] = value; }
    public string UserId { get => (string)this[nameof(UserId)]; set => this[nameof(UserId)] = value; }

    public enum _Signon {
      UseWindowsUserName = 0,
      UseDefaultUserId = 1,
      None = 2,
      UseSystemINavigatorDefault = 3,
      UseKerberosPrincipalncipal = 4
    }

    public enum _SSL {
      EncryptOnlythePassword = 0,
      EncryptAllClientsServerCommunication = 1
    }

    #endregion

    #region Server

    public _CommitMode CommitMode { get => (_CommitMode)this[nameof(CommitMode)]; set => this[nameof(CommitMode)] = value; }// = 2;
    public _ConnectionType ConnectionType { get => (_ConnectionType)this[nameof(ConnectionType)]; set => this[nameof(ConnectionType)] = value; }
    public string Database { get => (string)this[nameof(Database)]; set => this[nameof(Database)] = value; } //= string.Empty;
    public string DefaultLibraries { get => (string)this[nameof(DefaultLibraries)]; set => this[nameof(DefaultLibraries)] = value; }// = "QGPL";
    public int MaximumDecimalPrecision { get => (int)this[nameof(MaximumDecimalPrecision)]; set => this[nameof(MaximumDecimalPrecision)] = value; }// = 31;
    public int MaximumDecimalScale { get => (int)this[nameof(MaximumDecimalScale)]; set => this[nameof(MaximumDecimalScale)] = value; } //= 31;
    public int MinimumDivideScale { get => (int)this[nameof(MinimumDivideScale)]; set => this[nameof(MinimumDivideScale)] = value; }
    public _Naming Naming { get => (_Naming)this[nameof(Naming)]; set => this[nameof(Naming)] = value; }

    public enum _CommitMode {
      [Description("*NONE")] CommitImmediate = 0,
      [Description("*CS")] ReadCommitted = 1,
      [Description("*CHG")] ReadUncommitted = 2,
      [Description("*ALL")] RepeatableRead = 3,
      [Description("*RR")] Serializable = 4
    }

    public enum _ConnectionType {
      [Description("all SQL statements allowed")] ReadWrite = 0,
      [Description("SELECT and CALL statements allowed")] ReadCall = 1,
      [Description("SELECT statements only")] ReadOnly = 2
    }

    public enum _Naming {
      [Description("schema.table")] Sql = 0,
      [Description("schema/table")] System = 1
    }
    #endregion

    #region DataTypes
    public int DateFormat { get => (int)this[nameof(DateFormat)]; set => this[nameof(DateFormat)] = value; }// = 5;
    public int DateSeparator { get => (int)this[nameof(DateSeparator)]; set => this[nameof(DateSeparator)] = value; } //= 1;
    public int Decimal { get => (int)this[nameof(Decimal)]; set => this[nameof(Decimal)] = value; }
    public int DecFloatErrorOption { get => (int)this[nameof(DecFloatErrorOption)]; set => this[nameof(DecFloatErrorOption)] = value; }
    public int DecFloatRoundMode { get => (int)this[nameof(DecFloatRoundMode)]; set => this[nameof(DecFloatRoundMode)] = value; }
    public int MapDecimalFloatDescribe { get => (int)this[nameof(MapDecimalFloatDescribe)]; set => this[nameof(MapDecimalFloatDescribe)] = value; }// = 1;
    public int TimeFormat { get => (int)this[nameof(TimeFormat)]; set => this[nameof(TimeFormat)] = value; }
    public int TimeSeparator { get => (int)this[nameof(TimeSeparator)]; set => this[nameof(TimeSeparator)] = value; }
    public int CurrentImplicitXmlParseOption { get => (int)this[nameof(CurrentImplicitXmlParseOption)]; set => this[nameof(CurrentImplicitXmlParseOption)] = value; }
    public int XmlDeclaration { get => (int)this[nameof(XmlDeclaration)]; set => this[nameof(XmlDeclaration)] = value; }
    #endregion

    #region Package
    public string DefaultPackage { get => (string)this[nameof(DefaultPackage)]; set => this[nameof(DefaultPackage)] = value; }// = "QGPL/DEFAULT(IBM),2,0,1,0,512";
    public string DefaultPkgLibrary { get => (string)this[nameof(DefaultPkgLibrary)]; set => this[nameof(DefaultPkgLibrary)] = value; }// = "QGPL";
    public int ExtendedDynamic { get => (int)this[nameof(ExtendedDynamic)]; set => this[nameof(ExtendedDynamic)] = value; }// = 1;
    #endregion

    #region Performance
    public int AllowDataCompression { get => (int)this[nameof(AllowDataCompression)]; set => this[nameof(AllowDataCompression)] = value; }// = 1;
    public int BlockFetch { get => (int)this[nameof(BlockFetch)]; set => this[nameof(BlockFetch)] = value; }// = 1;
    public int BlockSizeKB { get => (int)this[nameof(BlockSizeKB)]; set => this[nameof(BlockSizeKB)] = value; }// = 256;
    public int Concurrency { get => (int)this[nameof(Concurrency)]; set => this[nameof(Concurrency)] = value; }
    public int CursorSensitivity { get => (int)this[nameof(CursorSensitivity)]; set => this[nameof(CursorSensitivity)] = value; }
    public int ExtendedColInfo { get => (int)this[nameof(ExtendedColInfo)]; set => this[nameof(ExtendedColInfo)] = value; }
    public int LazyClose { get => (int)this[nameof(LazyClose)]; set => this[nameof(LazyClose)] = value; }
    public int MaxFieldLength { get => (int)this[nameof(MaxFieldLength)]; set => this[nameof(MaxFieldLength)] = value; }// = 32;
    public int PreFetch { get => (int)this[nameof(PreFetch)]; set => this[nameof(PreFetch)] = value; }// = 1;
    public int QueryStorageLimit { get => (int)this[nameof(QueryStorageLimit)]; set => this[nameof(QueryStorageLimit)] = value; } //= -1;
    public int QueryOptimizeGoal { get => (int)this[nameof(QueryOptimizeGoal)]; set => this[nameof(QueryOptimizeGoal)] = value; }
    public int QueryTimeout { get => (int)this[nameof(QueryTimeout)]; set => this[nameof(QueryTimeout)] = value; } //= 1;
    #endregion

    #region Language
    public string LanguageID { get => (string)this[nameof(LanguageID)]; set => this[nameof(LanguageID)] = value; }// = "ENU";
    public int SortSequence { get => (int)this[nameof(SortSequence)]; set => this[nameof(SortSequence)] = value; }
    public string SortTable { get => (string)this[nameof(SortTable)]; set => this[nameof(SortTable)] = value; }// = string.Empty;
    public int SortWeight { get => (int)this[nameof(SortWeight)]; set => this[nameof(SortWeight)] = value; }
    #endregion

    #region Catalog
    public int CatalogOptions { get => (int)this[nameof(CatalogOptions)]; set => this[nameof(CatalogOptions)] = value; }// = 3;
    public int LibraryView { get => (int)this[nameof(LibraryView)]; set => this[nameof(LibraryView)] = value; }
    public int OdbcRemarks { get => (int)this[nameof(OdbcRemarks)]; set => this[nameof(OdbcRemarks)] = value; }
    public int SearchPattern { get => (int)this[nameof(SearchPattern)]; set => this[nameof(SearchPattern)] = value; } //= 1;
    #endregion

    #region Conversion
    public int AllowUnsupportedChar { get => (int)this[nameof(AllowUnsupportedChar)]; set => this[nameof(AllowUnsupportedChar)] = value; }
    public int CCSID { get => (int)this[nameof(CCSID)]; set => this[nameof(CCSID)] = value; }
    public int ForceTranslation { get => (int)this[nameof(ForceTranslation)]; set => this[nameof(ForceTranslation)] = value; }
    public int Graphic { get => (int)this[nameof(Graphic)]; set => this[nameof(Graphic)] = value; }
    public int HexParserOpt { get => (int)this[nameof(HexParserOpt)]; set => this[nameof(HexParserOpt)] = value; }
    public string TranslationDLL { get => (string)this[nameof(TranslationDLL)]; set => this[nameof(TranslationDLL)] = value; } //= string.Empty;
    public int TranslationOption { get => (int)this[nameof(TranslationOption)]; set => this[nameof(TranslationOption)] = value; }
    public int UnicodeSQL { get => (int)this[nameof(UnicodeSQL)]; set => this[nameof(UnicodeSQL)] = value; }
    #endregion

    #region Diagnostic
    public string QAQQINILibrary { get => (string)this[nameof(QAQQINILibrary)]; set => this[nameof(QAQQINILibrary)] = value; }// = string.Empty;
    public string SQDiagCode { get => (string)this[nameof(SQDiagCode)]; set => this[nameof(SQDiagCode)] = value; }// = string.Empty;
    public int Trace { get => (int)this[nameof(Trace)]; set => this[nameof(Trace)] = value; }
    #endregion

    #region Others
    public int AllowProcCalls { get => (int)this[nameof(AllowProcCalls)]; set => this[nameof(AllowProcCalls)] = value; }
    public int ConcurrentAccessResolution { get => (int)this[nameof(ConcurrentAccessResolution)]; set => this[nameof(ConcurrentAccessResolution)] = value; }
    public int ConvertDateTimeToChar { get => (int)this[nameof(ConvertDateTimeToChar)]; set => this[nameof(ConvertDateTimeToChar)] = value; }
    public int DB2SqlStates { get => (int)this[nameof(DB2SqlStates)]; set => this[nameof(DB2SqlStates)] = value; }
    public int DBCSNoTruncError { get => (int)this[nameof(DBCSNoTruncError)]; set => this[nameof(DBCSNoTruncError)] = value; }
    public int Debug { get => (int)this[nameof(Debug)]; set => this[nameof(Debug)] = value; }
    public int NewPwd { get => (int)this[nameof(NewPwd)]; set => this[nameof(NewPwd)] = value; }
    public int TrueAutoCommit { get => (int)this[nameof(TrueAutoCommit)]; set => this[nameof(TrueAutoCommit)] = value; }
    public int XALooselyCoupledSupport { get => (int)this[nameof(XALooselyCoupledSupport)]; set => this[nameof(XALooselyCoupledSupport)] = value; }
    public int XALockTimeout { get => (int)this[nameof(XALockTimeout)]; set => this[nameof(XALockTimeout)] = value; }
    public int XATransactionTimeout { get => (int)this[nameof(XATransactionTimeout)]; set => this[nameof(XATransactionTimeout)] = value; }
    #endregion

    #region
    #endregion

    public string Description { get => (string)this[nameof(Description)]; set => this[nameof(Description)] = value; } //= string.Empty;
    public int SQLConnectPromptMode { get => (int)this[nameof(SQLConnectPromptMode)]; set => this[nameof(SQLConnectPromptMode)] = value; }
    public int DelimitNames { get => (int)this[nameof(DelimitNames)]; set => this[nameof(DelimitNames)] = value; }

  }
}