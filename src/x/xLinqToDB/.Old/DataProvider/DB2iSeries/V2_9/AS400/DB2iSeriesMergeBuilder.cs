using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.Linq;
using LinqToDB.Mapping;
using LinqToDB.SqlProvider;
using LinqToDB.SqlQuery;
using System.Data.SqlTypes;

namespace xLinqToDB.DataProvider.DB2iSeries.V2_9.AS400 {
  public class DB2iSeriesMergeBuilder<TTarget, TSource> : BasicMergeBuilder<TTarget, TSource>
      where TTarget : class
      where TSource : class {

    public DB2iSeriesMergeBuilder(DataConnection connection, IMergeable<TTarget, TSource> merge) : base(connection, merge) { }

    DB2iSeriesSqlBuilder_Base ProviderSpecificSqlBuilder => (DB2iSeriesSqlBuilder_Base)SqlBuilder;
    public bool MapGuidAsString => ProviderSpecificSqlBuilder.MapGuidAsString;

    protected override bool ProviderUsesAlternativeUpdate => true;

    protected override void AddSourceValue(ValueToSqlConverter valueConverter, ColumnDescriptor column, SqlDataType columnType, object value, bool isFirstRow, bool isLastRow) {
      if (value == null || value is INullable && ((INullable)value).IsNull) {
        var casttype = (columnType.DataType == DataType.Undefined) ?
          columnType.Type.GetTypeForCast(MapGuidAsString) :
          SqlDataType.GetDataType(columnType.DataType).GetiSeriesType(MapGuidAsString);
        Command.Append($"CAST(NULL AS {casttype})");
        return;
      }
      // avoid parameters in source due to low limits for parameters number in providers
      if (!valueConverter.TryConvert(Command, columnType, value)) {
        var colType = value.GetType().GetTypeForCast(MapGuidAsString);
        // we have to use parameter wrapped in a cast
        var name = GetNextParameterName();
        var fullName = SqlBuilder.Convert(name, ConvertType.NameToQueryParameter).ToString();
        Command.Append($"CAST({fullName} as {colType})");
        AddParameter(new DataParameter(name, value, column.DataType));
      }
    }
  }
}
