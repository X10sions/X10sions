using LinqToDB.SqlProvider;
using LinqToDB.SqlQuery;
using Common.Data;
using Common.Data.GetSchemaTyped.DataRows;

namespace LinqToDB.DataProvider {

  public class GenericSqlOptimizer : _BaseSqlOptimizer {
    public GenericSqlOptimizer(DataSourceInformationRow dataSourceInformationRow, SqlProviderFlags sqlProviderFlags) : base(sqlProviderFlags) {
      this.dataSourceInformationRow = dataSourceInformationRow;
    }
    DataSourceInformationRow dataSourceInformationRow;

    public override ISqlExpression ConvertExpressionImpl(ISqlExpression expression, ConvertVisitor<RunOptimizationContext> visitor)
      => dataSourceInformationRow.DbSystemEnum   switch {
        DbSystem.Enum.DB2iSeries => expression.ConvertExpressionImpl_DB2iSeries_MTGFS01(
                                               visitor,
                                               (e, v) => base.ConvertExpressionImpl(e, v),
                                               (f, pn) => AlternativeConvertToBoolean(f, 1),
                                               (e, v) => Div(e, v)),
        _ => throw new NotImplementedException($"{dataSourceInformationRow.DataSourceProductName}: v{dataSourceInformationRow.Version}")
      };

    protected override ISqlExpression ConvertFunction(SqlFunction func)
      => dataSourceInformationRow switch { { DataSourceProductName: DataSourceInformationRow.DataSourceProductNames.DB2_for_IBM_i }
      => func.ConvertFunction_DB2iSeries_MTGFS01(
        (f, wp) => ConvertFunctionParameters(f, wp),
        f => base.ConvertFunction(f)),
        _ => throw new NotImplementedException($"{dataSourceInformationRow.DataSourceProductName}: v{dataSourceInformationRow.Version}")
      };

    public override SqlStatement Finalize(SqlStatement statement)
      => dataSourceInformationRow switch { { DataSourceProductName: DataSourceInformationRow.DataSourceProductNames.DB2_for_IBM_i }
      => statement.Finalize_DB2iSeries_MTGFS01(
        s => base.Finalize(s),
        ds => GetAlternativeDelete(ds),
        us => GetAlternativeUpdate(us)
        ),
        _ => throw new NotImplementedException($"{dataSourceInformationRow.DataSourceProductName}: v{dataSourceInformationRow.Version}")
      };

  }
}
namespace LinqToDB.DataProvider.DB2iSeries {

  public class DB2iSeriesV5R4SqlOptimizer : _BaseSqlOptimizer {
    public DB2iSeriesV5R4SqlOptimizer(SqlProviderFlags sqlProviderFlags) : base(sqlProviderFlags) { }

    public override ISqlExpression ConvertExpressionImpl(ISqlExpression expression, ConvertVisitor<RunOptimizationContext> visitor) => expression.ConvertExpressionImpl_DB2iSeries_MTGFS01(
      visitor,
      (e, v) => base.ConvertExpressionImpl(e, v),
      (f, pn) => AlternativeConvertToBoolean(f, 1),
      (e, v) => Div(e, v)
      );

  }

  public class DB2iSeriesV7R4SqlOptimizer : DB2iSeriesV5R4SqlOptimizer {
    public DB2iSeriesV7R4SqlOptimizer(SqlProviderFlags sqlProviderFlags) : base(sqlProviderFlags) { }
  }

}