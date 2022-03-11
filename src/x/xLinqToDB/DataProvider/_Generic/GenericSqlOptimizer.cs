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

      public override ISqlExpression ConvertExpressionImpl(ISqlExpression expression, ConvertVisitor<RunOptimizationContext> visitor) { 
      expression = base.ConvertExpressionImpl(expression, visitor);
      return dataSourceInformationRow.DataSourceProduct?.DbSystem?.Name switch {
        DbSystem.Names.DB2iSeries => ConvertExpressionImpl_DB2iSeries(expression),
        _ => expression
      };
    }

  }
}
namespace LinqToDB.DataProvider.DB2iSeries {

  public class DB2iSeriesV5R4SqlOptimizer : _BaseSqlOptimizer {
    public DB2iSeriesV5R4SqlOptimizer(SqlProviderFlags sqlProviderFlags) : base(sqlProviderFlags) {    }

    public override ISqlExpression ConvertExpressionImpl(ISqlExpression expression, ConvertVisitor<RunOptimizationContext> visitor) {
      expression = base.ConvertExpressionImpl(expression, visitor );
      return ConvertExpressionImpl_DB2iSeries(expression);
    }

  }

  public class DB2iSeriesV7R4SqlOptimizer : DB2iSeriesV5R4SqlOptimizer {
    public DB2iSeriesV7R4SqlOptimizer(SqlProviderFlags sqlProviderFlags) : base(sqlProviderFlags) {    }
  }

}