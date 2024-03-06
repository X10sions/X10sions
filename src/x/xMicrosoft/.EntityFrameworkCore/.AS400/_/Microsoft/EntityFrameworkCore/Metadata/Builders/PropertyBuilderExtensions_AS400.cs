using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using xEFCore;
using xEFCore.xAS400.Metadata;
using xEFCore.xAS400.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders {
  public static class PropertyBuilderExtensions_AS400 {

    public static PropertyBuilder ForAS400UseSequenceHiLo(
        [NotNull] this PropertyBuilder propertyBuilder,
        [CanBeNull] string name = null,
        [CanBeNull] string schema = null) {
      Check.NotNull(propertyBuilder, nameof(propertyBuilder));
      Check.NullButNotEmpty(name, nameof(name));
      Check.NullButNotEmpty(schema, nameof(schema));
      var property = propertyBuilder.Metadata;
      name = name ?? EFCoreConstants.Metadata.DefaultHiLoSequenceName;
      var model = property.DeclaringEntityType.Model;
      if (model.AS400().FindSequence(name, schema) == null) {
        model.AS400().GetOrAddSequence(name, schema).IncrementBy = 10;
      }
      GetAS400InternalBuilder(propertyBuilder).ValueGenerationStrategy(AS400ValueGenerationStrategy.SequenceHiLo);
      property.AS400().HiLoSequenceName = name;
      property.AS400().HiLoSequenceSchema = schema;
      return propertyBuilder;
    }

    public static PropertyBuilder<TProperty> ForAS400UseSequenceHiLo<TProperty>(
        [NotNull] this PropertyBuilder<TProperty> propertyBuilder,
        [CanBeNull] string name = null,
        [CanBeNull] string schema = null)
        => (PropertyBuilder<TProperty>)ForAS400UseSequenceHiLo((PropertyBuilder)propertyBuilder, name, schema);
    public static PropertyBuilder UseAS400IdentityColumn(
        [NotNull] this PropertyBuilder propertyBuilder) {
      Check.NotNull(propertyBuilder, nameof(propertyBuilder));
      GetAS400InternalBuilder(propertyBuilder).ValueGenerationStrategy(AS400ValueGenerationStrategy.IdentityColumn);
      return propertyBuilder;
    }

    public static PropertyBuilder<TProperty> UseAS400IdentityColumn<TProperty>(
        [NotNull] this PropertyBuilder<TProperty> propertyBuilder)
        => (PropertyBuilder<TProperty>)UseAS400IdentityColumn((PropertyBuilder)propertyBuilder);

    static AS400PropertyBuilderAnnotations GetAS400InternalBuilder(PropertyBuilder propertyBuilder)
       => propertyBuilder.GetInfrastructure<InternalPropertyBuilder>().AS400(ConfigurationSource.Explicit);

  }
}