using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using xEFCore.xAS400.Metadata;

namespace Microsoft.EntityFrameworkCore.Metadata {
  public static class IPropertyExtensions {

    public static IAS400PropertyAnnotations AS400([NotNull] this IProperty property)
        => new AS400PropertyAnnotations(Check.NotNull(property, nameof(property)));

    public static string GetTypeNameForCopy_SqlServer(this IProperty property, IRelationalTypeMapper typeMapper) {
      var typeName = property.AS400().ColumnType;
      if (typeName == null) {
        var principalProperty = property.FindPrincipal();
        typeName = principalProperty?.AS400().ColumnType;
        if (typeName == null) {
          if (property.ClrType == typeof(string)) {
            typeName = typeMapper.StringMapper?.FindMapping(
                property.IsUnicode() ?? principalProperty?.IsUnicode() ?? true, false, null).StoreType;
          } else if (property.ClrType == typeof(byte[])) {
            typeName = typeMapper.ByteArrayMapper?.FindMapping(false, false, null).StoreType;
          } else {
            typeName = typeMapper.FindMapping(property.ClrType).StoreType;
          }
        }
      }
      if (property.ClrType == typeof(byte[])
          && typeName != null
          && (typeName.Equals("rowversion", StringComparison.OrdinalIgnoreCase)
              || typeName.Equals("timestamp", StringComparison.OrdinalIgnoreCase))) {
        return property.IsNullable ? "varbinary(8)" : "binary(8)";
      }
      return typeName;
    }

  }
}