using System.Data.Common;

namespace RCommon.Persistence;

public interface IDataStore : IAsyncDisposable {
  DbConnection GetDbConnection();
}
