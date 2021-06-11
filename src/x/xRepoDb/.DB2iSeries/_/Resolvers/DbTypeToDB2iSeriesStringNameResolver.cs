using RepoDb.Interfaces;
using System.Data;

namespace RepoDb.Resolvers {
  public class DbTypeToDB2iSeriesStringNameResolver : IResolver<DbType, string> {
    public string Resolve(DbType dbType) {
      switch (dbType) {
        case DbType.AnsiString: return "VARCHAR";
        case DbType.AnsiStringFixedLength: return "CHAR";
        case DbType.Binary: return "BINARY";
        case DbType.Boolean: return "BIT";
        case DbType.Byte: return "TINYINT";
        case DbType.Currency: return "DECIMAL";
        case DbType.Date: return "DATE";
        case DbType.DateTime: return "DATETIME";
        case DbType.DateTime2: return "DATETIME2";
        case DbType.DateTimeOffset: return "DATETIMEOFFSET";
        case DbType.Decimal: return "DECIMAL";
        case DbType.Double: return "FLOAT";
        case DbType.Guid: return "UNIQUEIDENTIFIER";
        case DbType.Int16: return "SMALLINT";
        case DbType.Int32: return "INT";
        case DbType.Int64: return "BIGINT";
        case DbType.Object: return "OBJECT";
        case DbType.SByte: return "TINYINT";
        case DbType.Single: return "REAL";
        case DbType.String: return "VARCHAR";
        case DbType.StringFixedLength: return "CHAR";
        case DbType.Time: return "TIME";
        case DbType.Xml: return "XML";
        case DbType.UInt16: return "SMALLINT";
        case DbType.UInt32: return "INT";
        case DbType.UInt64: return "BIGINT";
        case DbType.VarNumeric: return "NUMERIC";
        default: return "VARCHAR";
      }
    }
  }
}
