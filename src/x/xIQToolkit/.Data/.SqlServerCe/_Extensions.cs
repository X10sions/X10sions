using IQToolkit.Data.Common;
using System.Linq.Expressions;

namespace IQToolkit.Data.SqlServerCe {
  public static class _Extensions {

    public static string FormatSqlCe(this Expression expression) => FormatSqlCe(expression, new SqlCeLanguage());

    public static string FormatSqlCe(this Expression expression, QueryLanguage language) {
      var formatter = new SqlCeFormatter(language);
      formatter.Visit(expression);
      return formatter.ToString();
    }

  }
}
