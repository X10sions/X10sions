using JetBrains.Annotations;
using xEFCore.xAS400.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal {
  public static class InternalModelBuilderExtensions {

    public static AS400ModelBuilderAnnotations AS400([NotNull] this InternalModelBuilder builder, ConfigurationSource configurationSource)
      => new AS400ModelBuilderAnnotations(builder, configurationSource);

  }
}