using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;

namespace xEFCore.xAS400.Storage.Internal {
  public class _AS400ByteArrayTypeMapping : ByteArrayTypeMapping {

    public _AS400ByteArrayTypeMapping(
          [NotNull] string storeType,
          [CanBeNull] DbType? dbType = System.Data.DbType.Binary,
          int? size = null)
          : base(storeType, dbType, size) {
      _maxSpecificSize = CalculateSize(size);
    }

    readonly int _maxSpecificSize;

    static int CalculateSize(int? size) => size.HasValue && size < 8000 ? size.Value : 8000;

    public override RelationalTypeMapping Clone(string storeType, int? size)
        => new _AS400ByteArrayTypeMapping(storeType, DbType, size);

    protected override void ConfigureParameter(DbParameter parameter) {
      var value = parameter.Value;
      var length = (value as string)?.Length ?? (value as byte[])?.Length;
      parameter.Size = value == null || value == DBNull.Value || length != null && length <= _maxSpecificSize
          ? _maxSpecificSize
          : -1;
    }

    protected override string GenerateNonNullSqlLiteral(object value) {
      var builder = new StringBuilder();
      builder.Append("0x");
      foreach (var @byte in (byte[])value) {
        builder.Append(@byte.ToString("X2", CultureInfo.InvariantCulture));
      }
      return builder.ToString();
    }

  }
}