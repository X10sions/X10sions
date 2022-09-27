using LinqToDB.SqlProvider;
using LinqToDB.SqlQuery;
using Common.Data;
using Common.Data.GetSchemaTyped.DataRows;

namespace LinqToDB.DataProvider {
  public class GenericSqlOptimizer : BasicSqlOptimizer {
    public GenericSqlOptimizer(DataSourceInformationRow dataSourceInformationRow, SqlProviderFlags sqlProviderFlags) : base(sqlProviderFlags) {
      this.dataSourceInformationRow = dataSourceInformationRow;
      this.dbSystem = dataSourceInformationRow.GetDbSystem();
    }

    DataSourceInformationRow dataSourceInformationRow;
    DbSystem? dbSystem;

    public override ISqlExpression ConvertExpressionImpl(ISqlExpression expression, ConvertVisitor<RunOptimizationContext> visitor)
      => dbSystem switch {
        var _ when dbSystem == DbSystem.DB2iSeries => expression.ConvertExpressionImpl_DB2iSeries_MTGFS01(
                                               visitor,
                                               (e, v) => base.ConvertExpressionImpl(e, v),
                                               (f, pn) => AlternativeConvertToBoolean(f, 1),
                                               (e, v) => Div(e, v)),
        _ => throw new NotImplementedException($"{dataSourceInformationRow.DataSourceProductName}: v{dataSourceInformationRow.Version}")
      };

    protected override ISqlExpression ConvertFunction(SqlFunction func)
      => dbSystem switch {
        var _ when dbSystem == DbSystem.DB2iSeries => func.ConvertFunction_DB2iSeries_MTGFS01((f, wp) => ConvertFunctionParameters(f, wp), f => base.ConvertFunction(f)),
        _ => throw new NotImplementedException($"{dataSourceInformationRow.DataSourceProductName}: v{dataSourceInformationRow.Version}")
      };

    public override SqlStatement FinalizeStatement(SqlStatement statement, EvaluationContext context)
      => dbSystem switch {
        var _ when dbSystem == DbSystem.DB2iSeries => statement.Finalize_DB2iSeries_MTGFS01(
          s => base.FinalizeStatement(s, context),
          ds => GetAlternativeDelete(ds),
          us => GetAlternativeUpdate(us)
          ),
        _ => throw new NotImplementedException($"{dataSourceInformationRow.DataSourceProductName}: v{dataSourceInformationRow.Version}")
      };

  }
}