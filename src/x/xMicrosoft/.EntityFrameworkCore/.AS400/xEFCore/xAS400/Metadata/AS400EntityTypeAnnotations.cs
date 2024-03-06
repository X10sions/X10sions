using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using xEFCore.xAS400.Metadata.Internal;

namespace xEFCore.xAS400.Metadata {
  public class AS400EntityTypeAnnotations : RelationalEntityTypeAnnotations, IAS400EntityTypeAnnotations {
    public AS400EntityTypeAnnotations([NotNull] IEntityType entityType)
        : base(entityType) {
    }

    public AS400EntityTypeAnnotations([NotNull] RelationalAnnotations annotations)
        : base(annotations) {
    }

    //public virtual bool IsMemoryOptimized {
    //  get => Annotations.Metadata[AS400AnnotationNames.MemoryOptimized] as bool? ?? false;
    //  set => SetIsMemoryOptimized(value);
    //}
    //protected virtual bool SetIsMemoryOptimized(bool value)
    //    => Annotations.SetAnnotation(AS400AnnotationNames.MemoryOptimized, value);

  }
}
