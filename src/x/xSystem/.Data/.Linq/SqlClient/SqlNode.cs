using System.Diagnostics;
using System.Linq.Expressions;

namespace System.Data.Linq.SqlClient {

  [DebuggerDisplay("text = {Text}, \r\nsource = {SourceExpression}")]
  internal abstract class SqlNode {
    internal Expression SourceExpression { get; private set; }
    internal SqlNodeType NodeType { get; }
    internal SqlNode(SqlNodeType nodeType, Expression sourceExpression) {
      NodeType = nodeType;
      SourceExpression = sourceExpression;
    }

    internal void ClearSourceExpression() => SourceExpression = null;
#if DEBUG
        private static DbFormatter formatter;
        internal static DbFormatter Formatter {
            get { return formatter; }
            set { formatter = value; }
        }

        internal string Text {
            get {
                if (Formatter == null)
                    return "SqlNode.Formatter is not assigned";
                return SqlNode.Formatter.Format(this, true);
            }
        }
#endif
  }

}
