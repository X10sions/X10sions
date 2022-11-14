using IBM.Data.DB2.iSeries;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using xEFCore.xAS400.Metadata.Internal;

namespace xEFCore.xAS400.Metadata {
  public class AS400ModelAnnotations : RelationalModelAnnotations, IAS400ModelAnnotations {

    public AS400ModelAnnotations([NotNull] IModel model) : base(model) {
    }

    protected AS400ModelAnnotations([NotNull] RelationalAnnotations annotations)
        : base(annotations) {
    }

    public virtual string HiLoSequenceName {
      get => (string)Annotations.Metadata[AS400AnnotationNames.HiLoSequenceName];
      [param: CanBeNull]
      set => SetHiLoSequenceName(value);
    }
    protected virtual bool SetHiLoSequenceName([CanBeNull] string value)
        => Annotations.SetAnnotation(AS400AnnotationNames.HiLoSequenceName, Check.NullButNotEmpty(value, nameof(value)));

    public virtual string HiLoSequenceSchema {
      get => (string)Annotations.Metadata[AS400AnnotationNames.HiLoSequenceSchema];
      [param: CanBeNull]
      set => SetHiLoSequenceSchema(value);
    }
    protected virtual bool SetHiLoSequenceSchema([CanBeNull] string value)
        => Annotations.SetAnnotation(AS400AnnotationNames.HiLoSequenceSchema, Check.NullButNotEmpty(value, nameof(value)));

    public virtual iDB2NamingConvention NamingConvention {
      get => (iDB2NamingConvention)Annotations.Metadata[AS400AnnotationNames.NamingConvention];
      [param: CanBeNull]
      set => SetNamingConvention(value);
    }
    protected virtual bool SetNamingConvention([CanBeNull] iDB2NamingConvention value)
      => Annotations.SetAnnotation(AS400AnnotationNames.NamingConvention, value);

    public virtual AS400ValueGenerationStrategy? ValueGenerationStrategy {
      get => (AS400ValueGenerationStrategy?)Annotations.Metadata[AS400AnnotationNames.ValueGenerationStrategy];
      set => SetValueGenerationStrategy(value);
    }
    protected virtual bool SetValueGenerationStrategy(AS400ValueGenerationStrategy? value)
      => Annotations.SetAnnotation(AS400AnnotationNames.ValueGenerationStrategy, value);

  }
}