using JetBrains.Annotations;
using xEFCore.xAS400.Metadata;

namespace Microsoft.EntityFrameworkCore.Metadata {
  public static class IMutableModelExtensions {

    public static AS400ModelAnnotations AS400([NotNull] this IMutableModel model)
        => (AS400ModelAnnotations)(IModel)model.AS400();

  }
}