using IQToolkit.Data.Common;
using System;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace IQToolkit.Data.Ado {

  /// <summary>
  /// A <see cref="QueryTypeSystem"/> for types based on <see cref="SqlDbType"/>.
  /// Default parser, format implementations assume a type system similar to TSQL.
  /// </summary>
  public class SqlDbTypeSystem : QueryTypeSystem {
    public override QueryType Parse(string typeDeclaration) {
      string[] args = null;
      string typeName = null;
      string remainder = null;

      var openParen = typeDeclaration.IndexOf('(');
      if (openParen >= 0) {
        typeName = typeDeclaration.Substring(0, openParen).Trim();

        var closeParen = typeDeclaration.IndexOf(')', openParen);
        if (closeParen < openParen) closeParen = typeDeclaration.Length;

        var argstr = typeDeclaration.Substring(openParen + 1, closeParen - (openParen + 1));
        args = argstr.Split(',');
        remainder = typeDeclaration.Substring(closeParen + 1);
      } else {
        var space = typeDeclaration.IndexOf(' ');
        if (space >= 0) {
          typeName = typeDeclaration.Substring(0, space);
          remainder = typeDeclaration.Substring(space + 1).Trim();
        } else {
          typeName = typeDeclaration;
        }
      }

      var isNotNull = (remainder != null) ? remainder.ToUpper().Contains("NOT NULL") : false;

      return GetQueryType(typeName, args, isNotNull);
    }

    /// <summary>
    /// Gets the <see cref="QueryType"/> for a know database type.
    /// This API does not parse the type name.
    /// Arguments to the type are specified by the <see cref="P:args"/> parameter.
    /// </summary>
    /// <param name="typeName">The base name of a type in the databases language.</param>
    /// <param name="args">Any additional arguments (like length of a text type)</param>
    /// <param name="isNotNull">Determines if the type cannot be null.</param>
    public virtual QueryType GetQueryType(string typeName, string[] args, bool isNotNull) {
      if (string.Compare(typeName, "rowversion", StringComparison.OrdinalIgnoreCase) == 0) {
        typeName = "Timestamp";
      }

      if (string.Compare(typeName, "numeric", StringComparison.OrdinalIgnoreCase) == 0) {
        typeName = "Decimal";
      }

      if (string.Compare(typeName, "sql_variant", StringComparison.OrdinalIgnoreCase) == 0) {
        typeName = "Variant";
      }

      var dbType = GetSqlDbType(typeName);

      var length = 0;
      short precision = 0;
      short scale = 0;

      switch (dbType) {
        case SqlDbType.Binary:
        case SqlDbType.Char:
        case SqlDbType.Image:
        case SqlDbType.NChar:
        case SqlDbType.NVarChar:
        case SqlDbType.VarBinary:
        case SqlDbType.VarChar:
          if (args == null || args.Length < 1) {
            length = 80;
          } else if (string.Compare(args[0], "max", StringComparison.OrdinalIgnoreCase) == 0) {
            length = int.MaxValue;
          } else {
            length = int.Parse(args[0]);
          }
          break;
        case SqlDbType.Money:
          if (args == null || args.Length < 1) {
            precision = 29;
          } else {
            precision = short.Parse(args[0], NumberFormatInfo.InvariantInfo);
          }
          if (args == null || args.Length < 2) {
            scale = 4;
          } else {
            scale = short.Parse(args[1], NumberFormatInfo.InvariantInfo);
          }
          break;
        case SqlDbType.Decimal:
          if (args == null || args.Length < 1) {
            precision = 29;
          } else {
            precision = short.Parse(args[0], NumberFormatInfo.InvariantInfo);
          }
          if (args == null || args.Length < 2) {
            scale = 0;
          } else {
            scale = short.Parse(args[1], NumberFormatInfo.InvariantInfo);
          }
          break;
        case SqlDbType.Float:
        case SqlDbType.Real:
          if (args == null || args.Length < 1) {
            precision = 29;
          } else {
            precision = short.Parse(args[0], NumberFormatInfo.InvariantInfo);
          }
          break;
      }

      return NewType(dbType, isNotNull, length, precision, scale);
    }

    /// <summary>
    /// Construct a new <see cref="QueryType"/> instance from
    /// </summary>
    protected virtual QueryType NewType(SqlDbType type, bool isNotNull, int length, short precision, short scale) => new SqlQueryType(type, isNotNull, length, precision, scale);

    /// <summary>
    /// Gets the <see cref="SqlDbType"/> given the type name (same name as <see cref="SqlDbType"/> members)
    /// </summary>
    public virtual SqlDbType GetSqlDbType(string typeName) => (SqlDbType)Enum.Parse(typeof(SqlDbType), typeName, true);

    /// <summary>
    /// Default maximum size of a text data type.
    /// </summary>
    public virtual int StringDefaultSize => int.MaxValue;

    /// <summary>
    /// Default maximum size of a binary data type.
    /// </summary>
    public virtual int BinaryDefaultSize => int.MaxValue;

    /// <summary>
    /// Gets the <see cref="QueryType"/> associated with a CLR type.
    /// </summary>
    public override QueryType GetColumnType(Type type) {
      var isNotNull = type.GetTypeInfo().IsValueType && !TypeHelper.IsNullableType(type);
      type = TypeHelper.GetNonNullableType(type);

      switch (TypeHelper.GetTypeCode(type)) {
        case TypeCode.Boolean:
          return NewType(SqlDbType.Bit, isNotNull, 0, 0, 0);
        case TypeCode.SByte:
        case TypeCode.Byte:
          return NewType(SqlDbType.TinyInt, isNotNull, 0, 0, 0);
        case TypeCode.Int16:
        case TypeCode.UInt16:
          return NewType(SqlDbType.SmallInt, isNotNull, 0, 0, 0);
        case TypeCode.Int32:
        case TypeCode.UInt32:
          return NewType(SqlDbType.Int, isNotNull, 0, 0, 0);
        case TypeCode.Int64:
        case TypeCode.UInt64:
          return NewType(SqlDbType.BigInt, isNotNull, 0, 0, 0);
        case TypeCode.Single:
        case TypeCode.Double:
          return NewType(SqlDbType.Float, isNotNull, 0, 0, 0);
        case TypeCode.String:
          return NewType(SqlDbType.NVarChar, isNotNull, StringDefaultSize, 0, 0);
        case TypeCode.Char:
          return NewType(SqlDbType.NChar, isNotNull, 1, 0, 0);
        case TypeCode.DateTime:
          return NewType(SqlDbType.DateTime, isNotNull, 0, 0, 0);
        case TypeCode.Decimal:
          return NewType(SqlDbType.Decimal, isNotNull, 0, 29, 4);
        default:
          if (type == typeof(byte[]))
            return NewType(SqlDbType.VarBinary, isNotNull, BinaryDefaultSize, 0, 0);
          else if (type == typeof(Guid))
            return NewType(SqlDbType.UniqueIdentifier, isNotNull, 0, 0, 0);
          else if (type == typeof(DateTimeOffset))
            return NewType(SqlDbType.DateTimeOffset, isNotNull, 0, 0, 0);
          else if (type == typeof(TimeSpan))
            return NewType(SqlDbType.Time, isNotNull, 0, 0, 0);
          else if (type.GetTypeInfo().IsEnum)
            return NewType(SqlDbType.Int, isNotNull, 0, 0, 0);
          else
            return null;
      }
    }

    /// <summary>
    /// True if the <see cref="SqlDbType"/> is a variable length type.
    /// </summary>
    public static bool IsVariableLength(SqlDbType dbType) {
      switch (dbType) {
        case SqlDbType.Image:
        case SqlDbType.NText:
        case SqlDbType.NVarChar:
        case SqlDbType.Text:
        case SqlDbType.VarBinary:
        case SqlDbType.VarChar:
        case SqlDbType.Xml:
          return true;
        default:
          return false;
      }
    }

    /// <summary>
    /// Format the <see cref="QueryType"/> as if specified in the database language.
    /// </summary>
    public override string Format(QueryType type, bool suppressSize) {
      var sqlDbType = (SqlQueryType)type;
      var sb = new StringBuilder();
      sb.Append(sqlDbType.SqlDbType.ToString().ToUpper());

      if (sqlDbType.Length > 0 && !suppressSize) {
        if (sqlDbType.Length == int.MaxValue) {
          sb.Append("(max)");
        } else {
          sb.AppendFormat(NumberFormatInfo.InvariantInfo, "({0})", sqlDbType.Length);
        }
      } else if (sqlDbType.Precision != 0) {
        if (sqlDbType.Scale != 0) {
          sb.AppendFormat(NumberFormatInfo.InvariantInfo, "({0},{1})", sqlDbType.Precision, sqlDbType.Scale);
        } else {
          sb.AppendFormat(NumberFormatInfo.InvariantInfo, "({0})", sqlDbType.Precision);
        }
      }

      return sb.ToString();
    }
  }
}