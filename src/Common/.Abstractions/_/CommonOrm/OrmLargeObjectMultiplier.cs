using System.ComponentModel;

namespace CommonOrm {
  public enum OrmLargeObjectMultiplier {
    _None,
    [Description("            1 024")] Kilo,
    [Description("        1 048 576")] Mega,
    [Description("    1 073 741 824")] Giga,
    [Description("1 099 511 627 776")] Tera
  }
}