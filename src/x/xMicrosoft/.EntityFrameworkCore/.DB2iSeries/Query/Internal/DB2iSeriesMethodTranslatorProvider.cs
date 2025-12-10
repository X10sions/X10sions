using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Reflection;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Query.Internal;

public sealed class DB2iSeriesMethodTranslatorProvider : IMethodCallTranslatorProvider {
  private readonly List<IMethodCallTranslator> _translators = new();

  public DB2iSeriesMethodTranslatorProvider(RelationalMethodCallTranslatorProviderDependencies deps, ISqlExpressionFactory sql) {
    //// Register built-in translators
    _translators.Add(new DB2iSeriesStringMethodTranslator(sql));
    _translators.Add(new DB2iSeriesDateTimeMethodTranslator(sql));
    _translators.Add(new DB2iSeriesMathMethodTranslator(sql));
  }

  public SqlExpression? Translate(IModel model, SqlExpression? instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger) {
    foreach (var translator in _translators) {
      var result = translator.Translate(instance, method, arguments, logger);
      if (result != null)
        return result;
    }
    return null;
  }

  public IEnumerable<IMethodCallTranslator> Translators => _translators;


}