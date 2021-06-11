using Microsoft.EntityFrameworkCore.Storage;

namespace xEFCore.xAS400.Storage.Internal {
  public class AS400ByteArrayRelationalTypeMapper : ByteArrayRelationalTypeMapper {
    public AS400ByteArrayRelationalTypeMapper() : base(
      8000,
      AS400TypeMapper.VarBinary,
      AS400TypeMapper.VarBinary,
      AS400TypeMapper.VarBinary900,
      AS400TypeMapper.RowVersion,
      size => new AS400VarBinaryTypeMapping(size)) {
    }

  }
}