using LinqToDB;
using LinqToDB.DataProvider;
using LinqToDB.Configuration;
using System.Collections.Generic;

namespace xLinqToDB.DataProvider.DB2iSeries.V2_9.AS400 {
  public class AS400Factory : IDataProviderFactory {
    public IDataProvider GetDataProvider(IEnumerable<NamedValue> attributes) => new AS400DataProvider();
  }
}