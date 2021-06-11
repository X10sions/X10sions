using LinqToDB.DataProvider;
using System;

namespace Common.AspNetCore.Identity.Providers.LinqToDB {
  public interface IDataConnectionOptions {
    IServiceProvider ServiceProvider { get; }
    IDataProvider DataProvider { get; set; }
    string ConnectionString { get; set; }
  }
}