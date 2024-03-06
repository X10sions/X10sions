using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;

namespace xEFCore.xAS400.Query.Internal {
  public class AS400QueryCompilationContext : RelationalQueryCompilationContext {

    public AS400QueryCompilationContext(
           [NotNull] QueryCompilationContextDependencies dependencies,
           [NotNull] ILinqOperatorProvider linqOperatorProvider,
           [NotNull] IQueryMethodProvider queryMethodProvider,
           bool trackQueryResults
      ) : base(dependencies, linqOperatorProvider, queryMethodProvider, trackQueryResults) {
    }

    public override bool IsLateralJoinSupported => true;

    public override string CreateUniqueTableAlias([NotNull] string currentAlias) {
      //  return CreateUniqueTableAlias_DB2(currentAlias);
      return base.CreateUniqueTableAlias(currentAlias);
    }

    string CreateUniqueTableAlias_DB2([NotNull] string currentAlias) {
      Check.NotNull(currentAlias, nameof(currentAlias));
      ISet<string> _tableAliasSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
      if (currentAlias.Length == 0) {
        return currentAlias;
      }
      int num = 0;
      string text = "";
      if (currentAlias.Contains(".")) {
        currentAlias = currentAlias.Substring(0, currentAlias.IndexOf(".", StringComparison.Ordinal));
      }
      text = currentAlias;
      while (_tableAliasSet.Contains(text)) {
        text = currentAlias + num++;
      }
      _tableAliasSet.Add(text);
      return text;
    }

  }
}