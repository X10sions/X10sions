using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Data.Common;

namespace xEFCore.xAS400.Storage.Internal {
  public class _AS400StringTypeMapping : StringTypeMapping {

    public _AS400StringTypeMapping(
        [NotNull] string storeType,
        [CanBeNull] DbType? dbType,
        bool unicode = false,
        int? size = null)
        : base(storeType, dbType, unicode, size) {
      _maxSpecificSize = CalculateSize(unicode, size);
    }

    readonly int _maxSpecificSize;

    static int CalculateSize(bool unicode, int? size)
       => unicode
           ? size.HasValue && size < 4000 ? size.Value : 4000
           : size.HasValue && size < 8000 ? size.Value : 8000;

    public override RelationalTypeMapping Clone(string storeType, int? size)
        => new _AS400StringTypeMapping(storeType, DbType, IsUnicode, size);

    protected override void ConfigureParameter(DbParameter parameter) {
      var value = parameter.Value;
      var length = (value as string)?.Length ?? (value as byte[])?.Length;
      parameter.Size = value == null || value == DBNull.Value || length != null && length <= _maxSpecificSize
        ? _maxSpecificSize
        : -1;     
    }

    protected override string GenerateNonNullSqlLiteral(object value)
        => IsUnicode
            ? $"N'{EscapeSqlLiteral((string)value)}'"
            : $"'{EscapeSqlLiteral((string)value)}'";

  }

}