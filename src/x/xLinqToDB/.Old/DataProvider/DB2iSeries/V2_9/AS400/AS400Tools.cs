using LinqToDB.Data;
using LinqToDB.DataProvider;

namespace xLinqToDB.DataProvider.DB2iSeries.V2_9.AS400 {
  public static class AS400Tools {
    static AS400Tools() {
      //DataConnection.AddDataProvider(AS400Factory.AS400ProviderName, _AS400DataProvider);
      DataConnection.AddDataProvider(_AS400DataProvider);
    }
    static readonly AS400DataProvider _AS400DataProvider = new AS400DataProvider();
    public static IDataProvider Get_AS400DataProvider() => _AS400DataProvider;
  }
}