using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace xEFCore.xAS400.Metadata.Internal {
  public class AS400PropertyBuilderAnnotations : AS400PropertyAnnotations {

    public AS400PropertyBuilderAnnotations(
        [NotNull] InternalPropertyBuilder internalBuilder,
        ConfigurationSource configurationSource)
        : base(new RelationalAnnotationsBuilder(internalBuilder, configurationSource)) {
    }

    private InternalPropertyBuilder PropertyBuilder => ((Property)Property).Builder;

    protected new virtual RelationalAnnotationsBuilder Annotations => (RelationalAnnotationsBuilder)base.Annotations;
    protected override bool ShouldThrowOnConflict => false;
    protected override bool ShouldThrowOnInvalidConfiguration => Annotations.ConfigurationSource == ConfigurationSource.Explicit;

#pragma warning disable 109
    public new virtual bool ColumnName([CanBeNull] string value) => SetColumnName(value);
    public new virtual bool ColumnType([CanBeNull] string value) => SetColumnType(value);
    public new virtual bool DefaultValueSql([CanBeNull] string value) => SetDefaultValueSql(value);
    public new virtual bool ComputedColumnSql([CanBeNull] string value) => SetComputedColumnSql(value);
    public new virtual bool DefaultValue([CanBeNull] object value) => SetDefaultValue(value);
    public new virtual bool HiLoSequenceName([CanBeNull] string value) => SetHiLoSequenceName(value);
    public new virtual bool HiLoSequenceSchema([CanBeNull] string value) => SetHiLoSequenceSchema(value);

    public new virtual bool ValueGenerationStrategy(AS400ValueGenerationStrategy? value) {
      if (!SetValueGenerationStrategy(value)) {
        return false;
      }
      if (value == null) {
        HiLoSequenceName(null);
        HiLoSequenceSchema(null);
      } else if (value.Value == AS400ValueGenerationStrategy.IdentityColumn) {
        HiLoSequenceName(null);
        HiLoSequenceSchema(null);
      }
      return true;
    }

#pragma warning restore 109
  }
}