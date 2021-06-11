using IBM.Data.DB2.iSeries;
namespace xLinqToDB.DataProvider.DB2iSeries {
  public static class DB2iSeriesConstants {
    //public const string LastInsertedIdentityGetter = "IDENTITY_VAL_LOCAL()";
    public const string AssemblyName = "IBM.Data.DB2.iSeries";
    public const string ProviderFactoryName = "IBM.Data.DB2.iSeries";
    public const string ClientNamespace = "IBM.Data.DB2.iSeries";

    public const string CommandTypeName = nameof(iDB2Command);
    public const string ConnectionTypeName = nameof(iDB2Connection);
    public const string DataReaderTypeName = nameof(iDB2DataReader);
    public const string ParameterTypeName = nameof(iDB2Parameter);
    public const string TransactionTypeName = nameof(iDB2Transaction);

    //public const string ConnectionNamespace = "IBM.Data.DB2.iSeries.iDB2Connection";
    public const string IdentityColumnSql = "IDENTITY_VAL_LOCAL()";
    public const string DummyTableSchema = "SYSIBM";
    public const string DummyTableName = "SYSDUMMY1";
    public const string ProviderName = "DB2.iSeries";
  }
}