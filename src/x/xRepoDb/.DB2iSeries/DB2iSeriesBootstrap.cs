using IBM.Data.DB2.iSeries;
using RepoDb.DbHelpers;
using RepoDb.DbSettings;
using RepoDb.StatementBuilders;

namespace RepoDb.DB2iSeries {
  public static class DB2iSeriesBootstrap {
    public static bool IsInitialized { get; private set; }

    public static void Initialize() {
      if (IsInitialized) return;
      // Map the DbSetting
      var dbSetting = new DB2iSeriesDbSettings();
      DbSettingMapper.Add(typeof(iDB2Connection), dbSetting, true);
      DbHelperMapper.Add(typeof(iDB2Connection), new DB2iSeriesDbHelper<iDB2Connection>(), true);
      StatementBuilderMapper.Add(typeof(iDB2Connection), new DB2iSeries_SqlServerStatementBuilder<iDB2Connection>(dbSetting), true);
      // Set the flag
      IsInitialized = true;
    }

  }
}