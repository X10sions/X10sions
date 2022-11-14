using JetBrains.Annotations;
using System;
using xEFCore.xAS400.Metadata;

namespace Microsoft.EntityFrameworkCore.Metadata {
  public static class IIndexExtensions {

    public static IAS400IndexAnnotations AS400([NotNull] this IIndex index)
        => new AS400IndexAnnotations(Check.NotNull(index, nameof(index)));

  }
}