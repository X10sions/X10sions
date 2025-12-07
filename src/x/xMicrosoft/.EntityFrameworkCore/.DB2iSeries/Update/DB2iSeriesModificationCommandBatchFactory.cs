using Microsoft.EntityFrameworkCore.Update;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Update;

public class DB2iSeriesModificationCommandBatchFactory : IModificationCommandBatchFactory {
  private readonly ModificationCommandBatchFactoryDependencies _dependencies;

  public DB2iSeriesModificationCommandBatchFactory(
      ModificationCommandBatchFactoryDependencies dependencies) {
    _dependencies = dependencies;
  }

  public virtual ModificationCommandBatch Create() => new SingularModificationCommandBatch(_dependencies);

}