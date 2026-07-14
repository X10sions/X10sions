using IQToolkit.Data.Common;
using System.Data;

namespace IQToolkit.Data.Ado {

  public class SqlQueryType : QueryType {
    /// <summary>
    /// Construct a <see cref="SqlQueryType"/>
    /// </summary>
    public SqlQueryType(SqlDbType sqlDbType, bool notNull, int length, short precision, short scale) {
      SqlDbType = sqlDbType;
      NotNull = notNull;
      Length = length;
      Precision = precision;
      Scale = scale;
    }

    public SqlDbType SqlDbType { get; }
    public override int Length { get; }
    public override bool NotNull { get; }
    public override short Precision { get; }
    public override short Scale { get; }
  }
}