using IBM.Data.DB2.iSeries;

namespace LinqToDB.DataProvider.DB2iSeries {
  public class DB2iSeriesDataProvider_iDB2 : GenericDataProvider<iDB2Connection, iDB2DataReader> {
    // https://www.ibm.com/support/knowledgecenter/en/ssw_ibm_i_71/rzaik/connectkeywords.htm
    public DB2iSeriesDataProvider_iDB2(string connectionString) : base(connectionString) {
      MappingSchema.AddScalarTypes_iDB2();
    }
  }

}