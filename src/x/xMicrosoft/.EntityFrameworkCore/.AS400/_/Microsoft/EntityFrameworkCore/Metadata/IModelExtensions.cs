using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using xEFCore.xAS400.Metadata;

namespace xEFCoreDev.Microsoft.EntityFrameworkCore.Metadata {
  public static class IModelExtensions {

    public static IAS400ModelAnnotations AS400([NotNull] this IModel model)
       => new AS400ModelAnnotations(Check.NotNull(model, nameof(model)));

  }
}
