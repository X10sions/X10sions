using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.DataProvider;
using System.Collections.Generic;
using System.Linq;

namespace xLinqToDB.DataProvider.DB2iSeries.V2_9.RoyChase{
  public class DB2iSeriesFactory : IDataProviderFactory {

    public IDataProvider GetDataProvider(IEnumerable<NamedValue> attributes) {
      var dataProviderOptions = new DB2iSeriesConfiguration {
        MapGuidAsString = false,
        Provider = DB2iSeriesProvider.DB2,
        Version = DB2iSeriesVersion.v5r4
      };
      if (attributes == null) {
        return new DB2iSeriesDataProvider_RoyChaseV2_9(dataProviderOptions);
      }
      var attribs = attributes.ToList();
      var attrib = attribs.FirstOrDefault(_ => _.Name == nameof(DB2iSeriesConfiguration.MapGuidAsString));
      if (attrib != null) {
        bool.TryParse(attrib.Value, out var mapGuidAsString);
        dataProviderOptions.MapGuidAsString = mapGuidAsString;
      }
      var version = attribs.FirstOrDefault(_ => _.Name == "MinVer");
      dataProviderOptions.Version = version != null && version.Value == "7.1.38" ? DB2iSeriesVersion.v7r1 : DB2iSeriesVersion.v5r4;
      if (dataProviderOptions.MapGuidAsString) {
        dataProviderOptions.Provider = DB2iSeriesProvider.DB2_GAS;
      }
      return new DB2iSeriesDataProvider_RoyChaseV2_9(dataProviderOptions);
    }

  }

}