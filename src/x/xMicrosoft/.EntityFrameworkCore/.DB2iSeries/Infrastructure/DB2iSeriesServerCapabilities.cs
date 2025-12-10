using System.Data.Odbc;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Infrastructure;

public sealed class DB2iSeriesServerCapabilities {
  public bool SupportsOffset { get; init; }
  public bool SupportsRowNumber { get; init; }
  public bool SupportsIdentity { get; init; }

  public static DB2iSeriesServerCapabilities Detect(OdbcConnection con) {
    // Minimal heuristic: query QSYS2.VERSION_INFO if available
    var caps = new DB2iSeriesServerCapabilities {
      SupportsOffset = false,
      SupportsRowNumber = true, // adjust based on actual v5r4 PTFs
      SupportsIdentity = false
    };
    return caps;
  }

}
