using JetBrains.Annotations;
using System;
using xEFCore.xAS400.Metadata;

namespace Microsoft.EntityFrameworkCore.Metadata {
  public static class IEntityTypeExtensions {

    public static IAS400EntityTypeAnnotations AS400([NotNull] this IEntityType entityType)
       => new AS400EntityTypeAnnotations(Check.NotNull(entityType, nameof(entityType)));
    
  }
}