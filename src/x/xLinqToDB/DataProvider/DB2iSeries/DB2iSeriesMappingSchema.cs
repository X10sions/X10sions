using Common.Data.GetSchemaTyped.DataRows;
using LinqToDB.Mapping;
using System;

namespace LinqToDB.DataProvider.DB2iSeries {
  public class DB2iSeriesMappingSchema : MappingSchema {
    public DB2iSeriesMappingSchema(string? configuration) : base(configuration) {
      //this.dataSourceInformationRow = dataSourceInformationRow;
    }
    //DataSourceInformationRow dataSourceInformationRow;

    //    public static MappingSchema GetMappingSchema<T>(DataSourceInformationRow dataSourceInformationRow) where T : IDbConnection => new GenericMappingSchema();

  }

}