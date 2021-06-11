using JetBrains.Annotations;
using System;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders {
  public static class EntityTypeBuilderExtensions_AS400 {

    public static EntityTypeBuilder ForAS400IsMemoryOptimized(
        [NotNull] this EntityTypeBuilder entityTypeBuilder, bool memoryOptimized = true) {
      Check.NotNull(entityTypeBuilder, nameof(entityTypeBuilder));
      //TODO  entityTypeBuilder.Metadata.AS400().IsMemoryOptimized = memoryOptimized;
      return entityTypeBuilder;
    }

    public static EntityTypeBuilder<TEntity> ForAS400IsMemoryOptimized<TEntity>(
        [NotNull] this EntityTypeBuilder<TEntity> entityTypeBuilder, bool memoryOptimized = true)
        where TEntity : class
        => (EntityTypeBuilder<TEntity>)ForAS400IsMemoryOptimized((EntityTypeBuilder)entityTypeBuilder, memoryOptimized);

  }
}