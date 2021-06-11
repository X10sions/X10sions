using JetBrains.Annotations;
using xEFCore.xAS400.Metadata;

namespace Microsoft.EntityFrameworkCore.Metadata {
  public static class IMutableIndexExtensions {

    public static AS400IndexAnnotations AS400([NotNull] this IMutableIndex index)
        => (AS400IndexAnnotations)((IIndex)index).AS400();

  }
}