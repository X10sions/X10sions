using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.EntityFrameworkCore.Metadata {
  public static class IModelExtensions {

    public static IEnumerable<IProperty> GetRootEntityTypesDeclaredProperties(this IModel model) {
      return from t in model.GetRootEntityTypes()
             from p in t.GetDeclaredProperties()
             select p;
    }

    public static IEnumerable<IProperty> GetRootEntityTypesDeclaredProperties(this IModel model, Type clrTypeUnwrapNullableType) {
      return from p in model.GetRootEntityTypesDeclaredProperties()
             where p.ClrType.UnwrapNullableType() == clrTypeUnwrapNullableType
             select p;
    }

  }
}