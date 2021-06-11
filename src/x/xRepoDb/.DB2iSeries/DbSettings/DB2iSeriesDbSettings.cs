namespace RepoDb.DbSettings {
  public sealed class DB2iSeriesDbSettings : BaseDbSetting {

    public DB2iSeriesDbSettings() {
      AreTableHintsSupported = false;
      AverageableType = typeof(double);
      ClosingQuote = "'";// = "]";
      DefaultSchema = null;//= "dbo";
      IsDirectionSupported = true;
      IsExecuteReaderDisposable = true;
      IsMultiStatementExecutable = true;
      IsPreparable = true;
      IsUseUpsert = false;
      OpeningQuote = "'";//= "[";
      ParameterPrefix = "@";
      SchemaSeparator = ".";
    }

  }
}