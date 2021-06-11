using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal {
  public static class InternalRelationshipBuilderExtensions {

    public static RelationalForeignKeyBuilderAnnotations AS400([NotNull] this InternalRelationshipBuilder builder, ConfigurationSource configurationSource)
       => new RelationalForeignKeyBuilderAnnotations(builder, configurationSource);

  }
}