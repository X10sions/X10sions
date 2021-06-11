using LinqToDB.Configuration;
using System.Collections.Generic;

namespace LinqToDB.DataProvider.Access.V_2_9_8 {
  public class AccessFactory_Odbc : IDataProviderFactory {
    IDataProvider IDataProviderFactory.GetDataProvider(IEnumerable<NamedValue> attributes) => new AccessDataProvider_Odbc();
  }

}