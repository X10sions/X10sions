using JetBrains.Annotations;
using System;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders {
  public static class IndexBuilderExtensions_AS400 {

    public static IndexBuilder ForAS400IsClustered([NotNull] this IndexBuilder indexBuilder){//TODO, bool clustered = true) {
      Check.NotNull(indexBuilder, nameof(indexBuilder));
      //TODO  indexBuilder.Metadata.AS400().IsClustered = clustered;
      return indexBuilder;
    }

  }
}