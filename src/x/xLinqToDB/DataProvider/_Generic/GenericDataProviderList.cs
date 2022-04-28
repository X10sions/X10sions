using System.Data;
using System.Data.Common;

namespace LinqToDB.DataProvider {
  public class GenericDataProviderList : Dictionary<string, IGenericDataProvider> {
    GenericDataProviderList() { }

    public static Dictionary<string, IGenericDataProvider> Instances = new Dictionary<string, IGenericDataProvider>();

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