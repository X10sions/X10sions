using LinqToDB.DataProvider;
using System;

namespace Common.AspNetCore.Identity.Providers.LinqToDB {
  public class DataConnectionOptions : IDataConnectionOptions {
    public DataConnectionOptions(IServiceProvider serviceProvider) {
      ServiceProvider = serviceProvider;
    }
    public IServiceProvider ServiceProvider { get; }
    public string ConnectionString { get; set; }
    public IDataProvider DataProvider { get; set; }
  }
}