namespace xEFCore.xAS400.Storage.Internal {
  public class AS400CharTypeMapping : _AS400StringTypeMapping {
    public AS400CharTypeMapping(int? size = null)
      : base($"char({size})", System.Data.DbType.AnsiStringFixedLength, false, size) {
    }

  }

}