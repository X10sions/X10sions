using IQToolkit.Data.Ado;
using IQToolkit.Data.Common;
using System;
using System.Data;
using System.Text;

namespace IQToolkit.Data.Access {

  public class AccessTypeSystem : SqlDbTypeSystem {
    public override int StringDefaultSize => 2000;
    public override int BinaryDefaultSize => 4000;

    public override QueryType GetQueryType(string typeName, string[] args, bool isNotNull) {
      if (string.Compare(typeName, "Memo", StringComparison.OrdinalIgnoreCase) == 0) {
        return base.GetQueryType("varchar", new[] { "max" }, isNotNull);
      }
      return base.GetQueryType(typeName, args, isNotNull);
    }

    public override SqlDbType GetSqlDbType(string typeName) {
      if (string.Compare(typeName, "Memo", StringComparison.OrdinalIgnoreCase) == 0) {
        return SqlDbType.VarChar;
      } else if (string.Compare(typeName, "Currency", StringComparison.OrdinalIgnoreCase) == 0) {
        return SqlDbType.Decimal;
      } else if (string.Compare(typeName, "ReplicationID", StringComparison.OrdinalIgnoreCase) == 0) {
        return SqlDbType.UniqueIdentifier;
      } else if (string.Compare(typeName, "YesNo", StringComparison.OrdinalIgnoreCase) == 0) {
        return SqlDbType.Bit;
      } else if (string.Compare(typeName, "LongInteger", StringComparison.OrdinalIgnoreCase) == 0) {
        return SqlDbType.BigInt;
      } else if (string.Compare(typeName, "VarWChar", StringComparison.OrdinalIgnoreCase) == 0) {
        return SqlDbType.NVarChar;
      } else {
        return base.GetSqlDbType(typeName);
      }
    }

    public override string Format(QueryType type, bool suppressSize) {
      var sb = new StringBuilder();
      var sqlDbType = ((SqlQueryType)type).SqlDbType;

      switch (sqlDbType) {
        case SqlDbType.BigInt:
        case SqlDbType.Bit:
        case SqlDbType.DateTime:
        case SqlDbType.Int:
        case SqlDbType.Money:
        case SqlDbType.SmallDateTime:
        case SqlDbType.SmallInt:
        case SqlDbType.SmallMoney:
        case SqlDbType.Timestamp:
        case SqlDbType.TinyInt:
        case SqlDbType.UniqueIdentifier:
        case SqlDbType.Variant:
        case SqlDbType.Xml:
          sb.Append(sqlDbType);
          break;
        case SqlDbType.Binary:
        case SqlDbType.Char:
        case SqlDbType.NChar:
          sb.Append(sqlDbType);
          if (type.Length > 0 && !suppressSize) {
            sb.Append("(");
            sb.Append(type.Length);
            sb.Append(")");
          }
          break;
        case SqlDbType.Image:
        case SqlDbType.NText:
        case SqlDbType.NVarChar:
        case SqlDbType.Text:
        case SqlDbType.VarBinary:
        case SqlDbType.VarChar:
          sb.Append(sqlDbType);
          if (type.Length > 0 && !suppressSize) {
            sb.Append("(");
            sb.Append(type.Length);
            sb.Append(")");
          }
          break;
        case SqlDbType.Decimal:
          sb.Append("Currency");
          break;
        case SqlDbType.Float:
        case SqlDbType.Real:
          sb.Append(sqlDbType);
          if (type.Precision != 0) {
            sb.Append("(");
            sb.Append(type.Precision);
            if (type.Scale != 0) {
              sb.Append(",");
              sb.Append(type.Scale);
            }
            sb.Append(")");
          }
          break;
      }

      return sb.ToString();
    }
  }
}