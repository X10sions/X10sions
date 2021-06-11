using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using xEFCore;
using xEFCore.xAS400.Metadata;

namespace Microsoft.EntityFrameworkCore {
  public static class ModelBuilderExtensions_AS400 {

    public static ModelBuilder ForAS400UseSequenceHiLo(
        [NotNull] this ModelBuilder modelBuilder,
        [CanBeNull] string name = null,
        [CanBeNull] string schema = null) {
      Check.NotNull(modelBuilder, nameof(modelBuilder));
      Check.NullButNotEmpty(name, nameof(name));
      Check.NullButNotEmpty(schema, nameof(schema));
      var model = modelBuilder.Model;
      name = name ?? EFCoreConstants.Metadata.DefaultHiLoSequenceName;
      if (model.AS400().FindSequence(name, schema) == null) {
        modelBuilder.HasSequence(name, schema).IncrementsBy(10);
      }
      model.AS400().ValueGenerationStrategy = AS400ValueGenerationStrategy.SequenceHiLo;
      model.AS400().HiLoSequenceName = name;
      model.AS400().HiLoSequenceSchema = schema;
      return modelBuilder;
    }

    public static ModelBuilder ForAS400UseIdentityColumns(
        [NotNull] this ModelBuilder modelBuilder) {
      Check.NotNull(modelBuilder, nameof(modelBuilder));
      var property = modelBuilder.Model;
      property.AS400().ValueGenerationStrategy = AS400ValueGenerationStrategy.IdentityColumn;
      property.AS400().HiLoSequenceName = null;
      property.AS400().HiLoSequenceSchema = null;
      return modelBuilder;
    }

  }
}