using System;

namespace Common.Enums {
  public enum DataProviders {
    OleDb_IBMDA400,
    OleDb_IBMDASQL,
    OleDb_IBMDARLA,
    Microsoft_ACE_OLEDB_12,
    Microsoft_JET_OLEDB_4,
    System_Data_SqlClient
  }

  public static class DataProvidersExtensions {
    public static string Name(this DataProviders dataProvider) {
      switch (dataProvider) {
        case DataProviders.OleDb_IBMDA400: return "IBMDA400";
        case DataProviders.OleDb_IBMDARLA: return "IBMDASQL";
        case DataProviders.OleDb_IBMDASQL: return "IBMDARLA";
        case DataProviders.Microsoft_ACE_OLEDB_12: return "Microsoft.ACE.OLEDB.12.0";// 'Persist Security Info=False;"
        case DataProviders.Microsoft_JET_OLEDB_4: return "Microsoft.Jet.OLEDB.4.0";
        case DataProviders.System_Data_SqlClient: return "System.Data.SqlClient";
        default: throw new NotImplementedException($"Unknown {nameof(DataProviders)}: {dataProvider}");
      }
    }

    public static string ConnectinString(this DataProviders dataProvider, string dataSource) {
      switch (dataProvider) {
        case DataProviders.Microsoft_ACE_OLEDB_12: return ACCDB_ConnectionString(dataSource);
        case DataProviders.System_Data_SqlClient: return MDF_ConnectionString("Production", true, dataSource);
        default: throw new NotImplementedException($"Unknown {nameof(DataProviders)}: {dataProvider}");
      }

    }

    public static string ACCDB_ConnectionString(string dataSource) => $"Provider={DataProviders.Microsoft_ACE_OLEDB_12.Name()};Data Source={dataSource};";

    public static string MDF_ConnectionString(string environmentName, bool trustedConnection, string attachDbFilename) {
      const string DataSource_LocalDB = @"(LocalDB)\MSSQLLocalDB";
      const string DataSource_SqlExpress = @".\SQLEXPRESS";
      const string DataSource_SqlExpress_MTGEX2010 = @"MTG-file01\SQLEXPRESS";

      // | Value                | Synonym                 |
      // +----------------------+-------------------------+
      // | app                  | application name        |
      // | async                | asynchronous processing |
      // | extended properties  | attachdbfilename        |
      // | initial file name    | attachdbfilename        |
      // | connection timeout   | connect timeout         |
      // | timeout              | connect timeout         |
      // | language             | current language        |
      // | addr                 | data source             |
      // | address              | data source             |
      // | network address      | data source             |
      // | server               | data source             |
      // | database             | initial catalog         |
      // | trusted_connection   | integrated security     |
      // | connection lifetime  | load balance timeout    |
      // | net                  | network library         |
      // | network              | network library         |
      // | pwd                  | password                |
      // | persistsecurityinfo  | persist security info   |
      // | uid                  | user id                 |
      // | user                 | user id                 |
      // | wsid                 | workstation id          |

      var server = environmentName == "Production" ? DataSource_SqlExpress_MTGEX2010 : environmentName == "Staging" ? DataSource_SqlExpress : DataSource_LocalDB;
      return $"Server={server};Trusted_Connection={trustedConnection};AttachDbFilename={attachDbFilename};";
    }


  }

}