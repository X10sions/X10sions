using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using xEFCore.xAS400.Infrastructure.Internal;
using xEFCore.xAS400.Metadata.Conventions.Internal;
using xEFCore.xAS400.Storage.Internal;

namespace xEFCore.xAS400.Metadata.Conventions {
  public class AS400ConventionSetBuilder : RelationalConventionSetBuilder {

    public AS400ConventionSetBuilder(
        [NotNull] RelationalConventionSetBuilderDependencies dependencies,
        [NotNull] ISqlGenerationHelper sqlGenerationHelper)
        : base(dependencies) {
      _sqlGenerationHelper = sqlGenerationHelper;
    }

    readonly IAS400Options _options;

    readonly ISqlGenerationHelper _sqlGenerationHelper;

    public override ConventionSet AddConventions(ConventionSet conventionSet) {
      Check.NotNull(conventionSet, nameof(conventionSet));

      base.AddConventions(conventionSet);

      var valueGenerationStrategyConvention = new AS400ValueGenerationStrategyConvention();
      conventionSet.ModelInitializedConventions.Add(valueGenerationStrategyConvention);

      ValueGeneratorConvention valueGeneratorConvention = new AS400ValueGeneratorConvention();
      ReplaceConvention(conventionSet.BaseEntityTypeChangedConventions, valueGeneratorConvention);

      //var AS400InMemoryTablesConvention = new AS400MemoryOptimizedTablesConvention();
      //conventionSet.EntityTypeAnnotationChangedConventions.Add(AS400InMemoryTablesConvention);

      ReplaceConvention(conventionSet.PrimaryKeyChangedConventions, valueGeneratorConvention);

      //conventionSet.KeyAddedConventions.Add(AS400InMemoryTablesConvention);

      ReplaceConvention(conventionSet.ForeignKeyAddedConventions, valueGeneratorConvention);

      ReplaceConvention(conventionSet.ForeignKeyRemovedConventions, valueGeneratorConvention);

      var AS400IndexConvention = new AS400IndexConvention(_sqlGenerationHelper);
      //conventionSet.IndexAddedConventions.Add(AS400InMemoryTablesConvention);
      conventionSet.IndexAddedConventions.Add(AS400IndexConvention);

      conventionSet.IndexUniquenessChangedConventions.Add(AS400IndexConvention);

      conventionSet.IndexAnnotationChangedConventions.Add(AS400IndexConvention);

      conventionSet.PropertyNullabilityChangedConventions.Add(AS400IndexConvention);

      conventionSet.PropertyAnnotationChangedConventions.Add(AS400IndexConvention);
      conventionSet.PropertyAnnotationChangedConventions.Add((AS400ValueGeneratorConvention)valueGeneratorConvention);

      return conventionSet;
    }

    public static ConventionSet Build(IAS400Options options) {
      var typeMapper = new AS400TypeMapper(new RelationalTypeMapperDependencies());

      return new AS400ConventionSetBuilder(
        new RelationalConventionSetBuilderDependencies(typeMapper, null, null),
        new AS400SqlGenerationHelper(new RelationalSqlGenerationHelperDependencies(), options))
        .AddConventions(
        new CoreConventionSetBuilder(
          new CoreConventionSetBuilderDependencies(typeMapper))
          .CreateConventionSet());
    }

  }
}
