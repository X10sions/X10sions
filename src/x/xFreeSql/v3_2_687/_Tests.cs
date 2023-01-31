namespace FreeSql.v3_2_687;
public static class xFreeSqlBuilderTests {

  public class INP15 {
    public string CONO15 { get; set; } = string.Empty;
  }

  public static void Test() {
    var connString = "";// appSettings.ConnectionStrings.GetDbConnectionString("System.Data.Odbc", "MartoggDB2i");
    var fsql = new xFreeSqlBuilder_v2022_12_23(xDataType.OdbcDB2iSeries)
      .UseQuoteSqlName(false)
      .UseConnectionString(connString)
      //.UseAutoSyncStructure(true) //automatically synchronize the entity structure to the database
      .Build(); //be sure to define as singleton mode
    var query = fsql.Select<INP15>().Where(x => x.CONO15 == "S1");
    var inp15s = query.ToList();
  }

}