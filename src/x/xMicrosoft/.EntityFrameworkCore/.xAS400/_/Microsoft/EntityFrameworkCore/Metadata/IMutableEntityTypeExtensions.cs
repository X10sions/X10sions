using JetBrains.Annotations;
using xEFCore.xAS400.Metadata;

namespace Microsoft.EntityFrameworkCore.Metadata {
  public static class IMutableEntityTypeExtensions {

    public static AS400EntityTypeAnnotations AS400([NotNull] this IMutableEntityType entityType)
        => (AS400EntityTypeAnnotations)((IEntityType)entityType).AS400();

  }
}