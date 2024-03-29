﻿using System.Data;
using System.Data.Common;
using LinqToDB.DataProvider;

namespace LinqToDB.Data {
  //public class GenericDataConnection<TConnection> : GenericDataConnection<TConnection>
  //  where TConnection : DbConnection, new() {

  //  public GenericDataConnection(string dataSourceProductName, Version dbVersion) : base(dataSourceProductName, dbVersion, typeof(TDataReader)) { }
  //}

  public class GenericDataConnection<TConnection, TDataReader> : DataConnection
    where TConnection : DbConnection, new()
    where TDataReader : IDataReader {

    public GenericDataConnection(string connectionString) : base(GenericDataProvider<TConnection>.GetInstance<TDataReader>(connectionString), connectionString) {
    }

    public TConnection TypedConnection => (TConnection)Connection;
  }

  //public class GenericDataConnection : DataConnection {

  //  public GenericDataConnection(string connectionString) : base(new Generic connectionString) {
  //  }

  //}

}