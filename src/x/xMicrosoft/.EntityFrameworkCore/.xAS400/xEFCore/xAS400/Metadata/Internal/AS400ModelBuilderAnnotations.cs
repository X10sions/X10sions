using IBM.Data.DB2.iSeries;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace xEFCore.xAS400.Metadata.Internal {
  public class AS400ModelBuilderAnnotations : AS400ModelAnnotations {

    public AS400ModelBuilderAnnotations(
        [NotNull] InternalModelBuilder internalBuilder,
        ConfigurationSource configurationSource)
        : base(new RelationalAnnotationsBuilder(internalBuilder, configurationSource)) {
    }

    public new virtual bool HiLoSequenceName([CanBeNull] string value) => SetHiLoSequenceName(value);
    public new virtual bool HiLoSequenceSchema([CanBeNull] string value) => SetHiLoSequenceSchema(value);
    public new virtual bool NamingConvention([CanBeNull] iDB2NamingConvention value) => SetNamingConvention(value);
    public new virtual bool ValueGenerationStrategy(AS400ValueGenerationStrategy? value) => SetValueGenerationStrategy(value);

  }
}
