using System;

namespace LinqToDB.Tests.Base {
  public class AllowMultipleQuery : IDisposable {
    private readonly bool _oldValue = Common.Configuration.Linq.AllowMultipleQuery;

    public AllowMultipleQuery(bool value = true) {
      Common.Configuration.Linq.AllowMultipleQuery = value;
    }

    public void Dispose() => Common.Configuration.Linq.AllowMultipleQuery = _oldValue;
  }
}