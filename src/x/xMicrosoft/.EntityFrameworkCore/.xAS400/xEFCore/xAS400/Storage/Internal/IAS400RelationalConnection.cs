using Microsoft.EntityFrameworkCore.Storage;

namespace xEFCore.xAS400.Storage.Internal {
  public interface IAS400RelationalConnection : IRelationalConnection {
    IAS400RelationalConnection CreateMasterConnection();
  }
}
