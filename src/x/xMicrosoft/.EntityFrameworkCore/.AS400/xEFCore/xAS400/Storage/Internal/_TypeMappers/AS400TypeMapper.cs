using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;

namespace xEFCore.xAS400.Storage.Internal {

  public class AS400TypeMapper : RelationalTypeMapper {

    public AS400TypeMapper([NotNull] RelationalTypeMapperDependencies dependencies)
       : base(dependencies) { }

    public static readonly AS400BinaryTypeMapping Binary = new AS400BinaryTypeMapping();
    public static readonly AS400CharTypeMapping Char = new AS400CharTypeMapping();
    public static readonly AS400NCharTypeMapping NChar = new AS400NCharTypeMapping();
    public static readonly AS400NVarCharTypeMapping NVarChar = new AS400NVarCharTypeMapping();//4000
    public static readonly AS400NVarCharTypeMapping NVarChar450 = new AS400NVarCharTypeMapping(450);
    public static readonly AS400RowVersionTypeMapping RowVersion = new AS400RowVersionTypeMapping();
    public static readonly AS400VarBinaryTypeMapping VarBinary = new AS400VarBinaryTypeMapping(null);//8000
    public static readonly AS400VarBinaryTypeMapping VarBinary900 = new AS400VarBinaryTypeMapping(900);
    public static readonly AS400VarCharTypeMapping VarChar = new AS400VarCharTypeMapping();//8000
    public static readonly AS400VarCharTypeMapping VarChar900 = new AS400VarCharTypeMapping(900);

    protected override string GetColumnType(IProperty property) => property.AS400().ColumnType;
    protected override IReadOnlyDictionary<Type, RelationalTypeMapping> GetClrTypeMappings() => new ClrTypeMappings_AS400();
    protected override IReadOnlyDictionary<string, RelationalTypeMapping> GetStoreTypeMappings() => new StoreTypeMappings_AS400();
    protected override bool RequiresKeyMapping(IProperty property) => base.RequiresKeyMapping(property) || property.IsIndex();

    public override IByteArrayRelationalTypeMapper ByteArrayMapper { get; } = new AS400ByteArrayRelationalTypeMapper();
    public override IStringRelationalTypeMapper StringMapper { get; } = new AS400StringRelationalTypeMapper();

    public override RelationalTypeMapping FindMapping(Type clrType) {
      Check.NotNull(clrType, nameof(clrType));
      clrType = clrType.UnwrapNullableType().UnwrapEnumType();
      return clrType == typeof(string)
          ? NVarChar//UnboundedUnicodeString
          : (clrType == typeof(byte[])
              ? VarBinary
              : base.FindMapping(clrType));
    }

    public override void ValidateTypeName(string storeType) {
      var _disallowedMappings = new DisallowedMappings_AS400();
      if (_disallowedMappings.Contains(storeType)) {
        throw new ArgumentException(EFCoreStrings.UnqualifiedDataType(storeType));
      }
    }


  }
}