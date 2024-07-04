using xSystem.Data.Linq.SqlClient.Common;

namespace xSystem.Data.Linq.SqlClient.Query {
  internal abstract class DbFormatter {
    internal abstract string Format(SqlNode node, bool isDebug);
    internal abstract string Format(SqlNode node);
  }

}
