using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace xEFCore.xAS400.Metadata.Internal {
  public class AS400IndexBuilderAnnotations : AS400IndexAnnotations {
    public AS400IndexBuilderAnnotations(
           [NotNull] InternalIndexBuilder internalBuilder,
           ConfigurationSource configurationSource)
           : base(new RelationalAnnotationsBuilder(internalBuilder, configurationSource)) {
    }

    public new virtual bool Name([CanBeNull] string value) => SetName(value);
    public new virtual bool HasFilter([CanBeNull] string value) => SetFilter(value);
    //public new virtual bool IsClustered(bool? value) => SetIsClustered(value);

  }
}
