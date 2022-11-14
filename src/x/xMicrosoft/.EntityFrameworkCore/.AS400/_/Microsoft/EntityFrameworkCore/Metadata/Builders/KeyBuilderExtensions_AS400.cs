using JetBrains.Annotations;
using System;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders {
  public static class KeyBuilderExtensions_AS400 {

    public static KeyBuilder ForAS400IsClustered([NotNull] this KeyBuilder keyBuilder){//TODO, bool clustered = true) {
      Check.NotNull(keyBuilder, nameof(keyBuilder));
      //TODO  keyBuilder.Metadata.AS400().IsClustered = clustered;
      return keyBuilder;
    }

  }
}