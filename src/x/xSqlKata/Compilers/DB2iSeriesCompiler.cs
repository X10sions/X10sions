namespace SqlKata.Compilers {

  public class DB2iSeriesCompiler : Compiler {
    public override string EngineCode { get; } = CustomEngineCodes.DB2iSeries;
    //protected override string parameterPlaceholder { get; set; } = "?";
    //protected override string parameterPrefix { get; set; } = "@p";
    //protected override string OpeningIdentifier { get; set; } = "\"" "[";
    //protected override string ClosingIdentifier { get; set; } = "\"" "]";
    protected override string LastId { get; set; } = "SELECT IDENTITY_VAL_LOCAL() AS id FROM SYSIBM/SYSDUMMY1";


    //public bool UseLegacyPagination { get; set; } = true;


    //public override string CompileTrue() => "1";

    //public override string CompileFalse() => "0";

    //public override string CompileLimit(SqlResult ctx) {
    //  var limit = ctx.Query.GetLimit(EngineCode);
    //  var offset = ctx.Query.GetOffset(EngineCode);

    //  if(limit == 0 && offset > 0) {
    //    ctx.Bindings.Add(offset);
    //    return "LIMIT -1 OFFSET ?";
    //  }

    //  return base.CompileLimit(ctx);
    //}

    //protected override string CompileBasicDateCondition(SqlResult ctx, BasicDateCondition condition) {
    //  var column = Wrap(condition.Column);
    //  var value = Parameter(ctx, condition.Value);

    //  var formatMap = new Dictionary<string, string> {
    //            {"date", "%Y-%m-%d"},
    //            {"time", "%H:%M:%S"},
    //            {"year", "%Y"},
    //            {"month", "%m"},
    //            {"day", "%d"},
    //            {"hour", "%H"},
    //            {"minute", "%M"},
    //        };

    //  if(!formatMap.ContainsKey(condition.Part)) {
    //    return $"{column} {condition.Operator} {value}";
    //  }

    //  var sql = $"strftime('{formatMap[condition.Part]}', {column}) {condition.Operator} cast({value} as text)";

    //  if(condition.IsNot) {
    //    return $"NOT ({sql})";
    //  }

    //  return sql;
    //}

  }
}
