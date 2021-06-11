using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore {
  public static class DbContextOptionsBuilderExtensions {

    public static void ConfigureWarnings(this DbContextOptionsBuilder optionsBuilder) {

      var coreOptionsExtension = optionsBuilder.Options.FindExtension<CoreOptionsExtension>() ?? new CoreOptionsExtension();
      coreOptionsExtension = coreOptionsExtension.WithWarningsConfiguration(
          coreOptionsExtension.WarningsConfiguration.TryWithExplicit(
              RelationalEventId.AmbientTransactionWarning, WarningBehavior.Throw));
      ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(coreOptionsExtension);
    }

  }
}