//using LinqToDB;
//using LinqToDB.DataProvider;
//using LinqToDB.Mapping;
//using LinqToDB.SchemaProvider;
//using LinqToDB.SqlProvider;

//namespace xLinqToDB.DataProvider.DB2iSeries.V3_2_3.TB {
//  public class DB2iSeriesDynamicDataProvider_TB : DynamicDataProviderBase<DB2iSeriesProviderAdapter> {

//    DB2iSeriesDynamicDataProvider_TB(MappingSchema mappingSchema) : base(DB2iSeriesConstants.ProviderName, mappingSchema, DB2iSeriesProviderAdapter.GetInstance()) {
//      _sqlOptimizer = new DB2iSeriesSqlOptimizer_TB(SqlProviderFlags);
//    }

//    public override TableOptions SupportedTableOptions => TableOptions.IsTemporary | TableOptions.IsLocalTemporaryStructure | TableOptions.IsLocalTemporaryData;
//    public override ISqlBuilder CreateSqlBuilder(MappingSchema mappingSchema) => new DB2iSeriesSqlBuilder_TB(this,MappingSchema,GetSqlOptimizer(),SqlProviderFlags );
//    public override ISchemaProvider GetSchemaProvider() => new DB2iSeriesSchemaProvider_TB();
//    readonly DB2iSeriesSqlOptimizer_TB _sqlOptimizer;
//    public override ISqlOptimizer GetSqlOptimizer() => _sqlOptimizer;
//  }
//}