using Microsoft.EntityFrameworkCore;
using X10sions.Wsus.Data.Models.Susdb;

namespace X10sions.Wsus.Data {
  public class SusdbDbContext : DbContext {
    public SusdbDbContext(DbContextOptions<SusdbDbContext> options)
        : base(options) {
    }

    public DbSet<ComputerTarget> ComputerTarget { get; set; } = default!;
    public DbSet<ComputerTargetDetail> ComputerTargetDetail { get; set; } = default!;
  }
}
