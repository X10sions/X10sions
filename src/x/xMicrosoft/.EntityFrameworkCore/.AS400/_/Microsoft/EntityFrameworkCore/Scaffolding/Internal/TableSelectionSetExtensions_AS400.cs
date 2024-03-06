using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Internal {
  static class TableSelectionSetExtensions_AS400 {

    static readonly List<string> _schemaPatterns = new List<string>    {
      "{schema}",
      "[{schema}]"
    };

    static readonly List<string> _tablePatterns = new List<string>    {
      "{schema}.{table}",
      "[{schema}].[{table}]",
      "{schema}.[{table}]",
      "[{schema}].{table}",
      "{table}",
      "[{table}]"
    };

    public static bool AllowsAS400(this TableSelectionSet tableSelectionSet, [CanBeNull] string schemaName, [NotNull] string tableName) {
      if (tableSelectionSet == null
          || tableSelectionSet.Schemas.Count == 0
          && tableSelectionSet.Tables.Count == 0) {
        return true;
      }
      var result = false;
      //TODO: look into performance for large selection sets and numbers of tables
      if (schemaName != null) {
        foreach (var pattern in _schemaPatterns) {
          var patternToMatch = pattern.Replace("{schema}", schemaName);
          var matchingSchemaSelections = tableSelectionSet.Schemas.Where(
              s => s.Text.Equals(patternToMatch, StringComparison.OrdinalIgnoreCase))
              .ToList();
          if (matchingSchemaSelections.Any()) {
            matchingSchemaSelections.ForEach(selection => selection.IsMatched = true);
            result = true;
          }
        }
      }
      foreach (var pattern in _tablePatterns) {
        var patternToMatch = pattern.Replace("{schema}", schemaName).Replace("{table}", tableName);
        var matchingTableSelections = tableSelectionSet.Tables.Where(
            t => t.Text.Equals(patternToMatch, StringComparison.OrdinalIgnoreCase))
            .ToList();
        if (matchingTableSelections.Any()) {
          matchingTableSelections.ForEach(selection => selection.IsMatched = true);
          result = true;
        }
      }
      return result;
    }

  }
}