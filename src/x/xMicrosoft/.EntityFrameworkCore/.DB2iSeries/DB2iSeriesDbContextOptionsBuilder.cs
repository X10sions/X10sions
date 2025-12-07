using Microsoft.EntityFrameworkCore;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries;

public class DB2iSeriesDbContextOptionsBuilder {
  private readonly DbContextOptionsBuilder _optionsBuilder;

  public DB2iSeriesDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder) {
    _optionsBuilder = optionsBuilder;
  }

  public virtual DbContextOptionsBuilder OptionsBuilder => _optionsBuilder;
}
