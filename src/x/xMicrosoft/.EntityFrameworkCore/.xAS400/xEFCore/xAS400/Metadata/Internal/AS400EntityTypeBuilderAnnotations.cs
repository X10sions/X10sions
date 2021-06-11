using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

namespace xEFCore.xAS400.Metadata.Internal {
  public class AS400EntityTypeBuilderAnnotations : AS400EntityTypeAnnotations {

    public AS400EntityTypeBuilderAnnotations(
        [NotNull] InternalEntityTypeBuilder internalBuilder, ConfigurationSource configurationSource)
        : base(new RelationalAnnotationsBuilder(internalBuilder, configurationSource)) {
    }

    public virtual bool ToSchema([CanBeNull] string name)
       => SetSchema(Check.NullButNotEmpty(name, nameof(name)));

    public virtual bool ToTable([CanBeNull] string name)
        => SetTableName(Check.NullButNotEmpty(name, nameof(name)));

    public virtual bool ToTable([CanBeNull] string name, [CanBeNull] string schema) {
      var originalTable = TableName;
      if (!SetTableName(Check.NullButNotEmpty(name, nameof(name)))) {
        return false;
      }
      if (!SetSchema(Check.NullButNotEmpty(schema, nameof(schema)))) {
        SetTableName(originalTable);
        return false;
      }
      return true;
    }

    //public new virtual bool IsMemoryOptimized(bool value) => SetIsMemoryOptimized(value);

  }
}
