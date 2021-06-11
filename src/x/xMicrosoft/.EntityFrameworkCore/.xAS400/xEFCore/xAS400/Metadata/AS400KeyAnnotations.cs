using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using xEFCore.xAS400.Metadata.Internal;

namespace xEFCore.xAS400.Metadata {
  public class AS400KeyAnnotations : RelationalKeyAnnotations, IAS400KeyAnnotations {
    public AS400KeyAnnotations([NotNull] IKey key)
        : base(key) {
    }

    protected AS400KeyAnnotations([NotNull] RelationalAnnotations annotations)
        : base(annotations) {
    }

    //public virtual bool? IsClustered {
    //  get { return (bool?)Annotations.Metadata[AS400AnnotationNames.Clustered]; }
    //  [param: CanBeNull]
    //  set { SetIsClustered(value); }
    //}
    //protected virtual bool SetIsClustered(bool? value)
    //    => Annotations.SetAnnotation(AS400AnnotationNames.Clustered, value);

  }
}