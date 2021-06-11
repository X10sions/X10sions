namespace xEFCore.xAS400.Storage.Internal {
  public class AS400NVarCharTypeMapping : _AS400StringTypeMapping {
    //_keyUnicodeString
    public AS400NVarCharTypeMapping(int? size = null)
      : base($"nvarchar({size})", System.Data.DbType.String, true, size) {
    }

  }
}