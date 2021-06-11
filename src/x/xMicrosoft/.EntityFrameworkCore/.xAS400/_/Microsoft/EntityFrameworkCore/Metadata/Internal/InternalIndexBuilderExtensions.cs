using JetBrains.Annotations;
using xEFCore.xAS400.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal {
  public static class InternalIndexBuilderExtensions {

    public static AS400IndexBuilderAnnotations AS400([NotNull] this InternalIndexBuilder builder, ConfigurationSource configurationSource)
      => new AS400IndexBuilderAnnotations(builder, configurationSource);

  }
}