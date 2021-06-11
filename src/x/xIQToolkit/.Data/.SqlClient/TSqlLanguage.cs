using IQToolkit.Data.Ado;
using IQToolkit.Data.Common;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace IQToolkit.Data.SqlClient {

  /// <summary>
  /// TSQL specific QueryLanguage
  /// </summary>
  public class TSqlLanguage : QueryLanguage {
    private readonly SqlDbTypeSystem typeSystem = new SqlDbTypeSystem();

    public TSqlLanguage() {
    }

    public override QueryTypeSystem TypeSystem => typeSystem;

    public override string Quote(string name) {
      if (name.StartsWith("[") && name.EndsWith("]")) {
        return name;
      } else if (name.IndexOf('.') > 0) {
        return "[" + string.Join("].[", name.Split(splitChars, StringSplitOptions.RemoveEmptyEntries)) + "]";
      } else {
        return "[" + name + "]";
      }
    }

    private static readonly char[] splitChars = new char[] { '.' };

    public override bool AllowsMultipleCommands => true;

    public override bool AllowSubqueryInSelectWithoutFrom => true;

    public override bool AllowDistinctInAggregates => true;

    public override Expression GetGeneratedIdExpression(MemberInfo member) => new FunctionExpression(TypeHelper.GetMemberType(member), "SCOPE_IDENTITY()", null);

    public override QueryLinguist CreateLinguist(QueryTranslator translator) => new TSqlLinguist(this, translator);

    class TSqlLinguist : QueryLinguist {
      public TSqlLinguist(TSqlLanguage language, QueryTranslator translator)
          : base(language, translator) {
      }

      public override Expression Translate(Expression expression) {
        // fix up any order-by's
        expression = OrderByRewriter.Rewrite(Language, expression);

        expression = base.Translate(expression);

        // convert skip/take info into RowNumber pattern
        expression = SkipToRowNumberRewriter.Rewrite(Language, expression);

        // fix up any order-by's we may have changed
        expression = OrderByRewriter.Rewrite(Language, expression);

        return expression;
      }

      public override string Format(Expression expression) => TSqlFormatter.Format(expression, Language);
    }

    private static TSqlLanguage _default;

    public static TSqlLanguage Default {
      get {
        if (_default == null) {
          System.Threading.Interlocked.CompareExchange(ref _default, new TSqlLanguage(), null);
        }
        return _default;
      }
    }
  }
}