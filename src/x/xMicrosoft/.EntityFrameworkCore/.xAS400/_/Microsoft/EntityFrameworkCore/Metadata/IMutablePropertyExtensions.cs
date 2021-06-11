using JetBrains.Annotations;
using xEFCore.xAS400.Metadata;

namespace Microsoft.EntityFrameworkCore.Metadata {
  public static class IMutablePropertyExtensions {

    public static AS400PropertyAnnotations AS400([NotNull] this IMutableProperty property)
      => (AS400PropertyAnnotations)((IProperty)property).AS400();

  }
}