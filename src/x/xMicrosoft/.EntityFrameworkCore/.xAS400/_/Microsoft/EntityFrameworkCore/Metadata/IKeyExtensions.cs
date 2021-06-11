using JetBrains.Annotations;
using System;
using xEFCore.xAS400.Metadata;

namespace Microsoft.EntityFrameworkCore.Metadata {
  public static class IKeyExtensions {

    public static IAS400KeyAnnotations AS400([NotNull] this IKey key)
        => new AS400KeyAnnotations(Check.NotNull(key, nameof(key)));

  }
}