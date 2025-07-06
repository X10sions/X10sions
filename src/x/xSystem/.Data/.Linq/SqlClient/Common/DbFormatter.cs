using System.Data.Linq.SqlClient.Common;

namespace System.Data.Linq.SqlClient.Query {
  internal abstract class DbFormatter {
    internal abstract string Format(SqlNode node, bool isDebug);
    internal abstract string Format(SqlNode node);
  }

}
