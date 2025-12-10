using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Odbc;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries;

public class DB2iSeriesOptionsExtension : RelationalOptionsExtension {
  private string _connectionString;
  private NamingConvention _namingConvention;
  private DbContextOptionsExtensionInfo _info;

  public DB2iSeriesOptionsExtension() { }

  protected DB2iSeriesOptionsExtension(DB2iSeriesOptionsExtension copyFrom) : base(copyFrom) {
    _connectionString = copyFrom._connectionString;
    _namingConvention = copyFrom._namingConvention;
  }

  public override DbContextOptionsExtensionInfo Info => _info ??= new ExtensionInfo(this);

  protected override RelationalOptionsExtension Clone() => new DB2iSeriesOptionsExtension(this);

  public virtual DB2iSeriesOptionsExtension WithConnectionString(string connectionString) {
    var clone = (DB2iSeriesOptionsExtension)Clone();
    clone._connectionString = connectionString;
    return clone;
  }

  public override string ConnectionString => _connectionString;
  public OdbcConnection? ExistingConnection { get; set; }


  public DB2iSeriesOptionsExtension WithNamingConvention(NamingConvention namingConvention = NamingConvention.SYS) {
    var clone = new DB2iSeriesOptionsExtension(this);
    clone._namingConvention = namingConvention;
    return clone;
  }

  public NamingConvention NamingConvention => _namingConvention;


  public override void ApplyServices(IServiceCollection services) => services.AddEntityFrameworkDB2iSeries();

  private sealed class ExtensionInfo : RelationalExtensionInfo {
    public ExtensionInfo(IDbContextOptionsExtension extension) : base(extension) { }
    public override bool IsDatabaseProvider => true;
    public override string LogFragment => "using IBM DB2iSeries";
    public override void PopulateDebugInfo(IDictionary<string, string> debugInfo) {
      debugInfo["DB2iSeries:Provider"] = "1";
    }
  }
}
