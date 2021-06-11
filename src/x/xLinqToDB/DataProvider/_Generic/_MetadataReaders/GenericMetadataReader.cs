using LinqToDB.Metadata;
using System;
using System.Reflection;
using System.Linq;

namespace LinqToDB.DataProvider.DB2iSeries {
  public class DB2iSeriesMetadataReader : IMetadataReader {
    public DB2iSeriesMetadataReader(string providerName) {
      this.providerName = providerName;
    }
    string providerName;

    public T[] GetAttributes<T>(Type type, MemberInfo memberInfo, bool inherit = true) where T : Attribute {
      if (typeof(T) == typeof(Sql.ExpressionAttribute)) {
        switch (memberInfo.Name) {
          case "CharIndex":
            return new[] { (T)(object)new Sql.FunctionAttribute("Locate") };
          case "Trim":
            if (memberInfo.ToString().EndsWith("(Char[])", StringComparison.CurrentCultureIgnoreCase)) {
              return new[] { (T)(object)(new Sql.ExpressionAttribute(providerName, "Strip({0}, B, {1})")) };
            }
            break;
          case "TrimLeft":
            if (memberInfo.ToString().EndsWith("(Char[])", StringComparison.CurrentCultureIgnoreCase) || memberInfo.ToString().EndsWith("System.Nullable`1[System.Char])", StringComparison.CurrentCultureIgnoreCase)) {
              return new[] { (T)(object)(new Sql.ExpressionAttribute(providerName, "Strip({0}, L, {1})")) };
            }
            break;
          case "TrimRight":
            if (memberInfo.ToString().EndsWith("(Char[])", StringComparison.CurrentCultureIgnoreCase) || memberInfo.ToString().EndsWith("System.Nullable`1[System.Char])", StringComparison.CurrentCultureIgnoreCase)) {
              return new[] { (T)(object)(new Sql.ExpressionAttribute(providerName, "Strip({0}, T, {1})")) };
            }
            break;
          case "Truncate": return new[] { (T)(object)new Sql.ExpressionAttribute(providerName, "Truncate({0}, 0)") };
          case "DateAdd": return new[] { (T)(object)new Sql.ExtensionAttribute(providerName, "") { ServerSideOnly = false, PreferServerSide = false, BuilderType = typeof(DB2iSeriesDateAddBuilder) } };
          case "DatePart": return new[] { (T)(object)new Sql.ExtensionAttribute(providerName, "") { ServerSideOnly = false, BuilderType = typeof(DB2iSeriesDatePartBuilder) } };
          case "DateDiff": return new[] { (T)(object)new Sql.ExtensionAttribute(providerName, "") { BuilderType = typeof(DB2iSeriesDateDiffBuilder) } };
          case "TinyInt": return new[] { (T)(object)new Sql.ExpressionAttribute(providerName, "SmallInt") { ServerSideOnly = true } };
          case "DefaultNChar":
          case "DefaultNVarChar": return new[] { (T)(object)new Sql.FunctionAttribute(providerName, "Char") { ServerSideOnly = true } };
          case "Substring": return new[] { (T)(object)new Sql.FunctionAttribute(providerName, "Substr") { PreferServerSide = true } };
          case "Atan2": return new[] { (T)(object)new Sql.FunctionAttribute(providerName, "Atan2", 1, 0) };
          case "Log": return new[] { (T)(object)new Sql.FunctionAttribute(providerName, "Ln") };
          case "Log10": return new[] { (T)(object)new Sql.FunctionAttribute(providerName, "Log") };
          case "NChar":
          case "NVarChar": return new[] { (T)(object)new Sql.FunctionAttribute(providerName, "Char") { ServerSideOnly = true } };
          case "Replicate": return new[] { (T)(object)new Sql.FunctionAttribute(providerName, "Repeat") };
          case "StringAggregate" when memberInfo is MethodInfo stringAggregateMethod:
            var firstParameter = stringAggregateMethod.GetParameters().Any(x => x.Name == "selector") ? "selector" : "source";
            return new[] { (T)(object)new Sql.ExtensionAttribute(providerName, "LISTAGG({" + firstParameter + "}, {separator}){_}{aggregation_ordering?}") { IsAggregate = true, ChainPrecedence = 10 } };

        }
      }
      return new T[0];
    }

    public T[] GetAttributes<T>(Type type, bool inherit = true) where T : Attribute => new T[0];

    public MemberInfo[] GetDynamicColumns(Type type) => new MemberInfo[] { };

  }

  //public class DB2iSeriesAttributeReader : IMetadataReader {
  //  private readonly AttributeReader _reader = new AttributeReader();

  //  public T[] GetAttributes<T>(Type type, bool inherit = true) where T : Attribute => Array<T>.Empty;

  //  public T[] GetAttributes<T>(Type type, MemberInfo memberInfo, bool inherit = true) where T : Attribute {
  //    if (typeof(T) == typeof(LinqToDB.Mapping.ColumnAttribute)) {
  //      var attrs = _reader.GetAttributes<System.Data.Linq.Mapping.ColumnAttribute>(type, memberInfo, inherit);
  //      if (attrs.Length == 1) {
  //        var c = attrs[0];
  //        var attr = new LinqToDB.Mapping.ColumnAttribute {
  //          Name = c.Name,
  //          DbType = c.DbType,
  //          CanBeNull = c.CanBeNull,
  //          Storage = c.Storage,
  //          SkipOnInsert = c.IsDbGenerated,
  //          SkipOnUpdate = c.IsDbGenerated
  //        };
  //        return new T[] {
  //          (T)(Attribute)attr
  //        };
  //      }
  //    }
  //    return Array<T>.Empty;
  //  }
  //  public MemberInfo[] GetDynamicColumns(Type type) => new MemberInfo[] { };
  //}

}