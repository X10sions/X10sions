using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Query;
//TODO  using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using Microsoft.EntityFrameworkCore.Query.Sql;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using xEFCore.xAS400.Infrastructure.Internal;
using xEFCore.xAS400.Internal;
using xEFCore.xAS400.Metadata.Conventions;
//TODO  using xEFCore.xAS400.Migrations.Internal;
//TODO  using xEFCore.xAS400.Migrations;
using xEFCore.xAS400.Query.ExpressionTranslators.Internal;
using xEFCore.xAS400.Query.Internal;
using xEFCore.xAS400.Query.Sql.Internal;
using xEFCore.xAS400.Storage.Internal;
using xEFCore.xAS400.Update.Internal;
using xEFCore.xAS400.ValueGeneration.Internal;

namespace Microsoft.Extensions.DependencyInjection {
  public static class IServiceCollectionExtensions_AS400 {

    public static IServiceCollection AddEntityFrameworkAS400x([NotNull] this IServiceCollection serviceCollection) {
      Check.NotNull(serviceCollection, nameof(serviceCollection));
      var builder = new EntityFrameworkRelationalServicesBuilder(serviceCollection);
      builder.TryAdd<IDatabaseProvider, DatabaseProvider<AS400OptionsExtension>>();
      builder.TryAdd<IValueGeneratorCache>(p => p.GetService<IAS400ValueGeneratorCache>());
      builder.TryAdd<IRelationalTypeMapper, AS400TypeMapper>();
      builder.TryAdd<ISqlGenerationHelper, AS400SqlGenerationHelper>();
      //TODO  builder.TryAdd<IMigrationsAnnotationProvider, AS400MigrationsAnnotationProvider>();
      builder.TryAdd<IRelationalValueBufferFactoryFactory, UntypedRelationalValueBufferFactoryFactory>();
      builder.TryAdd<IModelValidator, AS400ModelValidator>();
      builder.TryAdd<IConventionSetBuilder, AS400ConventionSetBuilder>();
      builder.TryAdd<IUpdateSqlGenerator>(p => p.GetService<IAS400UpdateSqlGenerator>());
      builder.TryAdd<IModificationCommandBatchFactory, AS400ModificationCommandBatchFactory>();
      builder.TryAdd<IValueGeneratorSelector, AS400ValueGeneratorSelector>();
      builder.TryAdd<IRelationalConnection>(p => p.GetService<IAS400RelationalConnection>());
      //TODO  builder.TryAdd<IMigrationsSqlGenerator, AS400MigrationsSqlGenerator>();
      //TODO  builder.TryAdd<IRelationalDatabaseCreator, AS400DatabaseCreator>();
      //TODO  builder.TryAdd<IHistoryRepository, AS400HistoryRepository>();
      builder.TryAdd<ICompiledQueryCacheKeyGenerator, AS400CompiledQueryCacheKeyGenerator>();
      builder.TryAdd<IExecutionStrategyFactory, AS400ExecutionStrategyFactory>();
      builder.TryAdd<IQueryCompilationContextFactory, AS400QueryCompilationContextFactory>();
      builder.TryAdd<IMemberTranslator, AS400CompositeMemberTranslator>();
      builder.TryAdd<ICompositeMethodCallTranslator, AS400CompositeMethodCallTranslator>();
      builder.TryAdd<IQuerySqlGeneratorFactory, AS400QuerySqlGeneratorFactory>();
      builder.TryAdd<ISingletonOptions, IAS400Options>(p => p.GetService<IAS400Options>());
      builder.TryAddProviderSpecificServices(delegate (ServiceCollectionMap b) {
        b.TryAddSingleton<IAS400ValueGeneratorCache, AS400ValueGeneratorCache>();
        b.TryAddSingleton<IAS400Options, AS400Options>();
        b.TryAddScoped<IAS400UpdateSqlGenerator, AS400UpdateSqlGenerator>();
        b.TryAddScoped<IAS400SequenceValueGeneratorFactory, AS400SequenceValueGeneratorFactory>();
        b.TryAddScoped<IAS400RelationalConnection, AS400RelationalConnection>();
      });
      builder.TryAddCoreServices();
      return serviceCollection;
    }


    //public static TConfig ConfigurePOCO<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new() {
    //  //example: services.ConfigurePOCO<MySettings>(Configuration.GetSection("MySettings"));
    //  return services.ConfigurePOCO(configuration, new TConfig());
    //}

    //public static TConfig ConfigurePOCO<TConfig>(this IServiceCollection services, IConfiguration configuration, Func<TConfig> pocoProvider) where TConfig : class {
    //  if (pocoProvider == null) throw new ArgumentNullException(nameof(pocoProvider));
    //  return services.ConfigurePOCO(configuration, pocoProvider());

    //}

    //public static TConfig ConfigurePOCO<TConfig>(this IServiceCollection services, IConfiguration configuration, TConfig config) where TConfig : class {
    //  if (services == null) throw new ArgumentNullException(nameof(services));
    //  if (configuration == null) throw new ArgumentNullException(nameof(configuration));
    //  if (config == null) throw new ArgumentNullException(nameof(config));

    //  configuration.Bind(config);
    //  services.AddSingleton(config);
    //  return config;
    //}

    //static void test(this IServiceCollection services, IConfiguration configuration) {
    //  var mySettings = new MySettings("foo");
    //  services.ConfigurePOCO(configuration.GetSection("MySettings"), mySettings);
    //  services.ConfigurePOCO(configuration.GetSection("MySettings"), () => new MySettings("foo"));
    //}

  }



}