namespace FreeSql.v3_2_687;
public static class xFreeSqlBuilderTests {

  public class INP15 {
    public string CONO15 { get; set; } = string.Empty;
  }

  public static void Test_WithBuilder() {
    var connString = "";// appSettings.ConnectionStrings.GetDbConnectionString("System.Data.Odbc", "MartoggDB2i");
    var fsql = new xFreeSqlBuilder(xDataType.DB2iOdbc)
      .UseConnectionString(connString)
      .UseQuoteSqlName(false)
      //.UseAutoSyncStructure(true) //automatically synchronize the entity structure to the database
      .Build(); //be sure to define as singleton mode
    var query = fsql.Select<INP15>().Where(x => x.CONO15 == "S1");
    var inp15s = query.ToList();
  }

  //public static void Test_WithoutBuilder() {
  //  var connString = "";// appSettings.ConnectionStrings.GetDbConnectionString("System.Data.Odbc", "MartoggDB2i");

  //  var fsql = new DB2iProvider<IFreeSql>(connString)
  //    .UseConnectionString(xDataType.DB2iOdbc, connString, null)
  //    .UseQuoteSqlName(false)
  //    //.UseAutoSyncStructure(true) //automatically synchronize the entity structure to the database
  //    .Build(); //be sure to define as singleton mode
  //  var query = fsql.Select<INP15>().Where(x => x.CONO15 == "S1");
  //  var inp15s = query.ToList();
  //}

}