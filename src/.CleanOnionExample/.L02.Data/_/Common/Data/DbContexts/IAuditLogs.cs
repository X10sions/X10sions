using Common.Data.Entities;
using Common.DTOs;
using Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace Common.Data.DbContexts;

public interface IAuditLogs {
  public DbSet<Audit> AuditLogs { get; set; }
}

public static class IAuditLogsExtensions {

  public static async Task<int> AuditableSaveChangesAsync<T>(this T dbContext, string? userId = null, CancellationToken cancellationToken = default) where T : DbContext, IAuditLogs {
    var auditEntries = dbContext.OnBeforeAuditableSaveChanges(userId);
    var result = await dbContext.SaveChangesAsync(cancellationToken);
    await dbContext.OnAfterAuditableSaveChangesAsync(auditEntries, cancellationToken);
    return result;
  }

  public static List<AuditEntry> OnBeforeAuditableSaveChanges<T>(this T dbContext, string userId) where T : DbContext, IAuditLogs {
    dbContext.ChangeTracker.DetectChanges();
    var auditEntries = new List<AuditEntry>();
    foreach (var entry in dbContext.ChangeTracker.Entries()) {
      if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
        continue;
      var auditEntry = new AuditEntry(entry);
      auditEntry.TableName = entry.Entity.GetType().Name;
      auditEntry.UserId = userId;
      auditEntries.Add(auditEntry);
      foreach (var property in entry.Properties) {
        if (property.IsTemporary) {
          auditEntry.TemporaryProperties.Add(property);
          continue;
        }
        string propertyName = property.Metadata.Name;
        if (property.Metadata.IsPrimaryKey()) {
          auditEntry.KeyValues[propertyName] = property.CurrentValue;
          continue;
        }
        switch (entry.State) {
          case EntityState.Added:
            auditEntry.AuditType = AuditType.Create;
            auditEntry.NewValues[propertyName] = property.CurrentValue;
            break;
          case EntityState.Deleted:
            auditEntry.AuditType = AuditType.Delete;
            auditEntry.OldValues[propertyName] = property.OriginalValue;
            break;
          case EntityState.Modified:
            if (property.IsModified) {
              auditEntry.ChangedColumns.Add(propertyName);
              auditEntry.AuditType = AuditType.Update;
              auditEntry.OldValues[propertyName] = property.OriginalValue;
              auditEntry.NewValues[propertyName] = property.CurrentValue;
            }
            break;
        }
      }
    }
    foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties)) {
      dbContext.AuditLogs.Add(auditEntry.ToAudit());
    }
    return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
  }

  public static Task OnAfterAuditableSaveChangesAsync<T>(this T dbContext, List<AuditEntry> auditEntries, CancellationToken cancellationToken = default) where T : DbContext, IAuditLogs {
    if (auditEntries == null || auditEntries.Count == 0)
      return Task.CompletedTask;
    foreach (var auditEntry in auditEntries) {
      foreach (var prop in auditEntry.TemporaryProperties) {
        if (prop.Metadata.IsPrimaryKey()) {
          auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
        } else {
          auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
        }
      }
      dbContext.AuditLogs.Add(auditEntry.ToAudit());
    }
    return dbContext.SaveChangesAsync(cancellationToken);
  }

}
