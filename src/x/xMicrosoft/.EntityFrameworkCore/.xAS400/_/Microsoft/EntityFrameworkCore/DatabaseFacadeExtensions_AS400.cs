using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Reflection;
using xEFCore.xAS400.Infrastructure.Internal;

namespace Microsoft.EntityFrameworkCore {
  public static class DatabaseFacadeExtensions_AS400 {

    public static bool IsAS400([NotNull] this DatabaseFacade database)
        => database.ProviderName.Equals(
            typeof(AS400OptionsExtension).Assembly.Name(),
            StringComparison.Ordinal);

  }
}