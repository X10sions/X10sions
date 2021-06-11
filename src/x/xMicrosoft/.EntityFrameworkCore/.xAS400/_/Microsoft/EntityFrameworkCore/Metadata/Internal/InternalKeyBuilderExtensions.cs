using JetBrains.Annotations;
using xEFCore.xAS400.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal {
  public static class InternalKeyBuilderExtensions {

    public static AS400KeyBuilderAnnotations AS400([NotNull] this InternalKeyBuilder builder, ConfigurationSource configurationSource)
        => new AS400KeyBuilderAnnotations(builder, configurationSource);

  }
}