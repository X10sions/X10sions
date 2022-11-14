namespace xEFCore.xAS400.Storage.Internal {
  public class AS400RowVersionTypeMapping : _AS400ByteArrayTypeMapping {

    //_rowversion
    public AS400RowVersionTypeMapping()
      : base("rowversion", System.Data.DbType.Binary, 8) {
    }

  }
}