using JetBrains.Annotations;
using xEFCore.xAS400.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal {
  public static class InternalEntityTypeBuilderExtensions {

    public static AS400EntityTypeBuilderAnnotations AS400([NotNull] this InternalEntityTypeBuilder builder, ConfigurationSource configurationSource)
      => new AS400EntityTypeBuilderAnnotations(builder, configurationSource);

  }
}