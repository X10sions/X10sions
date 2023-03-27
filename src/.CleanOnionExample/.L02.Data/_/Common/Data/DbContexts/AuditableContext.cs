using Common.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Common.Data.DbContexts;

public abstract class AuditableContext : DbContext, IAuditLogs {
  public AuditableContext(DbContextOptions options) : base(options) {
  }

  public DbSet<Audit> AuditLogs { get; set; }

  public virtual async Task<int> SaveChangesAsync(string? userId = null, CancellationToken cancellationToken = default)
    => await this.AuditableSaveChangesAsync(userId, cancellationToken);

}
