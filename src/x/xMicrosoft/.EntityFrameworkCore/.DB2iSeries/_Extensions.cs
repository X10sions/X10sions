using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore {
  public static class _Extensions {

    static void ConfigureWarningsDefault(this DbContextOptionsBuilder optionsBuilder) {
      var coreOptionsExtension = optionsBuilder.Options.FindExtension<CoreOptionsExtension>() ?? new CoreOptionsExtension();
      coreOptionsExtension = RelationalOptionsExtension.WithDefaultWarningConfiguration(coreOptionsExtension);
      //coreOptionsExtension = coreOptionsExtension.WithWarningsConfiguration(coreOptionsExtension.WarningsConfiguration.TryWithExplicit(DB2iSeriesEventId.ConflictingValueGenerationStrategiesWarning, WarningBehavior.Throw));
      ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(coreOptionsExtension);
    }

    static T GetOrCreateExtension<T>(this DbContextOptionsBuilder optionsBuilder, Func<T, T>? extensionActions = null) where T : class, IDbContextOptionsExtension, new() {
      var extension = optionsBuilder.Options.FindExtension<T>() ?? new T();
      return extensionActions == null ? extension : extensionActions(extension);
    }

    public static DbContextOptionsBuilder Use<TBuilder, TExtension>(this DbContextOptionsBuilder optionsBuilder
      , Func<DbContextOptionsBuilder, TBuilder> newDbContextOptionsBuilder
      , Action<TBuilder>? optionsAction = null
      , Func<TExtension, TExtension>? extensionActions = null)
      where TBuilder : IRelationalDbContextOptionsBuilderInfrastructure
      where TExtension : class, IDbContextOptionsExtension, new() {
      Check.NotNull(optionsBuilder, nameof(optionsBuilder));
      var extension = optionsBuilder.GetOrCreateExtension(extensionActions);
      ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
      optionsBuilder.ConfigureWarningsDefault();
      optionsAction?.Invoke(newDbContextOptionsBuilder(optionsBuilder));
      return optionsBuilder;
    }

  }
}
