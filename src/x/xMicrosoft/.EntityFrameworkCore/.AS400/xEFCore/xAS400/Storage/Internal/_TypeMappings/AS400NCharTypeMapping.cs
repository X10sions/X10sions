namespace xEFCore.xAS400.Storage.Internal {
  public class AS400NCharTypeMapping : _AS400StringTypeMapping {
    public AS400NCharTypeMapping(int? size = null)
      : base($"nchar({size})", System.Data.DbType.StringFixedLength, true, size) {
    }

  }

}