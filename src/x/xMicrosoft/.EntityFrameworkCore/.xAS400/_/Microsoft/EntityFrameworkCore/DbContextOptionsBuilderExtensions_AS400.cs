using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Data.Common;
using xEFCore.xAS400.Infrastructure;
using xEFCore.xAS400.Infrastructure.Internal;

namespace Microsoft.EntityFrameworkCore {
  public static class DbContextOptionsBuilderExtensions_AS400 {

    public static DbContextOptionsBuilder UseAS400x(
      [NotNull] this DbContextOptionsBuilder optionsBuilder,
      [NotNull] string connectionString,
      [CanBeNull] Action<AS400DbContextOptionsBuilder> optionsAction = null
      ) {
      Check.NotNull(optionsBuilder, nameof(optionsBuilder));
      Check.NotEmpty(connectionString, nameof(connectionString));
      var extension = (AS400OptionsExtension)GetOrCreateExtension(optionsBuilder).WithConnectionString(connectionString);
      ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
      optionsBuilder.ConfigureWarnings();
      optionsAction?.Invoke(new AS400DbContextOptionsBuilder(optionsBuilder));
      return optionsBuilder;
    }

    public static DbContextOptionsBuilder UseAS400x(
        [NotNull] this DbContextOptionsBuilder optionsBuilder,
        [NotNull] DbConnection connection,
        [CanBeNull] Action<AS400DbContextOptionsBuilder> optionsAction = null) {
      Check.NotNull(optionsBuilder, nameof(optionsBuilder));
      Check.NotNull(connection, nameof(connection));
      var extension = (AS400OptionsExtension)GetOrCreateExtension(optionsBuilder).WithConnection(connection);
      ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
      optionsBuilder.ConfigureWarnings();
      optionsAction?.Invoke(new AS400DbContextOptionsBuilder(optionsBuilder));
      return optionsBuilder;
    }

    public static DbContextOptionsBuilder<TContext> UseAS400x<TContext>(
        [NotNull] this DbContextOptionsBuilder<TContext> optionsBuilder,
        [NotNull] string connectionString,
        [CanBeNull] Action<AS400DbContextOptionsBuilder> optionsAction = null)
        where TContext : DbContext
        => (DbContextOptionsBuilder<TContext>)UseAS400x(
            (DbContextOptionsBuilder)optionsBuilder, connectionString, optionsAction);

    public static DbContextOptionsBuilder<TContext> UseAS400x<TContext>(
        [NotNull] this DbContextOptionsBuilder<TContext> optionsBuilder,
        [NotNull] DbConnection connection,
        [CanBeNull] Action<AS400DbContextOptionsBuilder> optionsAction = null)
        where TContext : DbContext
        => (DbContextOptionsBuilder<TContext>)UseAS400x(
            (DbContextOptionsBuilder)optionsBuilder, connection, optionsAction);

    static AS400OptionsExtension GetOrCreateExtension(DbContextOptionsBuilder optionsBuilder)
       => optionsBuilder.Options.FindExtension<AS400OptionsExtension>()
          ?? new AS400OptionsExtension();

  }
}