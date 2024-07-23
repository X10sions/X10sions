using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace RCommon.Persistence.EFCore;

public abstract class RCommonDbContext : DbContext, IDataStore {

  public RCommonDbContext(DbContextOptions options) : base(options) {
    if (options is null) {
      throw new ArgumentNullException(nameof(options));
    }
  }

  public DbConnection GetDbConnection() => base.Database.GetDbConnection();
}