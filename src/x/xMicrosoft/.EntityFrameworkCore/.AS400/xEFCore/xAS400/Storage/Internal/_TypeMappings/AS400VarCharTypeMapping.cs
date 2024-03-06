namespace xEFCore.xAS400.Storage.Internal {
  public class AS400VarCharTypeMapping : _AS400StringTypeMapping {
    //_keyAnsiString
    public AS400VarCharTypeMapping(int? size = null)
      : base($"varchar({size})", System.Data.DbType.AnsiString, false, size) {
    }

  }
}