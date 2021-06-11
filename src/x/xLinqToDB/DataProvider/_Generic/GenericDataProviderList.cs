using System.Data;
using System.Data.Common;
using System.Collections.Generic;

namespace LinqToDB.DataProvider {
  public class GenericDataProviderList : Dictionary<string, IDataProvider> {
    GenericDataProviderList() { }

    public static Dictionary<string, IDataProvider> Instances = new Dictionary<string, IDataProvider>();

    public static GenericDataProvider<TConnection, TDataReader> GetInstance<TConnection, TDataReader>(string connectionString)
      where TConnection : DbConnection, new()
      where TDataReader : IDataReader {
      GenericDataProvider<TConnection, TDataReader> genericDataProvider;
      Instances.TryGetValue(connectionString, out var dataProvider);
      if (dataProvider == null) {
        genericDataProvider = new GenericDataProvider<TConnection, TDataReader>(connectionString);
        Instances[connectionString] = genericDataProvider;
        return genericDataProvider;
      }
      return (GenericDataProvider<TConnection, TDataReader>)dataProvider;
    }

  }
}