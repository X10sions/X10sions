using JetBrains.Annotations;
using xEFCore.xAS400.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal {
  public static class InternalPropertyBuilderExtensions {

    public static AS400PropertyBuilderAnnotations AS400([NotNull] this InternalPropertyBuilder builder, ConfigurationSource configurationSource)
      => new AS400PropertyBuilderAnnotations(builder, configurationSource);

  }
}