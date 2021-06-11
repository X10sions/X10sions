namespace xEFCore.xAS400.Storage.Internal {
  public class AS400BinaryTypeMapping : _AS400ByteArrayTypeMapping {
    public AS400BinaryTypeMapping(int? size = null)
      : base($"binary({size})", System.Data.DbType.Binary,size) {
    }
  }
}