using LinqToDB;
using LinqToDB.Metadata;
using System;
using System.Reflection;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_1_6_1.TB {
  public class xDB2iSeriesMetadataReader : IMetadataReader {
    private readonly string providerName;

    public xDB2iSeriesMetadataReader(string providerName) {
      this.providerName = providerName;
    }

    public T[] GetAttributes<T>(Type type, MemberInfo memberInfo, bool inherit = true) where T : Attribute {
      if (typeof(Sql.ExpressionAttribute).IsAssignableFrom(typeof(T))) {
        switch (memberInfo.Name) {
          case "CharIndex":
            return GetFunction<T>(() => new Sql.FunctionAttribute("Locate"));
          case "Trim":
            if (memberInfo.ToString().EndsWith("(Char[])", StringComparison.CurrentCultureIgnoreCase)) {
              return GetExpression<T>(() => new Sql.ExpressionAttribute(providerName, "Strip({0}, B, {1})"));
            }
            break;
          case "TrimLeft":
            if (memberInfo.ToString().EndsWith("(Char[])", StringComparison.CurrentCultureIgnoreCase) || memberInfo.ToString().EndsWith("System.Nullable`1[System.Char])", StringComparison.CurrentCultureIgnoreCase)) {
              return GetExpression<T>(() => new Sql.ExpressionAttribute(providerName, "Strip({0}, L, {1})"));
            }
            break;
          case "TrimRight":
            if (memberInfo.ToString().EndsWith("(Char[])", StringComparison.CurrentCultureIgnoreCase) || memberInfo.ToString().EndsWith("System.Nullable`1[System.Char])", StringComparison.CurrentCultureIgnoreCase)) {
              return GetExpression<T>(() => new Sql.ExpressionAttribute(providerName, "Strip({0}, T, {1})"));
            }
            break;
          case "Truncate":
            if (!(type == typeof(LinqExtensions))) {
              if (typeof(T) == typeof(Sql.ExtensionAttribute)) {
                return new T[1]
                {
              (T)(Attribute)new Sql.ExtensionAttribute(providerName, "Truncate({0}, 0)")
                };
              }
              return new T[1]
              {
            (T)(Attribute)new Sql.ExpressionAttribute(providerName, "Truncate({0}, 0)")
              };
            }
            break;
          case "DateAdd":
            return GetExtension<T>(() => new Sql.ExtensionAttribute(providerName, "") {
              ServerSideOnly = false,
              PreferServerSide = false,
              BuilderType = typeof(xDateAddBuilderDB2i)
            });
          case "DatePart":
            return GetExtension<T>(() => new Sql.ExtensionAttribute(providerName, "") {
              ServerSideOnly = false,
              PreferServerSide = false,
              BuilderType = typeof(xDatePartBuilderDB2i)
            });
          case "DateDiff":
            return GetExtension<T>(() => new Sql.ExtensionAttribute(providerName, "") {
              BuilderType = typeof(xDateDiffBuilderDB2i)
            });
          case "TinyInt":
            return GetExpression<T>(() => new Sql.ExpressionAttribute(providerName, "SmallInt") {
              ServerSideOnly = true
            });
          case "Substring":
            return GetFunction<T>(() => new Sql.FunctionAttribute(providerName, "Substr") {
              PreferServerSide = true
            });
          case "Atan2":
            return GetFunction<T>(() => new Sql.FunctionAttribute(providerName, "Atan2", 1, 0));
          case "Log":
            return GetFunction<T>(() => new Sql.FunctionAttribute(providerName, "Ln"));
          case "Log10":
            return GetFunction<T>(() => new Sql.FunctionAttribute(providerName, "Log"));
          case "DefaultNChar":
          case "DefaultNVarChar":
          case "NChar":
          case "NVarChar":
            return GetFunction<T>(() => new Sql.FunctionAttribute(providerName, "Char") {
              ServerSideOnly = true
            });
          case "Replicate":
            return GetFunction<T>(() => new Sql.FunctionAttribute(providerName, "Repeat"));
        }
      }
      return new T[0];
    }

    private T[] GetExpression<T>(Func<Sql.ExpressionAttribute> build) {
      if (!(typeof(T) == typeof(Sql.ExpressionAttribute))) {
        return new T[0];
      }
      return new T[1]
      {
      (T)(object)build()
      };
    }

    private T[] GetExtension<T>(Func<Sql.ExpressionAttribute> build) {
      if (typeof(T) == typeof(Sql.ExpressionAttribute) || typeof(T) == typeof(Sql.ExtensionAttribute)) {
        return new T[1]
        {
        (T)(object)build()
        };
      }
      return new T[0];
    }

    private T[] GetFunction<T>(Func<Sql.ExpressionAttribute> build) {
      if (typeof(T) == typeof(Sql.ExpressionAttribute) || typeof(T) == typeof(Sql.FunctionAttribute)) {
        return new T[1]
        {
        (T)(object)build()
        };
      }
      return new T[0];
    }

    public MemberInfo[] GetDynamicColumns(Type type) => new MemberInfo[0];

    public T[] GetAttributes<T>(Type type, bool inherit = true) where T : Attribute => new T[0];
  }

}
