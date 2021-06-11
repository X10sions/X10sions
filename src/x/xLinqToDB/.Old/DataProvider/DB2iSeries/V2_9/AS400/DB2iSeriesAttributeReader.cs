using LinqToDB.Common;
using LinqToDB.Mapping;
using LinqToDB.Metadata;
using System;
using System.Reflection;

namespace xLinqToDB.DataProvider.DB2iSeries.V2_9.AS400 {
  public class DB2iSeriesAttributeReader : IMetadataReader {
    readonly AttributeReader _reader = new AttributeReader();

    public T[] GetAttributes<T>(Type type, bool inherit) where T : Attribute => Array<T>.Empty;

    public T[] GetAttributes<T>(Type type, MemberInfo memberInfo, bool inherit)
      where T : Attribute {
      if (typeof(T) == typeof(ColumnAttribute)) {
        var attrs = _reader.GetAttributes<System.Data.Linq.Mapping.ColumnAttribute>(type, memberInfo, inherit);
        if (attrs.Length == 1) {
          var c = attrs[0];
          var attr = new ColumnAttribute {
            Name = c.Name,
            DbType = c.DbType,
            CanBeNull = c.CanBeNull,
            Storage = c.Storage,
            SkipOnInsert = c.IsDbGenerated,
            SkipOnUpdate = c.IsDbGenerated
          };
          return new[] { (T)(Attribute)attr };
        }
      }
      return Array<T>.Empty;
    }

    public MemberInfo[] GetDynamicColumns(Type type) => new MemberInfo[] { };
  }
}