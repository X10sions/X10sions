using System.Data.Common;

namespace RCommon.Persistence.Sql;

public class RDbConnectionOptions {
  public RDbConnectionOptions() {  }

  public DbProviderFactory DbFactory { get; set; }
  public string ConnectionString { get; set; }
}
