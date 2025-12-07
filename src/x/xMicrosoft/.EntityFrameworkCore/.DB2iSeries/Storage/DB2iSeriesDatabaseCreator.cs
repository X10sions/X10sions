using Microsoft.EntityFrameworkCore.Storage;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Storage;

public class DB2iSeriesDatabaseCreator : RelationalDatabaseCreator {
  public DB2iSeriesDatabaseCreator(      RelationalDatabaseCreatorDependencies dependencies)      : base(dependencies) {  }

  public override bool Exists() {
    // DB2iSeries libraries (schemas) typically exist already
    return true;
  }

  public override Task<bool> ExistsAsync(CancellationToken cancellationToken = default) => Task.FromResult(true);

  public override void Create() {
    // Library creation handled externally on DB2iSeries
  }

  public override Task CreateAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

  public override void Delete() {
    //throw new NotImplementedException("Not implemented for safety");
    // Not implemented for safety
  }

  public override Task DeleteAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

  public override bool HasTables() => throw new NotImplementedException();

}