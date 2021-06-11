using Microsoft.EntityFrameworkCore.Storage;

namespace xEFCore.xAS400.Storage.Internal {
  public class AS400StringRelationalTypeMapper : StringRelationalTypeMapper {
    public AS400StringRelationalTypeMapper() : base(
      8000,
      AS400TypeMapper.VarChar,
      AS400TypeMapper.VarChar,
      AS400TypeMapper.VarChar900,
      size => new AS400VarCharTypeMapping(size),
      4000,
      AS400TypeMapper.NVarChar,
      AS400TypeMapper.NVarChar,
      AS400TypeMapper.NVarChar450,
      size => new AS400NVarCharTypeMapping(size)) {
    }

  }
}
