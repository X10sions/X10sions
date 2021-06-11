using System.Collections;
using System.Data.Common;
using System.IO;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider {
  internal interface IProvider : IDisposable {
    TextWriter Log { get; set; }
    DbConnection Connection { get; }
    DbTransaction Transaction { get; set; }
    int CommandTimeout { get; set; }
    void Initialize(IDataServices dataServices, object connection);
    void ClearConnection();
    void CreateDatabase();
    void DeleteDatabase();
    bool DatabaseExists();
    IExecuteResult Execute(Expression query);
    ICompiledQuery Compile(Expression query);
    IEnumerable Translate(Type elementType, DbDataReader reader);
    IMultipleResults Translate(DbDataReader reader);
    string GetQueryText(Expression query);
    DbCommand GetCommand(Expression query);
  }
}