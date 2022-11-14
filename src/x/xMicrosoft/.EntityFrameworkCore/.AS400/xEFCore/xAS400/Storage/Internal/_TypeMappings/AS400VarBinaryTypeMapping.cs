namespace xEFCore.xAS400.Storage.Internal {
  public class AS400VarBinaryTypeMapping : _AS400ByteArrayTypeMapping {
    //_keyBinary
    public AS400VarBinaryTypeMapping(int? size = null)
      : base($"varbinary({size})", System.Data.DbType.Binary, size) {
    }

  }
}