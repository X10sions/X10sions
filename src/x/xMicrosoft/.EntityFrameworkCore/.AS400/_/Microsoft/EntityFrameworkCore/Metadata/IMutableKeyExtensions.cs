using JetBrains.Annotations;
using xEFCore.xAS400.Metadata;

namespace Microsoft.EntityFrameworkCore.Metadata {
  public static class IMutableKeyExtensions {

    public static AS400KeyAnnotations AS400([NotNull] this IMutableKey key)
        => (AS400KeyAnnotations)(IKey)key.AS400();

  }
}