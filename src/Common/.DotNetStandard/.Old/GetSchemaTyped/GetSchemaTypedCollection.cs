using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Common.Data.GetSchemaTyped {

  //public class GetSchemaTypedCollection< TConnection> : GetSchemaTypedCollection where TConnection : DbConnection, new() {
  //  public GetSchemaTypedCollection(string connectionString, string collectionName) : base(new TConnection { ConnectionString = connectionString }, collectionName) { }

  //}

  public class GetSchemaTypedCollection<T> : GetSchemaTypedCollection {
    public GetSchemaTypedCollection(DbConnection dbConnection, string collectionName, Func<DataRow, T> toList, string[]? restrictionValues) : base(dbConnection, collectionName, restrictionValues) {
      TypedList = DataTable.DataRowsAs(toList);
      //List = DataTable.AsEnumerable().Select(x => toList(x));
      //List = DataTable.AsEnumerable().OfType<TDataType>();
      //List = DataTable.AsEnumerable().Cast<TDataType>();
    }

    public IEnumerable<T> TypedList { get; }

  }

  public class GetSchemaTypedCollection {
    public GetSchemaTypedCollection(DbConnection dbConnection, string collectionName, string[]? restrictionValues) {
      this.dbConnection = dbConnection;
      DataTable = dbConnection.GetSchemaDataTable(collectionName, restrictionValues);
    }
    protected DbConnection dbConnection;
    public DataTable DataTable { get; }
    public string DataTableJson => JsonConvert.SerializeObject(DataTable, Formatting.Indented);
    public string DataTableColumnsJson => JsonConvert.SerializeObject(DataTable.Columns, Formatting.Indented);
  }
}