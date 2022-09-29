using Ardalis.SmartEnum;

namespace Common.Data {
  public sealed class DataSourceProduct : SmartEnum<DataSourceProduct> {
    //GetSchema - DataSourceProduct 
    public static readonly DataSourceProduct Access = new DataSourceProduct("ACCESS", DbSystem.Access);
    public static readonly DataSourceProduct DB2_400_SQL = new DataSourceProduct("DB2/400 SQL", DbSystem.DB2iSeries);
    public static readonly DataSourceProduct DB2_for_IBM_i = new DataSourceProduct("DB2 for IBM i", DbSystem.DB2iSeries);
    public static readonly DataSourceProduct Excel = new DataSourceProduct("EXCEL", DbSystem.Excel);
    public static readonly DataSourceProduct IBM_DB2_for_i  = new DataSourceProduct("IBM DB2 for i", DbSystem.DB2iSeries);
    public static readonly DataSourceProduct Microsoft_SQL_Server = new DataSourceProduct("Microsoft SQL Server", DbSystem.SqlServer);
    public static readonly DataSourceProduct MS_Jet = new DataSourceProduct("MS Jet", DbSystem.Access);

    private DataSourceProduct(string name, DbSystem dbSystem) : base(name, List.Count) {
      DbSystem = dbSystem;
    }

    public DbSystem DbSystem { get; }
  }
}