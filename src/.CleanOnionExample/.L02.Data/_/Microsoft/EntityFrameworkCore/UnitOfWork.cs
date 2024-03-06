namespace Microsoft.EntityFrameworkCore;

public interface IUnitOfWork : IDisposable {
  Task Rollback();
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

internal class UnitOfWork : IUnitOfWork {
  public UnitOfWork(DbContext dbContext) => this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

  private readonly DbContext dbContext;
  private bool disposed;

  public Task Rollback() => Task.CompletedTask;

  public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => dbContext.SaveChangesAsync(cancellationToken);

  public void Dispose() {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing) {
    if (disposed) {
      return;
    }
    if (disposing) {
      dbContext.Dispose();
    }
    disposed = true;
  }

}
