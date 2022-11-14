using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace xEFCore.xAS400.Metadata.Internal {
  public class AS400KeyBuilderAnnotations : AS400KeyAnnotations {
    public AS400KeyBuilderAnnotations(
           [NotNull] InternalKeyBuilder internalBuilder,
           ConfigurationSource configurationSource)
           : base(new RelationalAnnotationsBuilder(internalBuilder, configurationSource)) {
    }

    public new virtual bool Name([CanBeNull] string value) => SetName(value);
    //public new virtual bool IsClustered(bool? value) => SetIsClustered(value);

  }
}
